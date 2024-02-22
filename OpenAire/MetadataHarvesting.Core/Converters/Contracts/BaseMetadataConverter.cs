using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MetadataHarvesting.Models;

namespace MetadataHarvesting.Core.Converters
{
	public abstract class BaseMetadataConverter<TMetadata> : IMetadataEncoder, IMetadataParser
		where TMetadata : RecordMetadata
	{
		public abstract string Prefix { get; }

		public XElement Encode(RecordMetadata metadata)
		{
			if (metadata is TMetadata recordMetadata)
			{
				return EncodeMetadata(recordMetadata);
			}

			return null;
		}

		public abstract RecordMetadata ParseMetadata(string xml);

		protected abstract XElement EncodeMetadata(TMetadata metadata);

		protected virtual IList<XElement> EncodeList<TItem>(XName name, IList<TItem> list, Func<TItem, string> toStringFunc = null)
		{
			var encodedList = new List<XElement>();

			if (list == null)
				return encodedList;

			encodedList = list
				.Where(item => item != null)
				.Select(item => CreateElement(name, item, toStringFunc))
				.ToList();

			return encodedList;
		}

		protected virtual XElement CreateElement<TItem>(XName name, TItem item, Func<TItem, string> toStringFunc = null)
		{
			if (item == null)
			{
				return null;
			}

			if (item is IXmlSerializableMetadataElement serializableMetadataElement)
			{
				return serializableMetadataElement.Serialize();
			}

			var content = toStringFunc?.Invoke(item) ?? item.ToString();

			return new XElement(name, content);
		}
	}
}
