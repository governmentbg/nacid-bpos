using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OpenScience.Data.Users.Models
{
	public class UserClassification
	{
		public int UserId { get; set; }
		public int ClassificationId { get; set; }
	}

	public class UserClassificationConfiguration : IEntityTypeConfiguration<UserClassification>
	{
		public void Configure(EntityTypeBuilder<UserClassification> builder)
		{
			builder.HasKey(e => new { e.UserId, e.ClassificationId });
		}
	}
}
