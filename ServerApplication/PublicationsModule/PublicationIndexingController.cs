using System.Threading.Tasks;
using FullTextSearch;
using Microsoft.AspNetCore.Mvc;
using OcrMicroservice.Models;

namespace ServerApplication.PublicationsModule
{
	[ApiController]
	[Route("api/[controller]")]
	public class PublicationIndexingController
	{
		private readonly IElasticsearchServiceAdapter indexingService;

		public PublicationIndexingController(IElasticsearchServiceAdapter indexingService)
		{
			this.indexingService = indexingService;
		}

		[HttpPost("{id:int}")]
		public async Task ProcessOcredPublicationContent(int id, [FromBody] OcrMetadata metadata)
		{
			if (metadata.Status == OcrStatus.Processed)
			{
				await indexingService.InsertContent(id, metadata.OcrResult.Content);
			}
		}
	}
}
