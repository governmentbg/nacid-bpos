using FullTextSearch;
using FullTextSearch.Extensions;
using FullTextSearch.Models;
using FullTextSearch.Models.Elasticsearch;
using Microsoft.AspNetCore.Mvc;
using OpenScience.Data.Publications.Enums;
using OpenScience.Data.Publications.Models;
using OpenScience.Services.Publications;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Hosting
{
	[ApiController]
	[Route("api")]
	public class PortalController : ControllerBase
	{
		private readonly IElasticsearchServiceAdapter elasticsearch;
		private readonly ElasticsearchUtilityService utilityService;
		private readonly PublicationService publicationService;

		public PortalController(
			IElasticsearchServiceAdapter elasticsearch,
			ElasticsearchUtilityService utilityService,
			PublicationService publicationService
		)
		{
			this.elasticsearch = elasticsearch;
			this.utilityService = utilityService;
			this.publicationService = publicationService;
		}

		[HttpGet("publication/{id:int}")]
		public async Task<object> GetPublication(int id)
		{
			var publication = await publicationService.GetByIdAsync(id);
			if (publication == null || publication.Status != PublicationStatus.Published)
				return null;

			return new {
				Id=publication.Id,
				Titles=publication.Titles.Select(e => e.Value),
				Subjects=publication.Subjects.Select(e => e.Value),
				Creators=publication.Creators.Select(e => e.DisplayName),
				PublicationYear=publication.PublishYear,
				Classifications = publication.Classifications.Select(e => e.Classification.Name),
				Descriptions = publication.Descriptions.Select(e => e.Value),
				Files = publication.Files.Select(file => new {
					DbId=file.DbId,
					Key = file.Key,
					Name = file.Name
				}),
				FileLocations = publication.FileLocations.Select(fileLocation => new {
					Url=fileLocation.FileUrl
				})
			};
		}

		[HttpGet("publication/{id:int}/details")]
		public async Task<Publication> GetPublicationDetailedInformation(int id)
		{
			var publication = await publicationService.GetByIdAsync(id);
			if (publication == null || publication.Status != PublicationStatus.Published)
				return null;

			return publication;
		}

		public class EsSearchRequest
		{
			public object Search { get; set; }
			public object Aggregate { get; set; }
		}

		[HttpPost("search")]
		public async Task<SearchResult> Search([FromBody] EsSearchRequest request)
		{
			SearchResult result = new SearchResult();

			if(request.Search != null)
			{
				var searchData = await elasticsearch.Search<EsSearchResult>(request.Search);
				result.Items = utilityService.MapSearchResults(searchData.Hits.Hits);
				result.TotalCount = searchData.Hits.Total.Value;
				result.Took = searchData.Took;
				result.TimedOut = searchData.Timed_out;
			}

			if(request.Aggregate != null)
			{
				var aggregationsData = await elasticsearch.Search<EsAggregationResult>(request.Aggregate);
				result.Categories = utilityService.GetCategories(aggregationsData.Aggregations);
			}

			return result;
		}

		[HttpPost("aggregate")]
		public async Task<SearchResult> Aggregate([FromBody] object query)
		{
			var data = await elasticsearch.Search<EsAggregationResult>(query);

			SearchResult result = new SearchResult {
				Categories = utilityService.GetCategories(data.Aggregations)
			};

			return result;
		}
	}
}
