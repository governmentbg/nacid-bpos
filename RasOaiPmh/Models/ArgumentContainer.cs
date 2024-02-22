namespace NacidRas.Integrations.OaiPmhProvider.Models
{
	public class ArgumentContainer
	{
		public ArgumentContainer()
		{
			this.Verb = string.Empty;
			this.MetadataPrefix = string.Empty;
			this.ResumptionToken = string.Empty;
			this.Identifier = string.Empty;
			this.From = string.Empty;
			this.Until = string.Empty;
			this.Set = string.Empty;
		}

		public string Verb { get; set; }
		public string MetadataPrefix { get; set; }
		public string ResumptionToken { get; set; }
		public string Identifier { get; set; }
		public string From { get; set; }
		public string Until { get; set; }
		public string Set { get; set; }

		public ArgumentContainer(string verb = null,
			string metadataPrefix = null,
			string resumptionToken = null,
			string identifier = null,
			string from = null,
			string until = null,
			string set = null)
		{
			Verb = verb;
			MetadataPrefix = metadataPrefix;
			ResumptionToken = resumptionToken;
			Identifier = identifier;
			From = from;
			Until = until;
			Set = set;
		}
	}
}
