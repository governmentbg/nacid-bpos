using System;
using System.Collections.Generic;
using NacidRas.Integrations.OaiPmhProvider.Models.DublinCore;
using NacidRas.Integrations.OaiPmhProvider.Models.OpenAire;

namespace NacidRas.Integrations.OaiPmhProvider.Models.Metadata.OpenAire
{
	public class OpenAireMetadata : RecordMetadata
	{
		public OpenAireMetadata()
		{
			MetadataFormat = MetadataFormatPrefix;
		}

		public static string MetadataFormatPrefix = "oai_openaire";

		public OpenAireTitles Titles { get; set; }
		public OpenAireCreators Creators { get; set; }
		public OpenAireContributors Contributors { get; set; }
		public OpenAireAlternateIdentifiers AlternateIdentifiers { get; set; }
		public OpenAireRelatedIdentifiers RelatedIdentifiers { get; set; }
		public OpenAireIdentifier ResourceIdentifier { get; set; }
		public OpenAireSubjects Subjects { get; set; }
		public ICollection<DublinCoreElement> Publishers { get; set; } = new List<DublinCoreElement>();
		public OpenAireDates Dates { get; set; }

		public ICollection<DublinCoreElement> Languages { get; set; } = new List<DublinCoreElement>();
		public OpenAireResourceType ResourceType { get; set; }
		public OpenAireSizes Sizes { get; set; }
		public ICollection<string> Formats { get; set; } = new List<string>();
		public OpenAireVersion ResourceVersion { get; set; }
		public OpenAireRights AccessRigths { get; set; }
		public ICollection<DublinCoreElement> Descriptions { get; set; } = new List<DublinCoreElement>();
		public OpenAireFundingReferences FundingReferences { get; set; }
		public ICollection<DublinCoreElement> Sources { get; set; } = new List<DublinCoreElement>();

		public LicenseCondition LicenseCondition { get; set; }
		public ICollection<DublinCoreElement> Coverages { get; set; } = new List<DublinCoreElement>();

		public string CitationConferenceDate { get; set; }
		public string CitationConferencePlace { get; set; }
		public int? CitationEdition { get; set; }
		public int? CitationEndPage { get; set; }
		public int? CitationIssue { get; set; }
		public int? CitationStartPage { get; set; }
		public string CitationTitle { get; set; }
		public int? CitationVolume { get; set; }

		public ICollection<OpenAireFile> FileLocations { get; set; } = new List<OpenAireFile>();

		public ICollection<string> Audience { get; set; } = new List<string>();

		public OpenAireDatesDate PublicationDate { get; set; }
	}
}
