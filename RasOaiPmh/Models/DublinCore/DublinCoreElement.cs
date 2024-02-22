using System.Collections.Generic;
using System.Xml.Linq;

namespace NacidRas.Integrations.OaiPmhProvider.Models.DublinCore
{
	public class DublinCoreElement
	{
		public string Value { get; set; }

		public string Language { get; set; }

		public override string ToString() => Value;

		public XElement Serialize(XName elementName)
		{
			var attributes = new List<XAttribute>();

			if (!string.IsNullOrEmpty(Language))
			{
				attributes.Add(new XAttribute(XNamespace.Xml + "lang", Language));
			}

			return new XElement(elementName, attributes, Value);
		}
	}

	public static class DublinCoreElementListExtensions
	{
		public static void Add(this ICollection<DublinCoreElement> collection, string value, string language = null)
		{
			var newElement = new DublinCoreElement { Value = value, Language = language };

			collection.Add(newElement);
		}
	}
}
