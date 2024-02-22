using System.Collections.Generic;

namespace NacidRas.Integrations.OaiPmhProvider.Models
{
	public class ListMetadataFormats
	{
		public IList<MetadataFormat> MetadataFormats { get; set; } = new List<MetadataFormat>();
	}
}
