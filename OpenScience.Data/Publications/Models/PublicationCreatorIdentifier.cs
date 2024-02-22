using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenScience.Data.Base.Models;
using OpenScience.Data.Nomenclatures.Models;

namespace OpenScience.Data.Publications.Models
{
	public class PublicationCreatorIdentifier : Entity
	{
		public int PublicationCreatorId { get; set; }
		public PublicationCreator PublicationCreator { get; set; }

		public string Value { get; set; }

		public int SchemeId { get; set; }
		public NameIdentifierScheme Scheme { get; set; }

		public int? ViewOrder { get; set;}
	}

	public class PublicationCreatorIdentifierConfiguration : IEntityTypeConfiguration<PublicationCreatorIdentifier>
	{
		public void Configure(EntityTypeBuilder<PublicationCreatorIdentifier> builder)
		{
			builder.HasKey(e => e.Id);

			builder.HasOne(e => e.PublicationCreator)
				.WithMany(e => e.Identifiers)
				.HasForeignKey(e => e.PublicationCreatorId);
		}
	}
}
