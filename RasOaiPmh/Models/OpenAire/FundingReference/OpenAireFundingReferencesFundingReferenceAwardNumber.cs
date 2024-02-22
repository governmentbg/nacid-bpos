using System.Collections.Generic;
using System.Xml.Linq;
using NacidRas.Integrations.OaiPmhProvider.Models.Metadata;

namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public class OpenAireFundingReferencesFundingReferenceAwardNumber : IXmlSerializableMetadataElement
	{
		public string AwardUri { get; set; }
		public string Value { get; set; }

		public XElement Serialize()
		{
			var attributes = new List<XAttribute>();
			if (!string.IsNullOrEmpty(AwardUri))
			{
				attributes.Add(new XAttribute("awardURI", AwardUri));
			}

			return new XElement(OaiNamespaces.OpenAireNamespace + "fundingReference", attributes, Value);
		}
	}
}
