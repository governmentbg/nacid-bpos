using FullTextSearch;
using FullTextSearch.Models;
using OpenScience.Common.Services;
using OpenScience.Data.Publications.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenScience.Services.Publications
{
	public class PublicationIndexingService
	{
		private readonly IElasticsearchServiceAdapter indexingService;
		private readonly IOcrServiceAdapter ocrService;
		private readonly RetryExecutionService retryExecutionService;

		public PublicationIndexingService(
			IElasticsearchServiceAdapter indexingService,
			IOcrServiceAdapter ocrService,
			RetryExecutionService retryExecutionService
		)
		{
			this.indexingService = indexingService;
			this.ocrService = ocrService;
			this.retryExecutionService = retryExecutionService;
		}

		public async Task IndexPublication(Publication publication, int remoteRequestRetryCount, string fileFetchingUrlTemplate, string ocrResponseUrlTemplate)
		{
			await retryExecutionService.ExecuteAsync<HttpRequestException>(remoteRequestRetryCount, async _ => await indexingService.Index(publication));

			var ocrTasks = publication.Files
				.Select(e => 
					retryExecutionService.ExecuteAsync<OcrRequestException>(
					remoteRequestRetryCount,
					async _ => await ocrService.Request(
						string.Format(fileFetchingUrlTemplate, e.Key.ToString(), e.DbId),
						e.MimeType,
						string.Format(ocrResponseUrlTemplate, e.PublicationId))
					)
				);
			await Task.WhenAll(ocrTasks);
		}

		public async Task IndexPublicationsWithContent(ICollection<Publication> publications, int remoteRequestRetryCount, string ocrResponseUrlTemplate)
		{
			if (!publications.Any())
			{
				return;
			}

			await retryExecutionService.ExecuteAsync<HttpRequestException>(remoteRequestRetryCount, async _ => await indexingService.BulkIndex(publications));

			var publicationFileLocations = publications
				.SelectMany(p => p.FileLocations)
				.ToList();
			var ocrTasks = publicationFileLocations.Select(publicationFileLocation =>
				retryExecutionService.ExecuteAsync<OcrRequestException>(
					remoteRequestRetryCount,
					async _ => await ocrService.Request(
						publicationFileLocation.FileUrl,
						publicationFileLocation.MimeType,
						string.Format(ocrResponseUrlTemplate, publicationFileLocation.PublicationId))
					)
				);

			await Task.WhenAll(ocrTasks);
		}
	}
}
