using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace ServerApplication.Infrastructure.Auth
{
	public class UserContext
	{
		public int UserId { get; set; }
		public List<int> InstitutionIds { get; set; } = new List<int>();
		public string RoleAlias { get; set; }
		
		public UserContext(IHttpContextAccessor contextAccessor)
		{
			UserId = int.Parse(contextAccessor.HttpContext.User.Claims.Single(c => c.Type.Equals(JwtRegisteredClaimNames.Jti)).Value);
			RoleAlias = contextAccessor.HttpContext.User.Claims.Single(c => c.Type.Equals(ClaimTypes.Role)).Value;

			string institutionIds = contextAccessor.HttpContext.User.Claims.Single(c => c.Type.Equals(ClaimTypes.Upn)).Value;
			if (!string.IsNullOrWhiteSpace(institutionIds))
			{
				foreach (string institutionId in institutionIds.Split(','))
				{
					InstitutionIds.Add(int.Parse(institutionId));
				}
			}
		}
	}
}
