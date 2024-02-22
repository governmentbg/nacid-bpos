using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OpenScience.Data.Users.Models
{
	public class RolePermission
	{
		public int RoleId { get; set; }
		public int PermissionId { get; set; }
	}

	public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
	{
		public void Configure(EntityTypeBuilder<RolePermission> builder)
		{
			builder.HasKey(e => new { e.RoleId, e.PermissionId });
		}
	}
}
