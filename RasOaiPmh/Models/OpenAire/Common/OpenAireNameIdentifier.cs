using System.Collections.Generic;
using System.Xml.Linq;
using NacidRas.Integrations.OaiPmhProvider.Models.Metadata;

namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public class OpenAireNameIdentifier : IXmlSerializableMetadataElement
	{
		public string NameIdentifierScheme { get; set; }
		public string SchemeUri { get; set; }
		public string Value { get; set; }

		public XElement Serialize()
		{
			var attributes = new List<XAttribute>();

			if (!string.IsNullOrEmpty(NameIdentifierScheme))
			{
				attributes.Add(new XAttribute("nameIdentifierScheme", NameIdentifierScheme));
			}

			if (!string.IsNullOrEmpty(SchemeUri))
			{
				attributes.Add(new XAttribute("schemeURI", SchemeUri));
			}

			return new XElement(OaiNamespaces.DataCiteNamespace + "nameIdentifier", attributes, Value);
		}
	}
}
