using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenScience.Data.Users.Models;
using OpenScience.Services.Auth;
using OpenScience.Services.Users;
using ServerApplication.Infrastructure.Auth;
using ServerApplication.UsersModule.Dtos;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ServerApplication.UsersModule
{
	[AllowAnonymous]
	[ApiController]
	[Route("api/[controller]")]
	public class LoginController : ControllerBase
	{
		private readonly UserService userService;
		private readonly TokenService tokenService;
		private readonly AuthConfiguration authConfiguration;

		public LoginController(
			UserService userService,
			TokenService tokenService,
			IOptions<AuthConfiguration> authConfigurationOptions
			)
		{
			this.userService = userService;
			this.tokenService = tokenService;
			this.authConfiguration = authConfigurationOptions.Value;
		}

		[HttpPost("")]
		public async Task<UserLoginInfoDto> Login([FromBody]LoginDto credentials)
		{
			User user = await userService.GetUserByCredentialsAsync(credentials.Username, credentials.Password);
			var result = new UserLoginInfoDto {
				Id = user.Id,
				Fullname = user.Fullname,
				RoleAlias = user.Role.Alias,
				InstitutionIds = user.Institutions.Select(e => e.InstitutionId).ToList(),
				Token = tokenService.CreateLoginToken(
					user.Username, 
					user.Id.ToString(), 
					authConfiguration, 
					KeyValuePair.Create(ClaimTypes.Role, user.Role.Alias), 
					KeyValuePair.Create(ClaimTypes.Upn, string.Join(',', user.Institutions.Select(i => i.InstitutionId)))
				)
			};
			
			return result;
		}
	}
}
