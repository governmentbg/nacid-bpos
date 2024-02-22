using System.Collections.Generic;
using System.Xml.Linq;

namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public class OpenAireRelatedIdentifiersRelatedIdentifier : IXmlSerializableMetadataElement
	{
		public string Value { get; set; }
		public RelatedIdentifierType RelatedIdentifierType { get; set; }
		public RelationType RelationType { get; set; }
		public string RelatedMetadataScheme { get; set; }
		public string SchemeURI { get; set; }
		public string SchemeType { get; set; }
		public ResourceGeneralType? ResourceTypeGeneral { get; set; }

		public XElement Serialize()
		{
			var attributes = new List<XAttribute>
			{
				new XAttribute("relatedIdentifierType",RelatedIdentifierType.ToString()),
				new XAttribute("relationType",RelationType.ToString()),
			};

			if (!string.IsNullOrEmpty(SchemeURI))
			{
				attributes.Add(new XAttribute("schemeURI", SchemeURI));
			}

			if (!string.IsNullOrEmpty(SchemeType))
			{
				attributes.Add(new XAttribute("schemeType", SchemeType));
			}

			if (!string.IsNullOrEmpty(RelatedMetadataScheme))
			{
				attributes.Add(new XAttribute("relatedMetadataScheme", RelatedMetadataScheme));
			}

			if (ResourceTypeGeneral.HasValue)
			{
				attributes.Add(new XAttribute("resourceTypeGeneral", ResourceTypeGeneral.ToString()));
			}

			return new XElement(OaiNamespaces.DataCiteNamespace + "relatedIdentifier", attributes, Value);
		}
	}
}
