using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MetadataHarvesting.Core.Converters;
using MetadataHarvesting.Models;
using MetadataHarvesting.Models.DublinCore;
using MetadataPublications.Converters.Extensions;
using OpenScience.Data.Nomenclatures.Models;
using OpenScience.Data.Publications.Models;
using OpenScience.Services.Nomenclatures;

namespace MetadataPublications.Converters
{
	public class DublinCorePublicationConverter : BasePublicationConverter<DublinCoreMetadata>
	{
		private const string ContributorTypeAlias = "Other";
		private const string DefaultLanguageAlias = "bg";

		private readonly IDictionary<string, string> dublinCoreTypeToResourceTypeMappings = new Dictionary<string, string> {
			{ "event", "conference object" },
			{ "movingimage", "moving image" },
			{ "stillimage", "still image" }
		};

		public DublinCorePublicationConverter(Func<string, IMetadataEncoder> metadataEncoderFactory, IAliasNomenclatureService aliasNomenclatureService)
			: base(metadataEncoderFactory, DublinCoreMetadata.MetadataFormatPrefix, aliasNomenclatureService)
		{
		}

		protected override RecordMetadata ConvertToRecordMetadata(Publication publication)
		{
			var metadata = new DublinCoreMetadata {
				Title = Convert(publication.Titles, pt => ConvertToDublinCoreElement(pt.Value, pt.Language?.Name)),
				Creator = Convert(publication.Creators, c => ConvertToDublinCoreElement(c.DisplayName, c.Language)),
				Contributor = Convert(publication.Contributors, c => ConvertToDublinCoreElement($"{c.LastName}, {c.FirstName}")),
				Format = Convert(publication.Formats, f => ConvertToDublinCoreElement(f.Value)),
				Publisher = Convert(publication.Publishers, p => ConvertToDublinCoreElement(p.Name)),
				Description = Convert(publication.Descriptions, d => ConvertToDublinCoreElement(d.Value, d.Language?.Name)),
				Subject = Convert(publication.Subjects, s => ConvertToDublinCoreElement(s.Value, s.Language?.Alias)),
				Language = Convert(publication.Languages, l => ConvertToDublinCoreElement(l.Language.Name, "en")),
				Coverage = Convert(publication.Coverages, c => ConvertToDublinCoreElement(c.Value)),
				Source = Convert(publication.Sources, c => ConvertToDublinCoreElement(c.Value)),
			};

			var publishDate = new DateTime(publication.PublishYear, publication.PublishMonth ?? 1, publication.PublishDay ?? 1);
			metadata.Date.Add(publishDate);

			if (publication.ResourceType != null)
			{
				var type = ConvertToDublinCoreElement(publication.ResourceType?.Alias);
				metadata.Type.Add(type);
			}

			if (publication.EmbargoPeriodStart.HasValue)
			{
				metadata.Date.Add(publication.EmbargoPeriodStart.Value);
			}

			if (publication.EmbargoPeriodEnd.HasValue)
			{
				metadata.Date.Add(publication.EmbargoPeriodEnd.Value);
			}

			metadata.Rights.Add(publication.AccessRight?.Alias);
			metadata.Identifier.Add(publication.Identifier);

			return metadata;
		}

		protected override Publication Convert(DublinCoreMetadata metadata)
		{
			var publication = new Publication {
				Titles = Convert(metadata.Title, dc => new PublicationTitle { Value = dc.Value, Language = aliasNomenclatureService.FindByAlias<Language>(dc.Language) }).SetViewOrder(),
				Creators = Convert(metadata.Creator, ConvertToPublicationCreator).Where(c => c != null).ToList().SetViewOrder(),
				Contributors = Convert(metadata.Contributor, ConvertToPublicationContributor).Where(c => c != null).ToList().SetViewOrder(),
				Publishers = Convert(metadata.Publisher, dc => new PublicationPublisher { Name = dc.Value }).SetViewOrder(),
				Descriptions = Convert(metadata.Description, dc => new PublicationDescription { Value = dc.Value, Language = aliasNomenclatureService.FindByAlias<Language>(dc.Language) }).SetViewOrder(),
				Formats = Convert(metadata.Format, dc => new PublicationFormat { Value = dc.Value }).SetViewOrder(),
				Sources = Convert(metadata.Source, dc => new PublicationSource { Value = dc.Value }).SetViewOrder(),
				Subjects = Convert(metadata.Subject, dc => new PublicationSubject { Value = dc.Value }).SetViewOrder(),
				Coverages = Convert(metadata.Coverage, dc => new PublicationCoverage { Value = dc.Value }).SetViewOrder(),
				ResourceType = Convert(metadata.Type, ConvertToResourceType).FirstOrDefault(),
				Languages = Convert(metadata.Language, dc => ConvertToPublicationLanguage(dc.Value)).Where(l => l != null).ToList().SetViewOrder()
			};

			metadata.Date = metadata.Date.Where(d => d.HasValue).OrderBy(d => d.Value).ToList();

			var publishDate = metadata.Date.FirstOrDefault();
			if (publishDate != null)
			{
				publication.PublishYear = publishDate.Value.Year;
				publication.PublishMonth = publishDate.Value.Month;
				publication.PublishDay = publishDate.Value.Day;
			}

			if (metadata.Date.Count > 3)
			{
				publication.EmbargoPeriodStart = metadata.Date[1];
				publication.EmbargoPeriodEnd = metadata.Date[2];
			}

			publication.Identifier =
				(metadata.Identifier.FirstOrDefault(dc => Uri.IsWellFormedUriString(dc.Value, UriKind.Absolute)) ??
				 metadata.Identifier.FirstOrDefault())?.Value;

			return publication;
		}

		private ResourceType ConvertToResourceType(DublinCoreElement dublinCoreElement)
		{
			var resourceType = dublinCoreElement.Value.ToLower().Trim();

			if (dublinCoreTypeToResourceTypeMappings.ContainsKey(resourceType))
			{
				resourceType = dublinCoreTypeToResourceTypeMappings[resourceType];
			}

			return aliasNomenclatureService.FindByAlias<ResourceType>(resourceType);
		}

		private PublicationContributor ConvertToPublicationContributor(DublinCoreElement dublinCoreElement)
		{
			var names = dublinCoreElement.Value
				.Split(',')
				.Select(s => s.Trim())
				.ToList();

			if (names.Count < 2)
			{
				return null;
			}

			var firstName = names[1];
			var lastName = names[0];

			return new PublicationContributor {
				FirstName = firstName,
				LastName = lastName,
				Type = aliasNomenclatureService.FindByAlias<ContributorType>(ContributorTypeAlias)
			};
		}

		private PublicationCreator ConvertToPublicationCreator(DublinCoreElement dublinCoreElement)
		{
			var names = dublinCoreElement.Value
				.Split(',')
				.Select(s => s.Trim())
				.ToList();

			if (names.Count < 2)
			{
				return null;
			}

			var firstName = names[1];
			var lastName = names[0];

			return new PublicationCreator {
				FirstName = firstName,
				LastName = lastName,
				Language = dublinCoreElement.Language
			};
		}

		private DublinCoreElement ConvertToDublinCoreElement(string value, string language = null) => new DublinCoreElement { Value = value, Language = language };

		private IList<TResult> Convert<TInput, TResult>(IEnumerable<TInput> elements, Func<TInput, TResult> selectExpression) => elements.Select(selectExpression).ToList();
	}
}
