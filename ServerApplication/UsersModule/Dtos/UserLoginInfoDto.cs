using System.Collections.Generic;

namespace ServerApplication.UsersModule.Dtos
{
	public class UserLoginInfoDto
	{
		public string Token { get; set; }

		public int Id { get; set; }
		public string Fullname { get; set; }
		public string RoleAlias { get; set; }
		public List<int> InstitutionIds { get; set; }
	}
}
