namespace ServerApplication.Infrastructure.Configuration
{
	public class FullTextConfiguration
	{
		public int ElasticMaxNumberAttempts { get; set; }
		public string ElastisearchServiceUrl { get; set; }

		public string OcrServiceUrl { get; set; }
		public string OcrResponseUrlTemplate { get; set; }
		public string FileFetchingUrlTemplate { get; set; }
	}
}
