using FullTextSearch;
using FullTextSearch.Models.Elasticsearch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenScience.Data;
using OpenScience.Data.Base.Models;
using Portal.Hosting.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Hosting
{
	[ApiController]
	[Route("api/[controller]")]
	public class ClassificationController
	{
		private readonly AppDbContext context;
		private readonly IElasticsearchServiceAdapter elasticsearch;
		private readonly ElasticsearchUtilityService elasticsearchUtility;

		public ClassificationController(
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
			var level1 = context.Classifications
				.AsNoTracking()
				.Where(e => !e.OrganizationId.HasValue && !e.ParentId.HasValue)
				.SelectMany(e => e.Children);

			var level2 = level1.SelectMany(e => e.Children);

			var query = context.Classifications
				.Where(e => level1.Select(l1 => l1.Id).Contains(e.Id))
					//|| level2.Select(l2 => l2.Id).Contains(e.Id))
				.Select(e => new Nomenclature {
					Id = e.Id,
					Name = e.Name
				});

			if (filter == null)
			{
				filter = new BaseNomenclatureFilterDto<Nomenclature>();
			}

			//var query = context.Classifications
			//	.AsNoTracking()
			//	.OrderBy(e => e.Id)
			//	.Select(e => new Nomenclature {
			//		Id = e.Id,
			//		Name = e.Name
			//	})
			//	.Where(filter.GetPredicate());

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
			var classification = await context.Classifications.SingleOrDefaultAsync(e => e.Id == id);

			if (classification == null)
				return null;

			return new Nomenclature {
				Id = classification.Id,
				Name = classification.Name
			};
		}

		[HttpPost("hierarchy")]
		public async Task<IEnumerable<ClassificationDto>> GetHierarchy([FromBody] object elasticsearchQuery)
		{
			var data = await elasticsearch.Search<EsSearchResult>(elasticsearchQuery);

			var scientificAreaRoots = context.Classifications
				.AsNoTracking()
				.Include(e => e.Children)
					.ThenInclude(e => e.Children)
				.Where(e => !e.OrganizationId.HasValue && !e.ParentId.HasValue)
				.ToList();

			var result = scientificAreaRoots
				.SelectMany(e => e.Children)
				.Select(e => new ClassificationDto {
					Id = e.Id,
					Name = e.Name,
					Count = elasticsearchUtility.GetBucketSize(EsCategory.Classification, e.Id.ToString(), data.Aggregations),
					Children = e.Children
					.Select(c => new ClassificationDto {
						Id = c.Id,
						Name = c.Name,
						Count = elasticsearchUtility.GetBucketSize(EsCategory.Classification, c.Id.ToString(), data.Aggregations)
					}).OrderBy(c => c.Id).ToList()
				})
				.OrderBy(e => e.Id)
				.ToList();


			return result;
		}
	}
}
