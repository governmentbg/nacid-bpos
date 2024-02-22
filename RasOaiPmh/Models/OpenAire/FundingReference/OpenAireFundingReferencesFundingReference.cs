using System.Collections.Generic;
using System.Xml.Linq;
using NacidRas.Integrations.OaiPmhProvider.Models.Metadata;

namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public class OpenAireFundingReferencesFundingReference : IXmlSerializableMetadataElement
	{
		public string FunderName { get; set; }
		public OpenAireFundingReferencesFundingReferenceFunderIdentifier FunderIdentifier { get; set; }
		public string FundingStream { get; set; }
		public OpenAireFundingReferencesFundingReferenceAwardNumber AwardNumber { get; set; }
		public string AwardTitle { get; set; }
		public XElement Serialize()
		{
			var elements = new List<XElement>();

			if (!string.IsNullOrEmpty(FunderName))
			{
				elements.Add(new XElement("funderName", FunderName));
			}

			if (!string.IsNullOrEmpty(FundingStream))
			{
				elements.Add(new XElement("fundingStream", FundingStream));
			}

			if (!string.IsNullOrEmpty(AwardTitle))
			{
				elements.Add(new XElement("awardTitle", AwardTitle));
			}

			if (FunderIdentifier != null)
			{
				elements.Add(FunderIdentifier.Serialize());
			}

			if (AwardNumber != null)
			{
				elements.Add(AwardNumber.Serialize());
			}

			return new XElement(OaiNamespaces.OpenAireNamespace + "fundingReference", elements);
		}
	}
}
