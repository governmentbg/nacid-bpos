using System.Collections.Generic;

namespace OpenScience.Common.DomainValidation.Models
{
	public class ResponseMessage
	{
		public const string Error = "Error";
		public const string Warning = "Warning";

		public string Status { get; set; }

		public List<DomainErrorMessage> Messages { get; set; }
	}
}
