using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NacidRas.Integrations.OaiPmhProvider.Models.Metadata;

namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public class OpenAireSizes : IXmlSerializableMetadataElement
	{
		public List<string> Sizes { get; set; }

		public OpenAireSizes()
		{
			Sizes = new List<string>();
		}

		public XElement Serialize()
		{
			var serializedSizes = Sizes
				.Select(size => new XElement(OaiNamespaces.DataCiteNamespace + "size", size))
				.ToList();

			return new XElement(OaiNamespaces.DataCiteNamespace + "sizes", serializedSizes);
		}
	}
}
