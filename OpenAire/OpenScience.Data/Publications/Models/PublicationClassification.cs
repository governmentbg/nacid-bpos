using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenScience.Data.Classifications.Models;

namespace OpenScience.Data.Publications.Models
{
	public class PublicationClassification
	{
		public int ClassificationId { get; set; }
		public Classification Classification { get; set; }
		public int PublicationId { get; set; }
		public Publication Publication { get; set; }
	}

	public class PublicationClassificationConfiguration : IEntityTypeConfiguration<PublicationClassification>
	{
		public void Configure(EntityTypeBuilder<PublicationClassification> builder)
		{
			builder.HasKey(e => new { e.PublicationId, e.ClassificationId });

			builder.HasOne(e => e.Publication)
				.WithMany(e => e.Classifications)
				.HasForeignKey(e => e.PublicationId);

			builder.HasOne(e => e.Classification)
				.WithMany(e => e.Publications)
				.HasForeignKey(e => e.ClassificationId);
		}
	}
}
