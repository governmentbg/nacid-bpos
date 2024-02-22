using System.Collections.Generic;
using System.Xml.Linq;
using NacidRas.Integrations.OaiPmhProvider.Models.Metadata;

namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public class OpenAireFundingReferencesFundingReferenceFunderIdentifier : IXmlSerializableMetadataElement
	{
		public FunderIdentifierType? FunderIdentifierType { get; set; }
		public string Value { get; set; }

		private string StringifiedFunderIdentifierType => FunderIdentifierType == OpenAire.FunderIdentifierType.CrossrefFunderID ? "Crossref Funder ID" : FunderIdentifierType.ToString();

		public XElement Serialize()
		{
			var attributes = new List<XAttribute>();

			if (FunderIdentifierType.HasValue)
			{
				attributes.Add(new XAttribute("funderIdentifierType", StringifiedFunderIdentifierType));
			}

			return new XElement(OaiNamespaces.DataCiteNamespace + "creatorName", attributes, Value);
		}
	}
}
