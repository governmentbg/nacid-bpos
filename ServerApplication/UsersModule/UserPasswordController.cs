using Microsoft.AspNetCore.Mvc;
using OpenScience.Common.DomainValidation;
using OpenScience.Data;
using OpenScience.Services.Users;
using OpenScience.Services.Users.Enums;
using ServerApplication.Infrastructure.Auth;
using ServerApplication.UsersModule.Dtos;
using System.Threading.Tasks;

namespace ServerApplication.UsersModule
{
	[ApiController]
	[Route("api/User")]
	public class UserPasswordController : ControllerBase
	{
		private readonly AppDbContext context;
		private readonly UserContext userContext;
		private readonly UserService userService;
		private readonly DomainValidationService validator;

		public UserPasswordController(
			AppDbContext context,
			UserContext userContext,
			UserService userService,
			DomainValidationService validator
			)
		{
			this.context = context;
			this.userContext = userContext;
			this.userService = userService;
			this.validator = validator;
		}

		[HttpPost("NewPassword")]
		public async Task ChangePassword([FromBody]UserChangePasswordDto passwordDto)
		{
			if (passwordDto.NewPassword != passwordDto.NewPasswordAgain)
			{
				validator.ThrowErrorMessage(UserErrorCode.User_ChangePasswordNewPasswordMismatch);
			}

			await userService.ChangePasswordAsync(userContext.UserId, passwordDto.OldPassword, passwordDto.NewPassword);
			await context.SaveChangesAsync();
		}
	}
}
