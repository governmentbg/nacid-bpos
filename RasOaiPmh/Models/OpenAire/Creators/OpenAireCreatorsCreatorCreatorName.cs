using System.Collections.Generic;
using System.Xml.Linq;

namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public class OpenAireCreatorsCreatorCreatorName : IXmlSerializableMetadataElement
	{
		public NameType? NameType { get; set; }
		public string Value { get; set; }

		public XElement Serialize()
		{
			var attributes = new List<XAttribute>();

			if (NameType.HasValue)
			{
				attributes.Add(new XAttribute("nameType", NameType.ToString()));
			}

			return new XElement(OaiNamespaces.DataCiteNamespace + "creatorName", attributes, Value);
		}
	}
}
