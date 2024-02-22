using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenScience.Data.Base.Models;
using OpenScience.Data.Nomenclatures.Models;

namespace OpenScience.Data.Publications.Models
{
	public class PublicationContributorIdentifier : Entity
	{
		public int PublicationContributorId { get; set; }
		public PublicationContributor PublicationContributor { get; set; }

		public string Value { get; set; }

		public int? SchemeId { get; set; }
		public NameIdentifierScheme Scheme { get; set; }

		public int? OrganizationalSchemeId { get; set; }
		public OrganizationalIdentifierScheme OrganizationalScheme { get; set; }

		public int? ViewOrder { get; set; }
	}

	public class PublicationContributorIdentifierConfiguration : IEntityTypeConfiguration<PublicationContributorIdentifier>
	{
		public void Configure(EntityTypeBuilder<PublicationContributorIdentifier> builder)
		{
			builder.HasKey(e => e.Id);

			builder.HasOne(e => e.PublicationContributor)
				.WithMany(e => e.Identifiers)
				.HasForeignKey(e => e.PublicationContributorId);
		}
	}
}
