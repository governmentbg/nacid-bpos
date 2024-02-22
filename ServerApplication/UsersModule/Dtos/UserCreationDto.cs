using System.Collections.Generic;

namespace ServerApplication.UsersModule.Dtos
{
	public class UserCreationDto
	{
		public string Username { get; set; }
		public string Fullname { get; set; }
		public string Email { get; set; }
		public int RoleId { get; set; }
		public string Orcid { get; set; }
		public List<int> InstitutionIds { get; set; }
	}
}
