using System.Collections.Generic;
using System.Xml.Linq;
using NacidRas.Integrations.OaiPmhProvider.Models.Metadata;

namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public class OpenAireTitlesTitle : IXmlSerializableMetadataElement
	{
		public TitleType? TitleType { get; set; }
		public string Language { get; set; }
		public string Value { get; set; }

		public XElement Serialize()
		{
			var attributes = new List<XAttribute>();

			if (TitleType.HasValue)
			{
				attributes.Add(new XAttribute("titleType", TitleType.ToString()));
			}

			if (!string.IsNullOrEmpty(Language))
			{
				attributes.Add(new XAttribute(XNamespace.Xml + "lang", Language));
			}

			return new XElement(OaiNamespaces.DataCiteNamespace + "title", attributes, Value);
		}
	}
}
