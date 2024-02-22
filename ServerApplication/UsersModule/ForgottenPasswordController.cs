using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenScience.Common.DomainValidation;
using OpenScience.Data;
using OpenScience.Data.Users.Models;
using OpenScience.Services.Users;
using OpenScience.Services.Users.Enums;
using ServerApplication.Infrastructure.Auth;
using ServerApplication.Infrastructure.Configuration;
using ServerApplication.UsersModule.Dtos;
using System.Threading.Tasks;

namespace ServerApplication.UsersModule
{
	[AllowAnonymous]
	[ApiController]
	[Route("api/[controller]")]
	public class ForgottenPasswordController : ControllerBase
	{
		private readonly AppDbContext context;
		private readonly UserService userService;
		private readonly DomainValidationService validator;
		private readonly AuthConfiguration authConfiguration;
		private readonly EmailConfiguration emailConfiguration;

		public ForgottenPasswordController(
			AppDbContext context,
			UserService userService,
			DomainValidationService validator,
			IOptions<AuthConfiguration> authConfigurationOptions,
			IOptions<EmailConfiguration> emailConfigurationOptions
			)
		{
			this.context = context;
			this.userService = userService;
			this.validator = validator;
			this.authConfiguration = authConfigurationOptions.Value;
			this.emailConfiguration = emailConfigurationOptions.Value;
		}

		[HttpPost]
		public async Task SendForgottenPasswordMail([FromBody]ForgottenPasswordDto model)
		{
			User user = await userService.GetUserByEmail(model.Mail);
			if(user == null)
			{
				validator.ThrowErrorMessage(UserErrorCode.User_InvalidCredentials);
			}

			if(user.IsLocked || !user.IsActive)
			{
				validator.ThrowErrorMessage(UserErrorCode.User_CannotRestoreUserPassword);
			}

			await userService.SendForgottenPasswordMailAsync(user, authConfiguration.Issuer, emailConfiguration);
			await context.SaveChangesAsync();
		}

		[HttpPost("Recovery")]
		public async Task RecoverPassword([FromBody]ForgottenPasswordRecoveryDto model)
		{
			if(model.NewPassword != model.NewPasswordAgain)
			{
				validator.ThrowErrorMessage(UserErrorCode.User_ChangePasswordNewPasswordMismatch);
			}

			await userService.RecoverPassword(model.Token, model.NewPassword);
			await context.SaveChangesAsync();
		}
	}
}
