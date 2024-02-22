using System.Xml.Linq;

namespace NacidRas.Integrations.OaiPmhProvider.Models
{
	public class RecordMetadata
	{
		public string MetadataFormat { get; set; }

		public XElement Content { get; set; }
	}
}
