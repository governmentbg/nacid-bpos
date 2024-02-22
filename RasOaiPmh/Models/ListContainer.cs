using System.Collections.Generic;

namespace NacidRas.Integrations.OaiPmhProvider.Models
{
	public class ListContainer<T>
	{
		public IList<T> Items { get; set; } = new List<T>();

		public ResumptionToken ResumptionToken { set; get; }
	}
}
