using System;
using System.Collections.Generic;

namespace NacidRas.Integrations.OaiPmhProvider.Models
{
	public class ResumptionToken
	{
		private string resumptionToken;

		// A UTC datetime value, which specifies a lower bound for datestamp-based selective harvesting.
		public DateTime? From { get; set; }

		// A UTC datetime value, which specifies a upper bound for datestamp-based selective harvesting.
		public DateTime? Until { get; set; }

		// A required argument, which specifies that headers should be returned 
		// only if the metadata format matching the supplied metadataPrefix is available or, 
		// depending on the repository's support for deletions, has been deleted.
		public string MetadataPrefix { get; set; }

		// An optional argument which specifies set criteria for selective harvesting.
		public string Set { get; set; }
	
		// A UTC datetime indicating when the resumptionToken ceases to be valid.
		public DateTime? ExpirationDate { get; set; }

		// An integer indicating the cardinality of the complete list 
		// (i.e., the sum of the cardinalities of the incomplete lists).
		public int? CompleteListSize { get; set; }

		// A count of the number of elements of the complete list thus far returned 
		// (i.e. cursor starts at 0).
		public int? Cursor { get; set; }

		// Optional custom parameters not part of the specification.
		public IDictionary<string, string> Custom { get; set; } = new Dictionary<string, string>();
		public string StringifiedExpirationDate { set; get; }

		public bool IsEscaped { set; get; } = false;

		public string Token
		{
			set => resumptionToken = IsEscaped == false ? value : EscapeSpecial(value);
			get => resumptionToken;
		}

		private string EscapeSpecial(string s)
		{
			s = s.Replace("%", "%25");
			s = s.Replace("/", "%2F");
			s = s.Replace("?", "%3F");
			s = s.Replace("#", "%23");
			s = s.Replace("=", "%3D");
			s = s.Replace("&", "%26");
			s = s.Replace(":", "%3A");
			s = s.Replace(";", "%3B");
			s = s.Replace(" ", "%20");
			s = s.Replace("+", "%2B");
			return s;
		}
	}
}
