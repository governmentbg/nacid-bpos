using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using NacidRas.Integrations.OaiPmhProvider.Models.DublinCore;

namespace NacidRas.Integrations.OaiPmhProvider.Converters
{
	public class DublinCoreMetadataConverter : BaseMetadataConverter<DublinCoreMetadata>
	{
		private readonly OaiConfiguration configuration;
		private readonly IDateConverter dateConverter;

		public DublinCoreMetadataConverter(OaiConfiguration configuration, IDateConverter dateConverter)
		{
			this.configuration = configuration;
			this.dateConverter = dateConverter;
		}

		public override string Prefix => DublinCoreMetadata.MetadataFormatPrefix;

		protected override XElement EncodeMetadata(DublinCoreMetadata metadata)
		{
			return new XElement(OaiNamespaces.OaiDcNamespace + "dc",
					new XAttribute(XNamespace.Xmlns + "oai_dc", OaiNamespaces.OaiDcNamespace),
					new XAttribute(XNamespace.Xmlns + "dc", OaiNamespaces.DcNamespace),
					new XAttribute(XNamespace.Xmlns + "xsi", OaiNamespaces.XsiNamespace),
					new XAttribute(OaiNamespaces.XsiNamespace + "schemaLocation", OaiNamespaces.OaiDcSchemaLocation),
					EncodeList(OaiNamespaces.DcNamespace + "title", metadata.Title),
					EncodeList(OaiNamespaces.DcNamespace + "creator", metadata.Creator),
					EncodeList(OaiNamespaces.DcNamespace + "subject", metadata.Subject),
					EncodeList(OaiNamespaces.DcNamespace + "description", metadata.Description),
					EncodeList(OaiNamespaces.DcNamespace + "publisher", metadata.Publisher),
					EncodeList(OaiNamespaces.DcNamespace + "contributor", metadata.Contributor),
					EncodeList(OaiNamespaces.DcNamespace + "date", metadata.Date, (date) => dateConverter.Encode(configuration.Identify.Granularity, date)),
					EncodeList(OaiNamespaces.DcNamespace + "type", metadata.Type),
					EncodeList(OaiNamespaces.DcNamespace + "format", metadata.Format),
					EncodeList(OaiNamespaces.DcNamespace + "identifier", metadata.Identifier),
					EncodeList(OaiNamespaces.DcNamespace + "source", metadata.Source),
					EncodeList(OaiNamespaces.DcNamespace + "language", metadata.Language),
					EncodeList(OaiNamespaces.DcNamespace + "relation", metadata.Relation),
					EncodeList(OaiNamespaces.DcNamespace + "coverage", metadata.Coverage),
					EncodeList(OaiNamespaces.DcNamespace + "rights", metadata.Rights));
		}

		protected override XElement CreateElement<TItem>(XName name, TItem item, Func<TItem, string> toStringFunc = null)
		{
			if (item is DublinCoreElement element && !string.IsNullOrEmpty(element.Language))
			{
				return new XElement(name, new XAttribute(XNamespace.Xml + "lang", element.Language), element.Value);
			}

			return base.CreateElement(name, item, toStringFunc);
		}

		private void AddElement(IList<DublinCoreElement> elements, XmlReader reader)
		{
			var lang = reader.GetAttribute("lang", XNamespace.Xml.ToString());
			DublinCoreElementListExtensions.Add(elements, reader.ReadString(), lang);
		}
	}
}
