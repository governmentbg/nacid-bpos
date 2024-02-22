using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenScience.Data.Base.Models;

namespace OpenScience.Data.Publications.Models
{
	public class PublicationCreatorAffiliation : Entity
	{
		public int PublicationCreatorId { get; set; }
		public PublicationCreator PublicationCreator { get; set; }

		public string InstitutionName { get; set; }

		public int? ViewOrder { get; set; }
	}

	public class PublicationCreatorAffiliationConfiguration : IEntityTypeConfiguration<PublicationCreatorAffiliation>
	{
		public void Configure(EntityTypeBuilder<PublicationCreatorAffiliation> builder)
		{
			builder.HasKey(e => e.Id);

			builder.HasOne(e => e.PublicationCreator)
				.WithMany(e => e.Affiliations)
				.HasForeignKey(e => e.PublicationCreatorId);
		}
	}
}
