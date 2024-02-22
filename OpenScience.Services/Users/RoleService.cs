using Microsoft.EntityFrameworkCore;
using OpenScience.Data;
using OpenScience.Data.Users.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenScience.Services.Users
{
	public class RoleService
	{
		private readonly AppDbContext context;

		public RoleService(AppDbContext context)
		{
			this.context = context;
		}

		public async Task<IEnumerable<Role>> GetRolesAsync(string textFilter, params string[] excludeRoleAliases)
		{
			IQueryable<Role> query = this.context.Roles.AsNoTracking();

			if(!string.IsNullOrWhiteSpace(textFilter))
			{
				query = query.Where(e => e.Name.Trim().ToLower().Contains(textFilter.Trim().ToLower()));
			}

			if(excludeRoleAliases.Length > 0)
			{
				query = query.Where(e => !excludeRoleAliases.Contains(e.Alias));
			}

			return await query.ToListAsync();
		}
	}
}
