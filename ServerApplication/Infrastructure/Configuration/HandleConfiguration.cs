namespace ServerApplication.Infrastructure.Configuration
{
	public class HandleConfiguration
	{
		public string HttpServerAddress { get; set; }

		public string Prefix { get; set; }

		public int AdminHandleIndex { get; set; }
		public string AdminHandle { get; set; }
		public string AdminCertPath { get; set; }
		public string AdminCertPass { get; set; }

		public int MaxNumberAttempts { get; set; }
		public string PreviewLinkTemplate { get; set; }
	}
}
