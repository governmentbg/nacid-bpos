using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OpenScience.Data.Users.Models
{
	public class UserRole
	{
		public int UserId { get; set; }

		public int RoleId { get; set; }
		public Role Role { get; set; }
	}

	public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
	{
		public void Configure(EntityTypeBuilder<UserRole> builder)
		{
			builder.HasKey(e => new { e.UserId, e.RoleId });
		}
	}
}
