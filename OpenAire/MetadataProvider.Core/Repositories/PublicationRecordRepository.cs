using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetadataHarvesting.Core.Converters;
using MetadataHarvesting.Models;
using MetadataProvider.Core.Filters;
using MetadataPublications.Converters;
using OpenScience.Data.Publications.Enums;
using OpenScience.Data.Publications.Models;
using OpenScience.Services.Base;

namespace MetadataProvider.Core.Repositories
{
	public class PublicationRecordRepository : IRecordRepository
	{
		private readonly IDateConverter dateConverter;
		private readonly IEntityService<Publication> publicationService;
		private readonly OaiConfiguration configuration;
		private readonly Func<string, IPublicationConverter> publicationConverterFactory;
		private readonly SetSpecClassificationService setSpecClassificationService;

		public PublicationRecordRepository(IDateConverter dateConverter,
			IEntityService<Publication> publicationService,
			OaiConfiguration configuration,
			Func<string, IPublicationConverter> publicationConverterFactory,
			SetSpecClassificationService setSpecClassificationService)
		{
			this.dateConverter = dateConverter;
			this.publicationService = publicationService;
			this.configuration = configuration;
			this.publicationConverterFactory = publicationConverterFactory;
			this.setSpecClassificationService = setSpecClassificationService;
		}

		public async Task<Record> GetRecord(string identifier, string metadataPrefix)
		{
			var stringifiedId = identifier
				.Split(':')
				.Last();

			if (!int.TryParse(stringifiedId, out var id)) { return null; }

			var publication = await publicationService.SingleAsync(p =>
				p.Id == id &&
				p.Status == PublicationStatus.Published &&
				p.Classifications.Any(c => c.Classification.IsOpenAirePropagationEnabled));

			if (publication == null)
			{
				return null;
			}

			var classificationIds = publication.Classifications
				.Select(c => c.ClassificationId)
				.Distinct()
				.ToList();

			var classificationsLookup = await setSpecClassificationService.CreateLookupForClassificationSetSpecs(classificationIds);

			return CreateRecord(publication, classificationsLookup, true, metadataPrefix);
		}

		public async Task<ListContainer<Record>> GetRecords(ArgumentContainer arguments, ResumptionToken resumptionToken = null)
		{
			return await GetRecordsOrIdentifiers(arguments, resumptionToken);
		}

		public async Task<ListContainer<Record>> GetIdentifiers(ArgumentContainer arguments, ResumptionToken resumptionToken = null)
		{
			return await GetRecordsOrIdentifiers(arguments, resumptionToken, false);
		}

		private async Task<ListContainer<Record>> GetRecordsOrIdentifiers(ArgumentContainer arguments, ResumptionToken resumptionToken = null, bool includeMetadata = true)
		{
			var (newResumptionToken, filter) = await CreateTokenAndFilter(arguments, resumptionToken);

			var publications = await publicationService.GetFilteredAsync(filter);

			var classificationids = publications
				.SelectMany(p => p.Classifications.Select(c => c.ClassificationId))
				.Distinct()
				.ToList();

			var classificationSetSpecsLookup = await setSpecClassificationService.CreateLookupForClassificationSetSpecs(classificationids);

			var items = publications
				.Select(publication => CreateRecord(publication, classificationSetSpecsLookup, includeMetadata, arguments.MetadataPrefix))
				.ToList();

			newResumptionToken.Cursor = resumptionToken?.Cursor != null ?
				resumptionToken.Cursor + items.Count :
				0;

			return new ListContainer<Record> {
				ResumptionToken = newResumptionToken,
				Items = items
			};
		}

		private async Task<(ResumptionToken newResumptionToken, PmhArgumentsFilter filter)> CreateTokenAndFilter(ArgumentContainer arguments, ResumptionToken resumptionToken)
		{
			var newResumptionToken = new ResumptionToken {
				MetadataPrefix = arguments.MetadataPrefix
			};

			var filter = new PmhArgumentsFilter { Limit = configuration.PageSize };

			if (!string.IsNullOrEmpty(arguments.From) && dateConverter.TryDecode(arguments.From, out var from))
			{
				newResumptionToken.From = from;
				var fromInLocalTime = from.ToLocalTime();

				filter.From = fromInLocalTime;
			}

			if (!string.IsNullOrEmpty(arguments.Until) && dateConverter.TryDecode(arguments.Until, out var until))
			{
				newResumptionToken.Until = until;
				var untilInLocalTime = until.ToLocalTime();

				if (until.Hour == 0 && until.Minute == 0 && until.Second == 0)
				{
					untilInLocalTime = untilInLocalTime.AddDays(1);
				}

				filter.Until = untilInLocalTime;
			}

			if (!string.IsNullOrEmpty(arguments.Set))
			{
				newResumptionToken.Set = arguments.Set;

				var classification = await setSpecClassificationService.GetClassificationBySetSpec(arguments.Set);
				filter.ClassificationId = classification?.Id;
			}

			newResumptionToken.CompleteListSize = await publicationService.GetFilteredCountAsync(filter);

			if (resumptionToken?.Cursor != null)
			{
				if (resumptionToken.Cursor == 0)
				{
					filter.Offset = configuration.PageSize;
				}
				else
				{
					filter.Offset = resumptionToken.Cursor.Value + configuration.PageSize;
				}
			}

			return (newResumptionToken, filter);
		}

		private Record CreateRecord(Publication publication, IDictionary<int, string> classificationSetSpecsLookup,
			bool includeMetadata = true, string metadataFormat = null)
		{
			var identifier = $"oai:openscience:{publication.Id.ToString()}";

			var header = new Header {
				Identifier = identifier,
				Datestamp = publication.ModificationDate
			};

			foreach (var classification in publication.Classifications)
			{
				if (classificationSetSpecsLookup.TryGetValue(classification.ClassificationId, out var setSpec))
				{
					header.SetSpecs.Add(setSpec);
				}
			}

			var record = new Record { Header = header };

			if (includeMetadata && !string.IsNullOrEmpty(metadataFormat))
			{
				var converter = publicationConverterFactory.Invoke(metadataFormat);
				var metadata = converter.ConvertToMetadata(publication);

				record.RecordMetadata = metadata;
			}

			return record;
		}
	}
}
