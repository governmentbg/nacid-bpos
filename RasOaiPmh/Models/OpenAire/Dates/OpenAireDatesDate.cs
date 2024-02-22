using System.Xml.Linq;

namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public class OpenAireDatesDate : IXmlSerializableMetadataElement
	{
		public DateType DateType { get; set; }
		public string Value { get; set; }

		public XElement Serialize()
		{
			return new XElement(OaiNamespaces.DataCiteNamespace + "date", new XAttribute("dateType", DateType.ToString()), Value);
		}
	}
}
