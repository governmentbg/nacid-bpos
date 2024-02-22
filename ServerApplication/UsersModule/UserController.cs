using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using OpenScience.Common.Constants;
using OpenScience.Common.DomainValidation;
using OpenScience.Data;
using OpenScience.Data.Users.Models;
using OpenScience.Services.Emails;
using OpenScience.Services.Users;
using OpenScience.Services.Users.Enums;
using ServerApplication.Infrastructure.Auth;
using ServerApplication.Infrastructure.Configuration;
using ServerApplication.UsersModule.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerApplication.UsersModule
{
	[ApiController]
	[Authorize(Policy = "RequireAdministratorRole")]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly AppDbContext context;
		private readonly UserContext userContext;
		private readonly UserService userService;
		private readonly DomainValidationService validator;
		private readonly EmailService emailService;
		private readonly AuthConfiguration authConfiguration; 
		private readonly EmailConfiguration emailConfiguration;

		public UserController(
			AppDbContext context,
			UserContext userContext,
			UserService userService,
			DomainValidationService validator,
			EmailService emailService,
			IOptions<AuthConfiguration> authConfigurationOptions,
			IOptions<EmailConfiguration> emailConfigurationOptions)
		{
			this.context = context;
			this.userContext = userContext;
			this.userService = userService;
			this.validator = validator;
			this.emailService = emailService;
			this.authConfiguration = authConfigurationOptions.Value;
			this.emailConfiguration = emailConfigurationOptions.Value;
		}

		[HttpGet]
		public async Task<IEnumerable<UserSearchResultDto>> SearchUsers([FromQuery]UserSearchFilterDto filter)
		{
			if(filter == null)
			{
				filter = new UserSearchFilterDto();
			}

			if(userContext.RoleAlias == UserRoleAliases.ORGANIZATION_ADMINISTRATOR)
			{
				filter.InstitutionIds = userContext.InstitutionIds;
			}

			return await userService.GetFilteredAsync(filter, UserSearchResultDto.SelectExpression);
		}

		[HttpGet("{id:int}")]
		public async Task<User> GetById([FromRoute]int id)
		{
			return await userService.GetById(id);
		}

		[HttpPut("{id:int}")]
		public async Task<User> UpdateUser([FromRoute]int id, [FromBody]User user)
		{
			userService.Update(user);
			await context.SaveChangesAsync();

			return await userService.GetById(id);
		}

		[HttpPost("Creation")]
		public async Task<User> CreateUser([FromBody]UserCreationDto userDto)
		{
			using (IDbContextTransaction transaction = context.BeginTransaction())
			{
				User user = await userService.CreateUserAsync(userDto.Username, userDto.Fullname, userDto.Email, userDto.RoleId, userDto.Orcid, userDto.InstitutionIds);
				await context.SaveChangesAsync();

				await userService.SendUserActivationMailAsync(user, authConfiguration.Issuer, emailConfiguration);
				await context.SaveChangesAsync();

				transaction.Commit();

				return user;
			}
		}

		[HttpPost("Reactivation/{userId:int}")]
		public async Task SendActivationLinkAgain([FromRoute]int userId)
		{
			User user = await userService.GetById(userId);
			if(!user.IsLocked)
			{
				validator.ThrowErrorMessage(UserErrorCode.User_UserAlreadyUnlocked);
			}

			await userService.SendUserActivationMailAsync(user, authConfiguration.Issuer, emailConfiguration);
			await context.SaveChangesAsync();
		}

		[HttpPost("Deactivation/{userId:int}")]
		public async Task<User> DeactivateUser([FromRoute]int userId)
		{
			await userService.DeactivateAsync(userId);
			await context.SaveChangesAsync();

			return await userService.GetById(userId);
		}
	}
}
