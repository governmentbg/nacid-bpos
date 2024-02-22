using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenScience.Data.Institutions.Models;

namespace OpenScience.Data.Users.Models
{
	public class UserInstitution
	{
		public int UserId { get; set; }

		public int InstitutionId { get; set; }
		public Institution Institution { get; set; }
	}

	public class UserInstitutionConfiguration : IEntityTypeConfiguration<UserInstitution>
	{
		public void Configure(EntityTypeBuilder<UserInstitution> builder)
		{
			builder.HasKey(e => new { e.UserId, e.InstitutionId });
		}
	}
}
