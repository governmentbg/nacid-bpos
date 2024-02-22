using System.Linq;
using MetadataHarvesting.Models.OpenAire;
using OpenScience.Data.Nomenclatures.Models;
using OpenScience.Data.Publications.Models;

namespace MetadataPublications.Converters.Extensions
{
	public static class OpenAireMetadataToPublicationExtensions
	{
		public static PublicationCreator ToPublicationCreator(this OpenAireCreatorsCreator openAireCreator)
		{
			var affiliations = openAireCreator.Affiliations
				.Select((c, index) => new PublicationCreatorAffiliation { InstitutionName = c, ViewOrder = index + 1 }).ToList();

			var identifiers = openAireCreator.NameIdentifiers
				.Select((i, index) => new PublicationCreatorIdentifier { Value = i.Value, ViewOrder = index + 1 }).ToList();

			return new PublicationCreator {
				FirstName = openAireCreator.GivenName,
				LastName = openAireCreator.FamilyName,
				Affiliations = affiliations,
				Identifiers = identifiers
			};
		}

		public static PublicationFileLocation ToPublicationFileLocation(this OpenAireFile openAireFile) => new PublicationFileLocation {
			AccessRightsUri = openAireFile.AccessRightsUri,
			FileUrl = openAireFile.Value,
			MimeType = openAireFile.MimeType,
			ObjectType = (OpenScience.Data.Publications.Enums.ObjectType)openAireFile.ObjectType
		};

		public static PublicationContributor ToPublicationContributor(this OpenAireContributorsContributor openAireContributor, ContributorType contributorType)
		{
			var identifiers = openAireContributor.NameIdentifiers
				.Select((i, index) => new PublicationContributorIdentifier { Value = i.Value, ViewOrder = index + 1 }).ToList();

			return new PublicationContributor {
				FirstName = openAireContributor.GivenName,
				LastName = openAireContributor.FamilyName,
				Identifiers = identifiers,
				InstitutionAffiliationName = openAireContributor.Affiliations.FirstOrDefault(),
				Type = contributorType
			};
		}

		public static PublicationFundingReference ToPublicationFundingReference(this OpenAireFundingReferencesFundingReference openAireFundingReference, OrganizationalIdentifierScheme scheme)
		{
			return new PublicationFundingReference {
				AwardTitle = openAireFundingReference.AwardTitle,
				AwardNumber = openAireFundingReference.AwardNumber?.Value,
				AwardURI = openAireFundingReference.AwardNumber?.AwardUri,
				Identifier = openAireFundingReference.FunderIdentifier?.Value,
				Name = openAireFundingReference.FunderName,
				Scheme = scheme,
			};
		}

		public static PublicationTitle ToPublicationTitle(this OpenAireTitlesTitle openAireTitle, Language language, TitleType titleType)
		{
			return new PublicationTitle {
				Language = language,
				Type = titleType,
				Value = openAireTitle.Value
			};
		}

		public static PublicationRelatedIdentifier ToPublicationRelatedIdentifier(this OpenAireRelatedIdentifiersRelatedIdentifier openAireRelatedIdentifier, ResourceIdentifierType resourceIdentifierType, RelationType relationType, ResourceTypeGeneral resourceTypeGeneral)
		{
			return new PublicationRelatedIdentifier {
				Value = openAireRelatedIdentifier.Value,
				RelatedMetadataScheme = openAireRelatedIdentifier.RelatedMetadataScheme,
				SchemeURI = openAireRelatedIdentifier.SchemeURI,
				SchemeType = openAireRelatedIdentifier.SchemeType,
				Type = resourceIdentifierType,
				RelationType = relationType,
				ResourceTypeGeneral = resourceTypeGeneral
			};
		}
	}
}
