using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NacidRas.Ems.Models;
using NacidRas.Infrastructure.Linq;
using NacidRas.Integrations.OaiPmhProvider.Contracts;
using NacidRas.Integrations.OaiPmhProvider.Converters;
using NacidRas.Integrations.OaiPmhProvider.Models;
using NacidRas.Integrations.OaiPmhProvider.Models.DublinCore;
using NacidRas.Ras;
using NacidRas.RasRegister;
using NacidRas.Register;

namespace NacidRas.Integrations.OaiPmhProvider.Repositories
{
	public class DissertaionRecordRepository : IRecordRepository
	{
		private readonly RasDbContext rasContext;
		private readonly Func<string, IDissertationToMetadataConverter> metadataConverterFactory;
		private readonly ISetRepository setRepository;
		private readonly OaiConfiguration configuration;
		private readonly IDateConverter dateConverter;

		public DissertaionRecordRepository(RasDbContext rasContext, Func<string, IDissertationToMetadataConverter> metadataConverterFactory, ISetRepository setRepository, OaiConfiguration configuration, IDateConverter dateConverter)
		{
			this.rasContext = rasContext;
			this.metadataConverterFactory = metadataConverterFactory;
			this.setRepository = setRepository;
			this.configuration = configuration;
			this.dateConverter = dateConverter;
		}

		public Record GetRecord(string identifier, string metadataPrefix)
		{
			var stringifiedId = identifier
				.Split(':')
				.Last();

			if (int.TryParse(stringifiedId, out var id))
			{
				var predicate = PredicateBuilder.True<AcademicDegreePart>();

				var part = BuildQuery(predicate)
					.SingleOrDefault(p => p.Entity.Dissertation.Id == id);

				return MapAcademicDegreeToRecord(part, metadataPrefix, true);
			}

			return null;
		}

		public ListContainer<Record> GetRecords(ArgumentContainer arguments, ResumptionToken resumptionToken = null) => GetRecordsOrIdentifiers(arguments, resumptionToken);

		public ListContainer<Record> GetIdentifiers(ArgumentContainer arguments, ResumptionToken resumptionToken = null) => GetRecordsOrIdentifiers(arguments, resumptionToken, false);

		private ListContainer<Record> GetRecordsOrIdentifiers(ArgumentContainer arguments, ResumptionToken resumptionToken = null, bool includeMetadata = true)
		{
			var predicate = PredicateBuilder.True<AcademicDegreePart>();

			var newResumptionToken = new ResumptionToken {
				MetadataPrefix = arguments.MetadataPrefix
			};

			if (!string.IsNullOrEmpty(arguments.From) && dateConverter.TryDecode(arguments.From, out var from))
			{
				newResumptionToken.From = from;
				var fromInLocalTime = from.ToLocalTime();

				predicate = predicate.And(p => p.CreationDate >= fromInLocalTime);
			}

			if (!string.IsNullOrEmpty(arguments.Until) && dateConverter.TryDecode(arguments.Until, out var until))
			{
				newResumptionToken.Until = until;
				var untilInLocalTime = until.ToLocalTime();

				if (until.Hour == 0 && until.Minute == 0 && until.Second == 0)
				{
					untilInLocalTime = untilInLocalTime.AddDays(1);
				}

				predicate = predicate.And(p => p.CreationDate <= untilInLocalTime);
			}

			if (!string.IsNullOrEmpty(arguments.Set))
			{
				newResumptionToken.Set = arguments.Set;
				var researchAreaName = arguments.Set
					.Split(':')
					.Last();

				predicate = predicate
					.And(p => p.Entity.ResearchArea.Name == researchAreaName);
			}


			var offset = 0;

			if (resumptionToken?.Cursor != null)
			{
				if (resumptionToken.Cursor == 0)
				{
					offset = configuration.PageSize;
				}
				else
				{
					offset = resumptionToken.Cursor.Value + configuration.PageSize;
				}
			}

			var query = BuildQuery(predicate);
			newResumptionToken.CompleteListSize = query.Count();

			var items = query
				.OrderBy(d => d.CreationDate)
				.ThenBy(d => d.Id)
				.Skip(offset)
				.Take(configuration.PageSize)
				.ToList()
				.Select(p => MapAcademicDegreeToRecord(p, arguments.MetadataPrefix, includeMetadata))
				.ToList();

			newResumptionToken.Cursor = resumptionToken?.Cursor != null ?
				resumptionToken.Cursor + items.Count :
				0;

			return new ListContainer<Record> { Items = items, ResumptionToken = newResumptionToken };
		}


		private Record MapAcademicDegreeToRecord(AcademicDegreePart part, string metadataPrefix, bool includeMetadata)
		{
			var identifier = $"oai:openras.nacid.bg:{part.Entity.Dissertation.Id.ToString()}";
			var header = new Header { Identifier = identifier, Datestamp = part.CreationDate };

			var record = new Record { Header = header };

			if (includeMetadata)
			{
				var lotId = rasContext
					.RasCommit
					.Where(c => c.Id == part.CommitId)
					.Select(c => c.LotId)
					.SingleOrDefault();

				var author = (Person)null;

				if (lotId != default)
				{
					author = rasContext
					   .RasCommit
					   .Include(c => c.PersonPart.Entity)
					   .SingleOrDefault(c =>
						   c.LotId == lotId && (c.State == CommitState.Actual ||
												c.State == CommitState.ActualWithModification))
					   ?.PersonPart.Entity;
				}

				var converter = metadataConverterFactory.Invoke(metadataPrefix);
				var metadata = converter.MapToRecordMetadata(part, author);

				record.RecordMetadata = metadata;
			}

			var setName = part.Entity.ResearchArea.Name;
			var setSpecs = setRepository.GetSets(null, null)
				.Items
				.Where(set => set.Name == setName);

			foreach (var spec in setSpecs)
			{
				header.SetSpecs.Add(spec.Spec);
			}

			return record;
		}

		private IQueryable<AcademicDegreePart> BuildQuery(Expression<Func<AcademicDegreePart, bool>> predicate)
		{
			var ids = rasContext
				.AcademicDegreeParts
				.Where(p => p.Entity.IsActive &&
							(p.State == PartState.Modified || p.State == PartState.Unchanged) &&
							p.Entity.Dissertation != null &&
							p.Entity.ResearchArea != null)
				.Where(predicate)
				.Select(p => new {
					p.Id,
					p.EntityId,
					DissertationId = p.Entity.Dissertation.Id
				})
				.ToList()
				.GroupBy(p => p.EntityId)
				.Select(p => p.First().Id)
				.ToList();

			return rasContext
				.AcademicDegreeParts
				.Include(t => t.Entity.Country)
				.Include(t => t.Entity.Dissertation.Language)
				.Include(t => t.Entity.Dissertation.DissertationFile)
				.Include(t => t.Entity.Institution)
				.Include(t => t.Entity.Settlement)
				.Include(p => p.Entity.ResearchArea)
				.Where(p => ids.Contains(p.Id));
		}
	}
}
