using OpenScience.Data.Base.Models;
using System.Collections.Generic;

namespace OpenScience.Data.Users.Models
{
	public class Role : Entity
	{
		public string Name { get; set; }
		public string Alias { get; set; }

		public ICollection<RolePermission> Permissions { get; set; }
	}
}
