using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenScience.Data;
using OpenScience.Services.Users;
using ServerApplication.UsersModule.Dtos;
using System.Threading.Tasks;

namespace ServerApplication.UsersModule
{
	[AllowAnonymous]
	[ApiController]
	[Route("api/[controller]")]
	public class ActivationController : ControllerBase
	{
		private readonly AppDbContext context;
		private readonly UserService userService;

		public ActivationController(
			AppDbContext context,
			UserService userService
			)
		{
			this.context = context;
			this.userService = userService;
		}

		[HttpPost]
		public async Task ActivateUser([FromBody]UserActivationDto activationDto)
		{
			await userService.ActivateUser(activationDto.Token, activationDto.Password);
			await context.SaveChangesAsync();
		}
	}
}
