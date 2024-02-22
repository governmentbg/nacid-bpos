using FullTextSearch;
using Microsoft.AspNetCore.Mvc;
using OcrMicroservice.Models;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using OpenScience.Services.Publications;

namespace ServerApplication.Diagnostics
{

#if DEBUG

	[ApiController]
	[Route("diagnostics")]
	public class DiagnosticsController : ControllerBase
	{
		private readonly IOcrServiceAdapter ocr;
		private readonly IElasticsearchServiceAdapter elasticsearch;
		private readonly PublicationService service;

		public DiagnosticsController(IOcrServiceAdapter ocr, IElasticsearchServiceAdapter elasticsearch, PublicationService service)
		{
			this.ocr = ocr;
			this.elasticsearch = elasticsearch;
			this.service = service;
		}

		[HttpGet("ocr/request")]
		public Task<OcrMetadata> OcrRequest([FromQuery] string localFilePath)
		{
			return ocr.RequestLocalFile(localFilePath);
		}

		[HttpGet("ocr/get")]
		public Task<OcrMetadata> OcrGet([FromQuery] Guid key, [FromQuery] bool includeResult = false)
		{
			return ocr.Get(key, includeResult);
		}

		[HttpGet("elastic/index")]
		[AllowAnonymous]
		public async Task OcrGet()
		{
			var publications = (await service.GetAllAsync());

			foreach (var publication in publications)
			{
				await elasticsearch.Update(publication);
			}
		}

#endif

	}
}
