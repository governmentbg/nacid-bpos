using FullTextSearch;
using FullTextSearch.Models.Elasticsearch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenScience.Data;
using OpenScience.Data.Base.Models;
using OpenScience.Data.Classifications.Models;
using Portal.Hosting.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Hosting
{

	[ApiController]
	[Route("api/[controller]")]
	public class InstitutionController
	{
		private readonly AppDbContext context;
		private readonly IElasticsearchServiceAdapter elasticsearch;
		private readonly ElasticsearchUtilityService elasticsearchUtility;

		public InstitutionController(
			AppDbContext context,
			IElasticsearchServiceAdapter elasticsearch,
			ElasticsearchUtilityService elasticsearchUtility
		)
		{
			this.context = context;
			this.elasticsearch = elasticsearch;
			this.elasticsearchUtility = elasticsearchUtility;
		}

		[HttpGet("")]
		public async Task<IEnumerable<Nomenclature>> GetFiltered([FromQuery]BaseNomenclatureFilterDto<Nomenclature> filter)
		{
			if (filter == null)
			{
				filter = new BaseNomenclatureFilterDto<Nomenclature>();
			}

			var query = context.Institutions
				.AsNoTracking()
				.OrderBy(e => e.Id)
				.Select(e => new Nomenclature {
					Id = e.Id,
					Name = e.Name
				})
				.Where(filter.GetPredicate());

			if (filter.Offset.HasValue && filter.Limit.HasValue)
			{
				query = query.Skip(filter.Offset.Value)
					.Take(filter.Limit.Value);
			}

			var result = await query.ToListAsync();

			return result;
		}

		[HttpGet("{id:int}")]
		public async Task<Nomenclature> GetById(int id)
		{
			var institution = await context.Institutions
				.SingleOrDefaultAsync(e => e.Id == id);

			if (institution == null)
				return null;

			return new Nomenclature {
				Id = institution.Id,
				Name = institution.Name
			};
		}

		[HttpPost("hierarchy/{institutionId:int}")]
		public async Task<ClassificationDto> GetHierarchy(int institutionId, [FromBody] object elasticsearchQuery)
		{
			var data = await elasticsearch.Search<EsSearchResult>(elasticsearchQuery);

			var classificationsFixup = context.Classifications
				.Where(e => e.OrganizationId == institutionId)
				.ToList();

			var institution = context
				.Institutions
				.AsNoTracking()
				.SingleOrDefault(e => e.Id == institutionId);

			var rootClassifications = context.Classifications
				.Where(e => e.OrganizationId == institutionId && !e.ParentId.HasValue)
				.ToList();

			return new ClassificationDto
			{
				Id = institution.Id,
				Name = institution.Name,
				Children = rootClassifications.Select(FormatClassification(data.Aggregations)).OrderBy(e => e.Name).ToList()
			};
		}

		private Func<Classification, ClassificationDto> FormatClassification(Dictionary<string, EsCategory> aggregations)
		{
			return (Classification classification) => new ClassificationDto
			{
				Id = classification.Id,
				Name = classification.Name,
				Count = elasticsearchUtility.GetBucketSize(EsCategory.Classification, classification.Id.ToString(), aggregations),
				Children = classification.Children.Select(FormatClassification(aggregations)).OrderBy(e => e.Name).ToList()
			};
		}
	}
}
