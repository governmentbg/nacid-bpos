using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenScience.Data.Base.Models;
using OpenScience.Data.Institutions.Models;
using OpenScience.Data.Nomenclatures.Models;
using OpenScience.Data.Publications.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace OpenScience.Data.Classifications.Models
{
	public class Classification : Entity
	{
		public string Name { get; set; }

		public int? OrganizationId { get; set; }
		public Institution Organization { get; set; }

		public bool IsReadonly { get; set; } = false;
		public string HarvestUrl { get; set; }
		public string MetadataFormat { get; set; }

		public bool IsOpenAirePropagationEnabled { get; set; } = true;

		public int? DefaultResourceTypeId { get; set; }
		public ResourceType DefaultResourceType { get; set; }

		public int? DefaultIdentifierTypeId { get; set; }
		public ResourceIdentifierType DefaultIdentifierType { get; set; }

		public int? DefaultAccessRightId { get; set; }
		public AccessRight DefaultAccessRight { get; set; }

		public int? DefaultLicenseConditionId { get; set; }
		public LicenseType DefaultLicenseCondition { get; set; }

		public DateTime? DefaultLicenseStartDate { get; set; }

		public ICollection<string> Sets { get; set; } = new List<string>();

		public string SetName { get; set; }

		public bool IsFromHarvesting { get; set; } = false;

		public int? ParentId { get; set; }
		public Classification Parent { get; set; }

		public ICollection<Classification> Children { get; set; } = new List<Classification>();

		public ICollection<PublicationClassification> Publications { get; set; } = new List<PublicationClassification>();
	}

	public class ClassificationConfiguration : IEntityTypeConfiguration<Classification>
	{
		public void Configure(EntityTypeBuilder<Classification> builder)
		{
			builder.HasMany(e => e.Children)
				.WithOne(e => e.Parent)
				.HasForeignKey(e => e.ParentId);

			builder
				.Property(s => s.Sets)
				.HasConversion(
					s => s.Any() ? JsonConvert.SerializeObject(s) : null,
					s => s == null ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(s)
				)
				.HasColumnName("sets");
		}
	}
}
