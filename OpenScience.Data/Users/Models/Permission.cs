using OpenScience.Data.Base.Models;

namespace OpenScience.Data.Users.Models
{
	public class Permission : Entity
	{
		public string Name { get; set; }
		public string Alias { get; set; }
	}
}
