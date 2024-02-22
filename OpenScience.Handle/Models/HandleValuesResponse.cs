using System.Collections.Generic;

namespace OpenScience.Handle.Models
{
	public class HandleValuesResponse : HandleOperationResponse
	{
		public List<HandleIdentifier> Values { get; set; }
	}
}
