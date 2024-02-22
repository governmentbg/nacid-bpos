using System.Xml.Linq;

namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public class LicenseCondition : IXmlSerializableMetadataElement
	{
		public string Uri { get; set; }
		public string StartDate { get; set; }
		public string Value { get; set; }

		public XElement Serialize()
		{
			return new XElement(OaiNamespaces.OpenAireNamespace + "licenseCondition",
				new XAttribute("startData", StartDate),
				new XAttribute("uri", Uri),
				Value);
		}
	}
}
