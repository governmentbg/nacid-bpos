using System.Collections.Generic;
using System.Xml.Linq;
using NacidRas.Integrations.OaiPmhProvider.Models.Metadata;

namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public class OpenAireSubjectsSubject : IXmlSerializableMetadataElement
	{
		public string SubjectScheme { get; set; }
		public string SchemeUri { get; set; }
		public string ValueUri { get; set; }
		public string Language { get; set; }
		public string Value { get; set; }

		public XElement Serialize()
		{
			var attributes = new List<XAttribute>();

			if (!string.IsNullOrEmpty(SubjectScheme))
			{
				attributes.Add(new XAttribute("subjectScheme", SubjectScheme));
			}

			if (!string.IsNullOrEmpty(SchemeUri))
			{
				attributes.Add(new XAttribute("schemeURI", SchemeUri));
			}

			if (!string.IsNullOrEmpty(ValueUri))
			{
				attributes.Add(new XAttribute("valueURI", ValueUri));
			}

			if (!string.IsNullOrEmpty(Language))
			{
				attributes.Add(new XAttribute(XNamespace.Xml + "lang", Language));
			}

			return new XElement(OaiNamespaces.DataCiteNamespace + "subject", attributes, Value);
		}
	}
}
