using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenScience.Common.Constants;
using OpenScience.Data.Users.Models;
using OpenScience.Services.Users;
using ServerApplication.Infrastructure.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerApplication.UsersModule
{
	[ApiController]
	[Authorize(Policy = "RequireAdministratorRole")]
	[Route("api/[controller]")]
	public class RoleController : ControllerBase
	{
		private readonly RoleService roleService;
		private readonly UserContext userContext;

		public RoleController(
			RoleService roleService,
			UserContext userContext
			)
		{
			this.roleService = roleService;
			this.userContext = userContext;
		}

		[HttpGet]
		public async Task<IEnumerable<Role>> GetRoles([FromQuery]string textFilter)
		{
			List<string> excludeAliases = new List<string>();
			if(userContext.RoleAlias == UserRoleAliases.ORGANIZATION_ADMINISTRATOR)
			{
				excludeAliases.Add(UserRoleAliases.ADMINISTRATOR);
				excludeAliases.Add(UserRoleAliases.ORGANIZATION_ADMINISTRATOR);
			}

			return await roleService.GetRolesAsync(textFilter, excludeAliases.ToArray());
		}
	}
}
