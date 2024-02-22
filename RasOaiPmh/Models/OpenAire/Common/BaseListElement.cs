using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public abstract class BaseListElement<TMetadataElement> : IXmlSerializableMetadataElement
		where TMetadataElement : IXmlSerializableMetadataElement
	{
		public List<TMetadataElement> Items { get; set; }

		public XName ElementName { get; }

		protected BaseListElement(XName elementName)
		{
			Items = new List<TMetadataElement>();
			ElementName = elementName;
		}

		public virtual XElement Serialize()
		{
			var serializedTitles = Items
				.Select(c => c.Serialize())
				.ToList();

			return new XElement(ElementName, serializedTitles);
		}
	}
}
