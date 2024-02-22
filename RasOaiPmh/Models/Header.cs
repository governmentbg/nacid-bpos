using System;
using System.Collections.Generic;

namespace NacidRas.Integrations.OaiPmhProvider.Models
{
	public class Header
	{
		/// <summary>
		/// The unique identifier of an item in a repository.
		/// </summary>
		public string Identifier { set; get; }

		/// <summary>
		/// The date of creation, modification or deletion of the record for the purpose of selective harvesting.
		/// </summary>
		public string StringifiedDatestamp { set; get; }

		public DateTime? Datestamp { get; set; }

		/// <summary>
		/// Optional with a value of deleted indicating the withdrawal of availability of the specified metadata 
		/// format for the item, dependent on the repository support for deletions.
		/// </summary>
		public string Status { set; get; }

		/// <summary>
		/// The set memberships of the item for the purpose of selective harvesting.
		/// </summary>
		public IList<string> SetSpecs { get; } = new List<string>();
	}
}
