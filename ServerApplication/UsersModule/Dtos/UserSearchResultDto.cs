using OpenScience.Data.Users.Models;
using System;
using System.Linq.Expressions;

namespace ServerApplication.UsersModule.Dtos
{
	public class UserSearchResultDto
	{
		public int Id { get; set; }

		public string Username { get; set; }

		public string Mail { get; set; }

		public string Fullname { get; set; }

		public string Role { get; set; }

		public DateTime? CreateDate { get; set; }
		public DateTime? UpdateDate { get; set; }

		public bool IsActive { get; set; }

		public bool IsLocked { get; set; }

		public static Expression<Func<User, UserSearchResultDto>> SelectExpression
		{
			get
			{
				return e => new UserSearchResultDto {
					Id = e.Id,
					Username = e.Username,
					Mail = e.Email,
					Fullname = e.Fullname,
					Role = e.Role.Name,
					CreateDate = e.CreateDate,
					UpdateDate = e.UpdateDate,
					IsActive = e.IsActive,
					IsLocked = e.IsLocked
				};
			}
		}
	}
}
