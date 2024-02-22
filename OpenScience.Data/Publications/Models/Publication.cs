using Newtonsoft.Json;
using OpenScience.Data.Base.Models;
using OpenScience.Data.Institutions.Models;
using OpenScience.Data.Nomenclatures.Models;
using OpenScience.Data.Publications.Enums;
using OpenScience.Data.Users.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenScience.Data.Publications.Models
{
	public class Publication : Entity
	{
		public ICollection<PublicationClassification> Classifications { get; set; } = new List<PublicationClassification>();

		// Resource type 
		// https://openaire-guidelines-for-literature-repository-managers.readthedocs.io/en/v4.0.0/field_publicationtype.html
		public int? ResourceTypeId { get; set; }
		public ResourceType ResourceType { get; set; }

		// Title 
		// https://openaire-guidelines-for-literature-repository-managers.readthedocs.io/en/v4.0.0/field_title.html
		public ICollection<PublicationTitle> Titles { get; set; } = new List<PublicationTitle>();

		// Creator 
		// https://openaire-guidelines-for-literature-repository-managers.readthedocs.io/en/v4.0.0/field_creator.html
		public ICollection<PublicationCreator> Creators { get; set; } = new List<PublicationCreator>();

		// Contributor 
		// https://openaire-guidelines-for-literature-repository-managers.readthedocs.io/en/v4.0.0/field_contributor.html
		public ICollection<PublicationContributor> Contributors { get; set; } = new List<PublicationContributor>();

		// Description 
		// https://openaire-guidelines-for-literature-repository-managers.readthedocs.io/en/v4.0.0/field_description.html
		public ICollection<PublicationDescription> Descriptions { get; set; } = new List<PublicationDescription>();

		// Language
		// https://openaire-guidelines-for-literature-repository-managers.readthedocs.io/en/v4.0.0/field_language.html
		public ICollection<PublicationLanguage> Languages { get; set; } = new List<PublicationLanguage>();

		// Resource identifier
		// https://openaire-guidelines-for-literature-repository-managers.readthedocs.io/en/v4.0.0/field_resourceidentifier.html
		public string Identifier { get; set; }
		public int? IdentifierTypeId { get; set; }
		public ResourceIdentifierType IdentifierType { get; set; }

		// Publication date
		// https://openaire-guidelines-for-literature-repository-managers.readthedocs.io/en/v4.0.0/field_publicationdate.html
		public int PublishYear { get; set; }
		public int? PublishMonth { get; set; }
		public int? PublishDay { get; set; }
		[JsonIgnore]
		[NotMapped]
		public string PublishDateFormated
		{
			get
			{

				if (this.PublishMonth.HasValue && this.PublishDay.HasValue)
				{
					string yearPart = this.PublishYear.ToString();
					string monthPart = this.PublishMonth < 10 ? $"0{this.PublishMonth.Value}" : this.PublishMonth.Value.ToString();
					string datePart = this.PublishDay < 10 ? $"0{this.PublishDay.Value}" : this.PublishDay.Value.ToString();
					return $"{yearPart}-{monthPart}-{datePart}";
				}
				else
					return null;
			}
		}

		// Resource version
		// https://openaire-guidelines-for-literature-repository-managers.readthedocs.io/en/v4.0.0/field_resourceversion.html
		public string ResourceVersion { get; set; }
		public string ResourceVersionURI { get; set; }

		// Subject
		// https://openaire-guidelines-for-literature-repository-managers.readthedocs.io/en/v4.0.0/field_subject.html
		public ICollection<PublicationSubject> Subjects { get; set; } = new List<PublicationSubject>();

		// Funding reference
		// https://openaire-guidelines-for-literature-repository-managers.readthedocs.io/en/v4.0.0/field_projectid.html
		public ICollection<PublicationFundingReference> FundingReferences { get; set; } = new List<PublicationFundingReference>();

		// Related identifier
		// https://openaire-guidelines-for-literature-repository-managers.readthedocs.io/en/v4.0.0/field_relatedidentifier.html
		public ICollection<PublicationRelatedIdentifier> RelatedIdentifiers { get; set; } = new List<PublicationRelatedIdentifier>();

		// Alternate identifier
		// https://openaire-guidelines-for-literature-repository-managers.readthedocs.io/en/v4.0.0/field_alternativeidentifier.html
		public ICollection<PublicationAlternateIdentifier> AlternateIdentifiers { get; set; } = new List<PublicationAlternateIdentifier>();

		// Source
		// https://openaire-guidelines-for-literature-repository-managers.readthedocs.io/en/v4.0.0/field_source.html
		public ICollection<PublicationSource> Sources { get; set; } = new List<PublicationSource>();

		// Publisher
		// https://openaire-guidelines-for-literature-repository-managers.readthedocs.io/en/v4.0.0/field_publisher.html
		public ICollection<PublicationPublisher> Publishers { get; set; } = new List<PublicationPublisher>();

		// Coverage
		// https://openaire-guidelines-for-literature-repository-managers.readthedocs.io/en/v4.0.0/field_coverage.html
		public ICollection<PublicationCoverage> Coverages { get; set; } = new List<PublicationCoverage>();

		// Citation
		// https://openaire-guidelines-for-literature-repository-managers.readthedocs.io/en/v4.0.0/field_citationtitle.html
		public string CitationTitle { get; set; }
		public int? CitationVolume { get; set; }
		public int? CitationIssue { get; set; }
		public int? CitationStartPage { get; set; }
		public int? CitationEndPage { get; set; }
		public int? CitationEdition { get; set; }
		public string CitationConferencePlace { get; set; }
		public DateTime? CitationConferenceStartDate { get; set; }
		public DateTime? CitationConferenceEndDate { get; set; }

		// Audience
		// https://openaire-guidelines-for-literature-repository-managers.readthedocs.io/en/v4.0.0/field_audience.html
		public ICollection<PublicationAudience> Audiences { get; set; } = new List<PublicationAudience>();

		// Access right
		// https://openaire-guidelines-for-literature-repository-managers.readthedocs.io/en/v4.0.0/field_accessrights.html
		public int AccessRightId { get; set; }
		public AccessRight AccessRight { get; set; }

		// Embargo period
		// https://openaire-guidelines-for-literature-repository-managers.readthedocs.io/en/v4.0.0/field_embargoenddate.html
		public DateTime? EmbargoPeriodStart { get; set; }
		public DateTime? EmbargoPeriodEnd { get; set; }

		// License condition
		// https://openaire-guidelines-for-literature-repository-managers.readthedocs.io/en/v4.0.0/field_licensecondition.html
		public int? LicenseTypeId { get; set; }
		public LicenseType LicenseType { get; set; }

		public string OtherLicenseCondition { get; set; }
		public string OtherLicenseURL { get; set; }

		public DateTime? LicenseStartDate { get; set; }
		
		// Size
		// https://openaire-guidelines-for-literature-repository-managers.readthedocs.io/en/v4.0.0/field_size.html
		public ICollection<PublicationSize> Sizes { get; set; } = new List<PublicationSize>();

		// Format
		// https://openaire-guidelines-for-literature-repository-managers.readthedocs.io/en/v4.0.0/field_format.html
		public ICollection<PublicationFormat> Formats { get; set; } = new List<PublicationFormat>();

		public ICollection<PublicationFile> Files { get; set; } = new List<PublicationFile>();

		public ICollection<PublicationFileLocation> FileLocations { get; set; } = new List<PublicationFileLocation>();

		public PublicationStatus Status { get; set; } = PublicationStatus.Draft;

		public DateTime ModificationDate { get; set; }

		public PublicationOriginDescription OriginDescription { get; set; }

		public int? CreatedByUserId { get; set; }
		public User CreatedByUser { get; set; } 

		public int? ModeratorInstitutionId { get; set; }
		public Institution ModeratorInstitution { get; set; }

		public Publication()
		{
			// Need to compare dates without milliseconds for harvesting
			var now = DateTime.UtcNow;
			ModificationDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
		}
	}
}
