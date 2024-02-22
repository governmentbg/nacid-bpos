using System.Collections.Generic;

namespace NacidRas.Integrations.OaiPmhProvider.Models
{
	public class Identify
	{
		/// <summary>
		/// A human readable name for the repository.
		/// </summary>
		public string RepositoryName { set; get; } = "OpenScience";

		/// <summary>
		/// The base URL of the repository.
		/// </summary>
		public string BaseUrl { set; get; } = "https://openras-test.nacid.bg/oai/request";

		/// <summary>
		/// The version of the OAI-PMH supported by the repository.
		/// </summary>
		public string ProtocolVersion { set; get; } = "2.0";

		/// <summary>
		/// A UTCdatetime that is the guaranteed lower limit of all datestamps recording changes, modifications, or deletions in the repository.
		/// </summary>
		public string EarliestDatestamp { set; get; } = "1987-02-16T00:00:00Z";

		/// <summary>
		/// The manner in which the repository supports the notion of deleted records.
		/// </summary>
		public string DeletedRecord { set; get; } = "persistent";

		/// <summary>
		/// The finest harvesting granularity supported by the repository. 
		/// </summary>
		public string Granularity { set; get; } = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'";

		/// <summary>
		/// The e-mail addresses of the administrators of the repository.
		/// </summary>
		public IList<string> AdminEmails { get; } = new List<string> { "ivan.mladenov@abbaty.com" };

		/// <summary>
		/// Compression encodings supported by the repository.
		/// </summary>
		public IList<string> Compressions { get; } = new List<string>();

		/// <summary>
		/// An extensible mechanism for communities to describe their repositories.
		/// </summary>
		public IList<string> Descriptions { get; } = new List<string>();
	}
}
