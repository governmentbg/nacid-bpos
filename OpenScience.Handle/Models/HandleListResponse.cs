using System.Collections.Generic;

namespace OpenScience.Handle.Models
{
	public class HandleListResponse : HandleBaseResponse
	{
		public string Prefix { get; set; }
		public int TotalCount { get; set; }
		public List<string> Handles { get; set; }
	}
}
