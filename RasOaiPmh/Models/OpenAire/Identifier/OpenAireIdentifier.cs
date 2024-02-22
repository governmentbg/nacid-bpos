using System.Xml.Linq;

namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public class OpenAireIdentifier : IXmlSerializableMetadataElement
	{
		public IdentifierType IdentifierType { get; set; }
		public string Value { get; set; }

		public XElement Serialize()
		{
			return new XElement(OaiNamespaces.DataCiteNamespace + "identifier", new XAttribute("identifierType", IdentifierType.ToString()), Value);
		}
	}
}
