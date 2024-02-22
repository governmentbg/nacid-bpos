using System.Collections.Generic;
using System.Linq;
using MetadataHarvesting.Models.DublinCore;
using MetadataHarvesting.Models.OpenAire;
using OpenScience.Data.Publications.Models;

namespace MetadataPublications.Converters.Extensions
{
	public static class PublicationToOpenAireMetadataExtensions
	{
		public static DublinCoreElement ToDublinCoreElement(this PublicationEntity entity, string value,
			string language = null) => new DublinCoreElement { Value = value, Language = language };

		public static OpenAireFile ToOpenAireFileLocation(this PublicationFileLocation publicationFileLocation) => new OpenAireFile {
			AccessRightsUri = publicationFileLocation.AccessRightsUri,
			Value = publicationFileLocation.FileUrl,
			MimeType = publicationFileLocation.MimeType,
			ObjectType = (ObjectType)publicationFileLocation.ObjectType
		};

		public static OpenAireAlternateIdentifiersAlternateIdentifier ToOpenAireAlternateIdentifier(this PublicationAlternateIdentifier publicationAlternateIdentifier) =>
			new OpenAireAlternateIdentifiersAlternateIdentifier { AlternateIdentifierType = publicationAlternateIdentifier.Type?.Alias, Value = publicationAlternateIdentifier.Value };

		public static OpenAireFundingReferencesFundingReference ToOpenAireFundingReference(
			this PublicationFundingReference publicationFundingReference)
		{
			return new OpenAireFundingReferencesFundingReference {
				AwardTitle = publicationFundingReference.AwardTitle,
				FunderName = publicationFundingReference.Name,
				AwardNumber = new OpenAireFundingReferencesFundingReferenceAwardNumber {
					AwardUri = publicationFundingReference.AwardURI,
					Value = publicationFundingReference.AwardTitle
				},
				FunderIdentifier = new OpenAireFundingReferencesFundingReferenceFunderIdentifier {
					Value = publicationFundingReference.Identifier
				}
			};
		}

		public static OpenAireRelatedIdentifiersRelatedIdentifier ToOpenAireRelatedIdentifier(
			this PublicationRelatedIdentifier publicationRelatedIdentifier) =>
			new OpenAireRelatedIdentifiersRelatedIdentifier {
				ResourceTypeGeneral = publicationRelatedIdentifier.ResourceTypeGeneral?.Alias,
				RelationType = publicationRelatedIdentifier.RelationType?.Alias,
				Value = publicationRelatedIdentifier.Value,
				RelatedMetadataScheme = publicationRelatedIdentifier.RelatedMetadataScheme,
				RelatedIdentifierType = publicationRelatedIdentifier.Type?.Alias,
				SchemeType = publicationRelatedIdentifier.SchemeType,
				SchemeURI = publicationRelatedIdentifier.SchemeURI
			};

		public static OpenAireSizes ToOpenAireSizes(this ICollection<PublicationSize> publicationSizes)
		{
			if (!publicationSizes.Any())
			{
				return null;
			}

			return new OpenAireSizes {
				Sizes = publicationSizes.Select(s => s.Value).ToList()
			};
		}

		public static OpenAireSubjectsSubject ToOpenAireSubject(this PublicationSubject publicationSubject) => new OpenAireSubjectsSubject {
			Language = publicationSubject?.Language?.Alias,
			Value = publicationSubject.Value
		};

		public static OpenAireContributorsContributor ToOpenAireContributor(this PublicationContributor publicationContributor)
		{
			var contributor = new OpenAireContributorsContributor {
				GivenName = publicationContributor.FirstName,
				FamilyName = publicationContributor.LastName,
				ContributorType = publicationContributor.Type?.Alias,
				NameIdentifiers = publicationContributor.Identifiers.Select(i => new OpenAireNameIdentifier {
					NameIdentifierScheme = i.Scheme?.Name,
					SchemeUri = i.Scheme?.Uri,
					Value = i.Value
				}).ToList(),
				ContributorName = new OpenAireContributorsContributorContributorName {
					NameType = (NameType)publicationContributor.NameType,
					Value = $"{publicationContributor.LastName}, {publicationContributor.FirstName}"
				}
			};

			contributor.Affiliations.Add(publicationContributor.InstitutionAffiliationName);

			return contributor;
		}

		public static OpenAireTitlesTitle ToOpenAireTitle(this PublicationTitle title)
		{
			return new OpenAireTitlesTitle {
				Language = title.Language?.Alias,
				Value = title.Value,
				TitleType = title.Type?.Alias
			};
		}

		public static OpenAireCreatorsCreator ToOpenAireCreator(this PublicationCreator creator)
		{
			var result = new OpenAireCreatorsCreator {
				Affiliations = creator.Affiliations.Select(a => a.InstitutionName).ToList(),
				CreatorName =
					new OpenAireCreatorsCreatorCreatorName {
						NameType = (NameType)creator.NameType,
						Value = creator.DisplayName
					},
				GivenName = creator.FirstName,
				FamilyName = creator.LastName,
				NameIdentifiers = creator.Identifiers.Select(i => new OpenAireNameIdentifier {
					NameIdentifierScheme = i.Scheme?.Name,
					SchemeUri = i.Scheme?.Uri,
					Value = i.Value
				}).ToList()
			};

			return result;
		}
	}
}
