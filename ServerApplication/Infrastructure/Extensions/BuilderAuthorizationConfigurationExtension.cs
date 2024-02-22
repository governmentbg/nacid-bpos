using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using OpenScience.Common.Constants;
using System.Security.Claims;

namespace ServerApplication.Infrastructure.Extensions
{
	public static class BuilderAuthorizationConfigurationExtension
	{
		public static void ConfigureAuthorization(this IMvcCoreBuilder builder)
		{
			builder.AddAuthorization(options => {
				options.DefaultPolicy =
					new AuthorizationPolicyBuilder()
						.RequireAuthenticatedUser()
						.Build();

				options.AddPolicy("default",
					p => p.RequireAuthenticatedUser());

				options.AddPolicy("RequireAdministratorRole", p => {
					p.RequireClaim(ClaimTypes.Role, UserRoleAliases.ADMINISTRATOR, UserRoleAliases.ORGANIZATION_ADMINISTRATOR);
				});
			});
		}
	}
}
