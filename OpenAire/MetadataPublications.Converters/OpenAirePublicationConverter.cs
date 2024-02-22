using System;
using System.Collections.Generic;
using System.Linq;
using MetadataHarvesting.Core.Converters;
using MetadataHarvesting.Models;
using MetadataHarvesting.Models.DublinCore;
using MetadataHarvesting.Models.Metadata.OpenAire;
using MetadataHarvesting.Models.OpenAire;
using MetadataPublications.Converters.Extensions;
using OpenScience.Data.Nomenclatures.Models;
using OpenScience.Data.Publications.Models;
using OpenScience.Services.Nomenclatures;

namespace MetadataPublications.Converters
{
	public class OpenAirePublicationConverter : BasePublicationConverter<OpenAireMetadata>
	{
		public OpenAirePublicationConverter(Func<string, IMetadataEncoder> metadataEncoderFactory, IAliasNomenclatureService aliasNomenclatureService)
		  : base(metadataEncoderFactory, OpenAireMetadata.MetadataFormatPrefix, aliasNomenclatureService)
		{
		}

		protected override Publication Convert(OpenAireMetadata metadata)
		{
			var publication = new Publication {
				CitationIssue = metadata.CitationIssue,
				CitationTitle = metadata.CitationTitle,
				CitationVolume = metadata.CitationVolume,
				CitationStartPage = metadata.CitationStartPage,
				CitationEndPage = metadata.CitationEndPage,
				CitationEdition = metadata.CitationEdition,
				CitationConferencePlace = metadata.CitationConferencePlace,
				Identifier = metadata.ResourceIdentifier?.Value,
				Sizes = MapCollection(metadata.Sizes?.Sizes, s => new PublicationSize { Value = s }).SetViewOrder(),
				Formats = MapCollection(metadata.Formats, dc => new PublicationFormat { Value = dc.Value }).SetViewOrder(),
				Coverages = MapCollection(metadata.Coverages, dc => new PublicationCoverage { Value = dc.Value }).SetViewOrder(),
				Subjects = MapCollection(metadata.Subjects?.Items, dc => new PublicationSubject { Language = aliasNomenclatureService.FindByAlias<Language>(dc?.Language), Value = dc.Value }).SetViewOrder(),
				AlternateIdentifiers = MapCollection(metadata.AlternateIdentifiers?.Items, at => new PublicationAlternateIdentifier { Value = at.Value, Type = aliasNomenclatureService.FindByAlias<ResourceIdentifierType>(at.AlternateIdentifierType) }).SetViewOrder(),
				Publishers = MapCollection(metadata.Publishers, dc => new PublicationPublisher { Name = dc.Value }).SetViewOrder(),
				Creators = MapCollection(metadata.Creators?.Items, c => c.ToPublicationCreator()).SetViewOrder(),
				Contributors = MapCollection(metadata.Contributors?.Items, c => c.ToPublicationContributor(aliasNomenclatureService.FindByAlias<ContributorType>(c.ContributorType))).SetViewOrder(),
				ResourceVersion = metadata.ResourceVersion?.Value,
				ResourceVersionURI = metadata.ResourceVersion?.Uri,
				ResourceType = aliasNomenclatureService.FindByAlias<ResourceType>(metadata.ResourceType?.Value),
				FundingReferences = MapCollection(metadata.FundingReferences?.Items, fr => fr.ToPublicationFundingReference(aliasNomenclatureService.FindByAlias<OrganizationalIdentifierScheme>(fr.FunderIdentifier?.Value))).SetViewOrder(),
				LicenseType = aliasNomenclatureService.FindByAlias<LicenseType>(metadata.LicenseCondition?.Value),
				Sources = MapCollection(metadata.Sources, dc => new PublicationSource { Value = dc.Value }).SetViewOrder(),
				Audiences = MapCollection(metadata.Audience, a => new PublicationAudience { Type = aliasNomenclatureService.FindByAlias<AudienceType>(a) }).SetViewOrder(),
				FileLocations = MapCollection(metadata.FileLocations, f => f.ToPublicationFileLocation()).SetViewOrder()
			};

			var accessRight = aliasNomenclatureService.FindByAlias<AccessRight>(metadata.AccessRigths?.Value);
			if (accessRight != null)
			{
				publication.AccessRightId = accessRight.Id;
			}

			if (!string.IsNullOrEmpty(metadata.ResourceIdentifier?.IdentifierType))
			{
				publication.IdentifierType = aliasNomenclatureService.FindByAlias<ResourceIdentifierType>(metadata.ResourceIdentifier
						?.IdentifierType);
			}

			publication.Titles = MapCollection(metadata.Titles?.Items, t => t.ToPublicationTitle(aliasNomenclatureService.FindByAlias<Language>(t.Language), aliasNomenclatureService.FindByAlias<TitleType>(t.TitleType))).SetViewOrder();

			publication.Descriptions = MapCollection(metadata.Descriptions, dc => new PublicationDescription { Value = dc.Value, Language = aliasNomenclatureService.FindByAlias<Language>(dc.Language) }).SetViewOrder();

			publication.Languages = MapCollection(metadata.Languages, dc => ConvertToPublicationLanguage(dc.Value)).SetViewOrder();

			publication.RelatedIdentifiers = MapCollection(metadata.RelatedIdentifiers?.Items,
				ri => ri.ToPublicationRelatedIdentifier(
					aliasNomenclatureService.FindByAlias<ResourceIdentifierType>(ri.RelatedIdentifierType),
					aliasNomenclatureService.FindByAlias<RelationType>(ri.RelationType),
					aliasNomenclatureService.FindByAlias<ResourceTypeGeneral>(ri.ResourceTypeGeneral)))
				.SetViewOrder();

			if (metadata.LicenseCondition?.StartDate != null)
			{
				publication.LicenseStartDate = DateTime.Parse(metadata.LicenseCondition.StartDate);
			}

			// Has embargo period dates
			if (metadata.Dates?.Items.Any() != null)
			{
				var embargoPeriodStartDate = metadata.Dates.Items.SingleOrDefault(d => d.DateType == DateType.Accepted);
				publication.EmbargoPeriodStart = (embargoPeriodStartDate?.Value != null) ? DateTime.Parse(embargoPeriodStartDate.Value) : (DateTime?)null;

				var embargoPeriodEndDate = metadata.Dates.Items.SingleOrDefault(d => d.DateType == DateType.Available);
				publication.EmbargoPeriodEnd = (embargoPeriodEndDate?.Value != null) ? DateTime.Parse(embargoPeriodEndDate.Value) : (DateTime?)null;
			}

			if (metadata.PublicationDate != null && metadata.PublicationDate.DateType == DateType.Issued && !string.IsNullOrEmpty(metadata.PublicationDate.Value) && DateTime.TryParse(metadata.PublicationDate.Value, out DateTime publicationDate))
			{
				publication.PublishYear = publicationDate.Year;
				publication.PublishMonth = publicationDate.Month;
				publication.PublishDay = publicationDate.Day;
			}

			return publication;
		}

		protected override RecordMetadata ConvertToRecordMetadata(Publication publication)
		{
			var metadata = new OpenAireMetadata {
				CitationIssue = publication.CitationIssue,
				CitationTitle = publication.CitationTitle,
				CitationVolume = publication.CitationVolume,
				CitationStartPage = publication.CitationStartPage,
				CitationEndPage = publication.CitationEndPage,
				CitationEdition = publication.CitationEdition,
				CitationConferencePlace = publication.CitationConferencePlace,
				Creators = MapBaseListElement<PublicationCreator, OpenAireCreators, OpenAireCreatorsCreator>(publication.Creators, c => c.ToOpenAireCreator()),
				Titles = MapBaseListElement<PublicationTitle, OpenAireTitles, OpenAireTitlesTitle>(publication.Titles, t => t.ToOpenAireTitle()),
				Contributors = MapBaseListElement<PublicationContributor, OpenAireContributors, OpenAireContributorsContributor>(publication.Contributors, c => c.ToOpenAireContributor()),
				Audience = publication.Audiences.Select(a => a.Type?.Alias).ToList(),
				Coverages = publication.Coverages.Select(c => c.ToDublinCoreElement(c.Value)).ToList(),
				Descriptions = publication.Descriptions.Select(d => d.ToDublinCoreElement(d.Value, d.Language?.Alias)).ToList(),
				Formats = publication.Formats.Select(f => f.ToDublinCoreElement(f.Value)).ToList(),
				Languages = publication.Languages.Select(l => l.ToDublinCoreElement(l.Language?.Alias)).ToList(),
				Publishers = publication.Publishers.Select(p => p.ToDublinCoreElement(p.Name)).ToList(),
				Sources = publication.Sources.Select(s => s.ToDublinCoreElement(s.Value)).ToList(),
				AlternateIdentifiers = MapBaseListElement<PublicationAlternateIdentifier, OpenAireAlternateIdentifiers, OpenAireAlternateIdentifiersAlternateIdentifier>(publication.AlternateIdentifiers, i => i.ToOpenAireAlternateIdentifier()),
				FundingReferences = MapBaseListElement<PublicationFundingReference, OpenAireFundingReferences, OpenAireFundingReferencesFundingReference>(publication.FundingReferences, f => f.ToOpenAireFundingReference()),
				Subjects = MapBaseListElement<PublicationSubject, OpenAireSubjects, OpenAireSubjectsSubject>(publication.Subjects, s => s.ToOpenAireSubject()),
				Sizes = publication.Sizes.ToOpenAireSizes(),
				RelatedIdentifiers = MapBaseListElement<PublicationRelatedIdentifier, OpenAireRelatedIdentifiers, OpenAireRelatedIdentifiersRelatedIdentifier>(publication.RelatedIdentifiers, i => i.ToOpenAireRelatedIdentifier()),
				FileLocations = MapCollection(publication.FileLocations, pfl => pfl.ToOpenAireFileLocation())
			};

			if (publication.IdentifierType != null)
			{
				metadata.ResourceIdentifier = new OpenAireIdentifier { IdentifierType = publication.IdentifierType.Alias, Value = publication.Identifier };
			}

			if (publication.ResourceType != null)
			{
				metadata.ResourceType = new OpenAireResourceType { Uri = publication.ResourceType?.Uri, Value = publication.ResourceType?.Alias, ResourceTypeGeneral = "literature" };
			}

			if (publication.CitationConferenceStartDate.HasValue && publication.CitationConferenceEndDate.HasValue)
			{
				metadata.CitationConferenceDate = $"{publication.CitationConferenceStartDate.Value:yyyy-MM-dd} - {publication.CitationConferenceEndDate.Value:yyyy-MM-dd}";
			}

			if (publication.EmbargoPeriodStart.HasValue)
			{
				metadata.Dates = metadata.Dates ?? new OpenAireDates();

				metadata.Dates.Items.Add(new OpenAireDatesDate { DateType = DateType.Accepted, Value = $"{publication.EmbargoPeriodStart.Value:yyyy-MM-dd}" });
			}

			if (publication.EmbargoPeriodEnd.HasValue)
			{
				metadata.Dates = metadata.Dates ?? new OpenAireDates();

				metadata.Dates.Items.Add(new OpenAireDatesDate { DateType = DateType.Available, Value = $"{publication.EmbargoPeriodEnd.Value:yyyy-MM-dd}" });
			}

			if (!publication.EmbargoPeriodStart.HasValue && !publication.EmbargoPeriodEnd.HasValue)
			{
				metadata.PublicationDate = new OpenAireDatesDate {
					DateType = DateType.Issued
				};

				if (publication.PublishMonth.HasValue && publication.PublishDay.HasValue)
				{
					var publicationDate = new DateTime(publication.PublishYear, publication.PublishMonth.Value, publication.PublishDay.Value);
					metadata.PublicationDate.Value = publicationDate.ToString("yyyy-MM-dd");
				}
				else
				{
					metadata.PublicationDate.Value = publication.PublishYear.ToString();
				}
			}

			if (publication.LicenseType != null)
			{
				metadata.LicenseCondition = new LicenseCondition {
					StartDate = publication.LicenseStartDate?.ToString("yyyy-MM-dd"),
					Uri = publication.OtherLicenseURL,
					Value = publication.LicenseType.Alias
				};
			}

			if (!string.IsNullOrEmpty(publication.ResourceVersion))
			{
				metadata.ResourceVersion = new OpenAireVersion { Uri = publication.ResourceVersionURI, Value = publication.ResourceVersion };
			}

			if (publication.AccessRight != null)
			{
				metadata.AccessRigths = new OpenAireRights { AccessRightsType = publication.AccessRight?.Alias, AccessRightsUri = publication.AccessRight?.Uri };
			}

			return metadata;
		}

		private IList<TOutput> MapCollection<TInput, TOutput>(ICollection<TInput> collection, Func<TInput, TOutput> mappingFunc)
			=> (collection ?? Enumerable.Empty<TInput>()).Select(mappingFunc).ToList();

		private TBaseElement MapBaseListElement<TInput, TBaseElement, TMetadataElement>(IEnumerable<TInput> elements, Func<TInput, TMetadataElement> mappingFunc)
			where TBaseElement : BaseListElement<TMetadataElement>, new()
			where TMetadataElement : IXmlSerializableMetadataElement
		{
			if (elements != null && !elements.Any())
			{
				return null;
			}

			var baseElement = new TBaseElement {
				Items = elements
					.Select(mappingFunc)
					.ToList()
			};

			return baseElement;
		}
	}
}
