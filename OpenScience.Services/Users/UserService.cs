using Microsoft.EntityFrameworkCore;
using OpenScience.Common.Configuration;
using OpenScience.Common.DomainValidation;
using OpenScience.Common.DomainValidation.Enums;
using OpenScience.Data;
using OpenScience.Data.Emails.Models;
using OpenScience.Data.Nomenclatures.Models;
using OpenScience.Data.Users.Models;
using OpenScience.Services.Auth;
using OpenScience.Services.Base;
using OpenScience.Services.Emails;
using OpenScience.Services.Users.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenScience.Services.Users
{
	public class UserService : BaseEntityService<User>
	{
		private readonly DomainValidationService validator;
		private readonly PasswordHasher passwordHasher;
		private readonly EmailService emailService;

		public UserService(
			AppDbContext context,
			DomainValidationService validator,
			PasswordHasher passwordHasher,
			EmailService emailService
			)
			: base(context)
		{
			this.validator = validator;
			this.passwordHasher = passwordHasher;
			this.emailService = emailService;
		}

		public async Task<User> GetById(int id)
		{
			return await context.Users
				.AsNoTracking()
				.Include(e => e.Role)
				.Include(e => e.Institutions)
					.ThenInclude(i => i.Institution)
				.SingleAsync(e => e.Id == id);
		}

		public async Task<User> GetUserByEmail(string email)
		{
			var user = await context.Users
				.AsNoTracking()
				.SingleOrDefaultAsync(e => e.Email.Trim().ToLower() == email.Trim().ToLower());

			return user;
		}

		public async Task<User> GetUserByCredentialsAsync(string username, string password)
		{
			var user = await context.Users
				.AsNoTracking()
				.Include(e => e.Role)
				.Include(e => e.Institutions)
				.SingleOrDefaultAsync(e => e.Username.Trim() == username.Trim());
			if (user == null || !user.IsActive)
			{
				validator.ThrowErrorMessage(UserErrorCode.User_InvalidCredentials);
			}

			bool isSamePassword = passwordHasher.VerifyHashedPassword(user.PasswordHash, password, user.PasswordSalt);
			if (!isSamePassword)
			{
				validator.ThrowErrorMessage(UserErrorCode.User_InvalidCredentials);
			}

			return user;
		}

		public async Task<User> CreateUserAsync(string username, string fullname, string email, int roleId, string orcid, List<int> institutionIds)
		{
			bool isEmailTaken = await context.Users
				.AnyAsync(e => e.Email.Trim().ToLower().Contains(email.Trim().ToLower()) && e.IsActive);
			if (isEmailTaken)
			{
				validator.ThrowErrorMessage(UserErrorCode.User_EmailTaken);
			}

			var user = new User(username, fullname, email, roleId, orcid, institutionIds);
			this.Add(user);

			return user;
		}

		public override User Update(User entity)
		{
			var originalUser = context.Users
				.AsNoTracking()
				.Include(e => e.Institutions)
				.Single(e => e.Id == entity.Id);

			var forAdd = entity.Institutions.Where(e => !originalUser.Institutions.Any(oi => oi.InstitutionId == e.InstitutionId))
				.Select(e => new UserInstitution { UserId = e.UserId, InstitutionId = e.InstitutionId });
			if (forAdd.Any())
			{
				context.Set<UserInstitution>().AddRange(forAdd);
			}

			var forDelete = originalUser.Institutions.Where(e => !entity.Institutions.Any(i => e.InstitutionId == i.InstitutionId));
			if (forDelete.Any())
			{
				context.Set<UserInstitution>().RemoveRange(forDelete);
			}

			entity.PasswordHash = originalUser.PasswordHash;
			entity.PasswordSalt = originalUser.PasswordSalt;
			entity.UpdateDate = DateTime.UtcNow;

			context.Entry(entity).State = EntityState.Modified;

			return entity;
		}

		public async Task DeactivateAsync(int id)
		{
			User user = await GetByIdAsync(id);
			if(user == null)
			{
				validator.ThrowErrorMessage(SystemErrorCode.System_IncorrectParameters);
			}

			if (!user.IsActive)
			{
				validator.ThrowErrorMessage(UserErrorCode.User_UserAlreadyDeactivated);
			}

			user.Deactivate();
			context.Entry(user).State = EntityState.Modified;
		}

		public async Task<Email> SendUserActivationMailAsync(User user, string activationHost, IEmailConfiguration emailConfiguration)
		{
			PasswordToken passwordToken = new PasswordToken(user.Id, 20160);
			context.PasswordTokens.Add(passwordToken);

			var payload = new {
				Name = user.Fullname,
				Username = user.Username,
				ActivationLink = $"{activationHost}/userActivation?token={passwordToken.Value}"
			};
			Email email = await emailService.ComposeEmailAsync(EmailTypeAlias.UserActivation, payload, user.Email);
			context.Emails.Add(email);

			emailService.SendEmail(email, emailConfiguration.FromName, emailConfiguration.FromAddress, emailConfiguration);

			return email;
		}

		public async Task<Email> SendForgottenPasswordMailAsync(User user, string forgottenPasswordHost, IEmailConfiguration emailConfiguration)
		{
			PasswordToken passwordToken = new PasswordToken(user.Id, 20160);
			context.PasswordTokens.Add(passwordToken);

			var payload = new {
				Name = user.Fullname,
				Username = user.Username,
				ForgottenPasswordLink = $"{forgottenPasswordHost}/passwordRecovery?token={passwordToken.Value}"
			};
			Email email = await emailService.ComposeEmailAsync(EmailTypeAlias.ForgottenPassword, payload, user.Email);
			context.Emails.Add(email);

			emailService.SendEmail(email, emailConfiguration.FromName, emailConfiguration.FromAddress, emailConfiguration);

			return email;
		}

		public async Task ActivateUser(string token, string password)
		{
			if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(password))
			{
				validator.ThrowErrorMessage(SystemErrorCode.System_IncorrectParameters);
			}

			PasswordToken passwordToken = await context.PasswordTokens
				.Include(e => e.User)
				.SingleAsync(e => e.Value == token);
			if (passwordToken.IsUsed)
			{
				validator.ThrowErrorMessage(UserErrorCode.User_ActivationTokenAlreadyUsed);
			}

			if (passwordToken.ExpirationTime < DateTime.UtcNow)
			{
				validator.ThrowErrorMessage(UserErrorCode.User_ActivationTokenExpired);
			}

			passwordToken.Use();

			string salt = passwordHasher.GenerateSalt();
			string hash = passwordHasher.HashPassword(password, salt);
			passwordToken.User.Activate(hash, salt);
		}

		public async Task ChangePasswordAsync(int userId, string oldPassword, string newPassword)
		{
			User user = await SingleAsync(e => e.Id == userId);
			if(!passwordHasher.VerifyHashedPassword(user.PasswordHash, oldPassword, user.PasswordSalt))
			{
				validator.ThrowErrorMessage(UserErrorCode.User_ChangePasswordOldPasswordMismatch);
			}

			string newPasswordSalt = passwordHasher.GenerateSalt();
			string newPasswordHash = passwordHasher.HashPassword(newPassword, newPasswordSalt);
			user.ChangePassword(newPasswordHash, newPasswordSalt);

			Update(user);
		}

		public async Task RecoverPassword(string token, string password)
		{
			if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(password))
			{
				validator.ThrowErrorMessage(SystemErrorCode.System_IncorrectParameters);
			}

			PasswordToken passwordToken = await context.PasswordTokens
				.Include(e => e.User)
				.SingleAsync(e => e.Value == token);
			if (passwordToken.IsUsed)
			{
				validator.ThrowErrorMessage(UserErrorCode.User_PasswordRecoveryTokenAlreadyUsed);
			}

			if (passwordToken.ExpirationTime < DateTime.UtcNow)
			{
				validator.ThrowErrorMessage(UserErrorCode.User_PasswordRecoveryTokenExpired);
			}

			passwordToken.Use();

			string salt = passwordHasher.GenerateSalt();
			string hash = passwordHasher.HashPassword(password, salt);
			passwordToken.User.ChangePassword(hash, salt);
		}
	}
}
