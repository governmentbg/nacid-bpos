using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using MetadataHarvesting.Models;
using MetadataHarvesting.Models.DublinCore;

namespace MetadataHarvesting.Core.Converters
{
	public class DublinCoreMetadataConverter : BaseMetadataConverter<DublinCoreMetadata>, IDublinCoreMetadataConverter
	{
		private readonly OaiConfiguration configuration;
		private readonly IDateConverter dateConverter;

		public DublinCoreMetadataConverter(OaiConfiguration configuration, IDateConverter dateConverter)
		{
			this.configuration = configuration;
			this.dateConverter = dateConverter;
		}

		public override string Prefix => DublinCoreMetadata.MetadataFormatPrefix;

		public override RecordMetadata ParseMetadata(string xml)
		{
			var result = new DublinCoreMetadata();

			var reader = new XmlTextReader(xml, XmlNodeType.Element, null);
			while (reader.Read() && reader.Name != "oai_dc:dc" && reader.Name != "oaidc:dc") ;

			var dublinCoreXml = reader.ReadInnerXml();

			reader = new XmlTextReader(dublinCoreXml, XmlNodeType.Element, null);

			while (reader.Read())
			{
				var metadata = reader.Name.Replace("dc:", "");

				if (metadata == "date")
				{
					result.Date.Add(dateConverter.Decode(reader.ReadString()));
				}
				else
				{
					ICollection<DublinCoreElement> collection = null;

					switch (metadata)
					{
						case "title":
							collection = result.Title;
							break;
						case "creator":
							collection = result.Creator;
							break;
						case "subject":
							collection = result.Subject;
							break;
						case "description":
							collection = result.Description;
							break;
						case "publisher":
							collection = result.Publisher;
							break;
						case "contributor":
							collection = result.Contributor;
							break;
						case "type":
							collection = result.Type;
							break;
						case "format":
							collection = result.Format;
							break;
						case "identifier":
							collection = result.Identifier;
							break;
						case "source":
							collection = result.Source;
							break;
						case "language":
							collection = result.Language;
							break;
						case "relation":
							collection = result.Relation;
							break;
						case "coverage":
							collection = result.Coverage;
							break;
						case "rights":
							collection = result.Rights;
							break;
					}

					collection?.Add(ReadElement(reader));
				}
			}

			return result;
		}

		public DublinCoreElement ReadElement(XmlReader reader)
		{
			var language = reader.GetAttribute("lang", XNamespace.Xml.ToString());
			var value = reader.ReadString();

			return new DublinCoreElement { Language = language, Value = value };
		}

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
	}
}
