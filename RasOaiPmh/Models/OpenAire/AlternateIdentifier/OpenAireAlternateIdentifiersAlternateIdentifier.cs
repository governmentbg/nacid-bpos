using System.Xml.Linq;

namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public class OpenAireAlternateIdentifiersAlternateIdentifier : IXmlSerializableMetadataElement
	{
		public string AlternateIdentifierType { get; set; }
		public string Value { get; set; }

		public XElement Serialize()
		{
			return new XElement(OaiNamespaces.DataCiteNamespace + "alternateIdentifier", new XAttribute("alternateIdentifierType", AlternateIdentifierType), Value);
		}
	}
}
