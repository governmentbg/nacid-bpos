using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using MetadataHarvesting.Models;
using MetadataHarvesting.Models.DublinCore;
using MetadataHarvesting.Models.Metadata;
using MetadataHarvesting.Models.Metadata.OpenAire;
using MetadataHarvesting.Models.OpenAire;

namespace MetadataHarvesting.Core.Converters
{
	public class OpenAireMetadataConverter : BaseMetadataConverter<OpenAireMetadata>
	{
		private readonly IDublinCoreMetadataConverter dublinCoreMetadataConverter;

		public OpenAireMetadataConverter(IDublinCoreMetadataConverter dublinCoreMetadataConverter)
		{
			this.dublinCoreMetadataConverter = dublinCoreMetadataConverter;
		}

		public override string Prefix => OpenAireMetadata.MetadataFormatPrefix;

		public override RecordMetadata ParseMetadata(string xml)
		{
			var reader = new XmlTextReader(xml, XmlNodeType.Element, null);
			while (reader.Read() && reader.Name != "resource") ;

			var openAireXml = reader.ReadInnerXml();

			reader = new XmlTextReader(openAireXml, XmlNodeType.Element, null);

			var result = new OpenAireMetadata();

			while (reader.Read())
			{
				var elementName = reader.Name;

				if (elementName.Contains("datacite:"))
				{
					elementName = elementName.Replace("datacite:", "");
					ReadDataciteElement(elementName, result, reader);
				}
				else if (elementName.Contains("dcterms:"))
				{
					elementName = elementName.Replace("dcterms:", "");
					ReadDcTermsElement(elementName, result, reader);
				}
				else if (elementName.Contains("dc:"))
				{
					elementName = elementName.Replace("dc:", "");
					ReadDublinCoreElement(elementName, result, reader);
				}
				else if (!string.IsNullOrEmpty(elementName))
				{
					elementName = elementName.Replace("oaire:", "");
					ReadOpenAireElement(elementName, result, reader);
				}
			}

			return result;
		}

		protected override XElement EncodeMetadata(OpenAireMetadata metadata)
		{
			var attributes = GetMetadataAttributes();
			var elements = ConvertMetadataElements(metadata);

			return new XElement("resource", attributes, elements);
		}

		private IEnumerable<XAttribute> GetMetadataAttributes()
		{
			return new List<XAttribute>
			{
				new XAttribute(XNamespace.Xmlns + "rdf", OaiNamespaces.RdfNamespace),
				new XAttribute(XNamespace.Xmlns + "xsi", OaiNamespaces.XsiNamespace),
				new XAttribute(XNamespace.Xmlns + "dc", OaiNamespaces.DcNamespace),
				new XAttribute(XNamespace.Xmlns + "datacite", OaiNamespaces.DataCiteNamespace),
				new XAttribute(XNamespace.Xmlns + "dcterms", OaiNamespaces.DcTermsNamespace),
				new XAttribute(XNamespace.Xmlns + "oaire", OaiNamespaces.OpenAireNamespace),
				//new XAttribute(OaiNamespaces.Xmlns, OaiNamespaces.OpenAireXmlns),
				new XAttribute(OaiNamespaces.XsiNamespace + "schemaLocation", OaiNamespaces.OaiOpenAireSchemaLocation),
			};
		}

		private IEnumerable<XElement> ConvertMetadataElements(OpenAireMetadata metadata)
		{
			var elements = new List<XElement>();

			AddListElement(metadata.Creators, elements);
			AddListElement(metadata.Titles, elements);
			AddListElement(metadata.Contributors, elements);
			AddListElement(metadata.FundingReferences, elements);
			AddListElement(metadata.Dates, elements);
			AddListElement(metadata.AlternateIdentifiers, elements);
			AddListElement(metadata.RelatedIdentifiers, elements);
			AddListElement(metadata.Subjects, elements);

			elements.Add(CreateElement(OaiNamespaces.OpenAireNamespace + "citationTitle", metadata.CitationTitle));
			elements.Add(CreateElement(OaiNamespaces.OpenAireNamespace + "citationVolume", metadata.CitationVolume));
			elements.Add(CreateElement(OaiNamespaces.OpenAireNamespace + "citationIssue", metadata.CitationIssue));
			elements.Add(CreateElement(OaiNamespaces.OpenAireNamespace + "citationStartPage", metadata.CitationStartPage));
			elements.Add(CreateElement(OaiNamespaces.OpenAireNamespace + "citationEndPage", metadata.CitationEndPage));
			elements.Add(CreateElement(OaiNamespaces.OpenAireNamespace + "citationEdition", metadata.CitationEdition));
			elements.Add(CreateElement(OaiNamespaces.OpenAireNamespace + "citationConferencePlace", metadata.CitationConferencePlace));
			elements.Add(CreateElement(OaiNamespaces.OpenAireNamespace + "citationConferenceDate", metadata.CitationConferenceDate));

			AddElement(metadata.AccessRigths, elements);
			AddElement(metadata.Sizes, elements);
			AddElement(metadata.ResourceType, elements);
			AddElement(metadata.ResourceIdentifier, elements);
			AddElement(metadata.LicenseCondition, elements);
			AddElement(metadata.ResourceVersion, elements);
			AddElement(metadata.PublicationDate, elements);

			AddSerializedList(metadata.Languages, dc => dc.Serialize(OaiNamespaces.DcNamespace + "language"), elements);
			AddSerializedList(metadata.Audience, s => new XElement(OaiNamespaces.DcTermsNamespace + "audience", s), elements);
			AddSerializedList(metadata.Formats, dc => dc.Serialize(OaiNamespaces.DcNamespace + "format"), elements);
			AddSerializedList(metadata.Publishers, dc => dc.Serialize(OaiNamespaces.DcNamespace + "publisher"), elements);
			AddSerializedList(metadata.Descriptions, dc => dc.Serialize(OaiNamespaces.DcNamespace + "description"), elements);
			AddSerializedList(metadata.Sources, dc => dc.Serialize(OaiNamespaces.DcNamespace + "source"), elements);
			AddSerializedList(metadata.Coverages, dc => dc.Serialize(OaiNamespaces.DcNamespace + "coverage"), elements);
			AddSerializedList(metadata.FileLocations, f => f.Serialize(), elements);

			elements = elements.Where(e => e != null).ToList();

			return elements;
		}

		private void AddElement(IXmlSerializableMetadataElement metadataElement, ICollection<XElement> elements)
		{
			if (metadataElement != null)
			{
				var serializedElement = metadataElement.Serialize();
				elements.Add(serializedElement);
			}
		}

		private void AddListElement<TMetadataElement>(BaseListElement<TMetadataElement> metadataElement, ICollection<XElement> elements)
		  where TMetadataElement : IXmlSerializableMetadataElement
		{
			if (metadataElement != null && metadataElement.Items.Any())
			{
				var serializedElement = metadataElement.Serialize();
				elements.Add(serializedElement);
			}
		}

		private void AddSerializedList<TInput>(IEnumerable<TInput> elements, Func<TInput, XElement> serializeFunc, List<XElement> xElements)
		{
			var serializedElements = elements
			  .Select(serializeFunc)
			  .ToList();

			xElements.AddRange(serializedElements);
		}

		private void ReadOpenAireElement(string elementName, OpenAireMetadata metadata, XmlReader reader)
		{
			elementName = elementName.Replace("oaire:", "");

			if (elementName == "citationTitle")
			{
				metadata.CitationTitle = reader.ReadInnerXml();
			}
			else if (elementName == "citationVolume")
			{
				int.TryParse(reader.ReadInnerXml(), out var citationVolume);
				metadata.CitationVolume = citationVolume;
			}
			else if (elementName == "citationIssue")
			{
				int.TryParse(reader.ReadInnerXml(), out var citationIssue);
				metadata.CitationIssue = citationIssue;
			}
			else if (elementName == "citationStartPage")
			{
				int.TryParse(reader.ReadInnerXml(), out var citationStartPage);
				metadata.CitationStartPage = citationStartPage;
			}
			else if (elementName == "citationEdition")
			{
				int.TryParse(reader.ReadInnerXml(), out var citationEdition);
				metadata.CitationEdition = citationEdition;
			}
			else if (elementName == "citationConferencePlace")
			{
				metadata.CitationConferencePlace = reader.ReadInnerXml();
			}
			else if (elementName == "citationConferenceDate")
			{
				metadata.CitationConferenceDate = reader.ReadInnerXml();
			}
			else if (elementName == "citationEndPage")
			{
				int.TryParse(reader.ReadInnerXml(), out var citationEndPage);
				metadata.CitationEndPage = citationEndPage;
			}
			else
			{
				var element = XElement.Load(reader.ReadSubtree());

				if (elementName == "fundingReferences")
				{
					metadata.FundingReferences = ReadElement<OpenAireFundingReferences>(element);
				}
				else if (elementName == "resourceType")
				{
					metadata.ResourceType = ReadElement<OpenAireResourceType>(element);
				}
				else if (elementName == "version")
				{
					metadata.ResourceVersion = ReadElement<OpenAireVersion>(element);
				}
				else if (elementName == "file")
				{
					metadata.FileLocations.Add(ReadElement<OpenAireFile>(element));
				}
				else if (elementName == "licenseCondition")
				{
					metadata.LicenseCondition = ReadElement<LicenseCondition>(element);
				}
			}
		}

		private void ReadDataciteElement(string elementName, OpenAireMetadata metadata, XmlReader reader)
		{
			var element = XElement.Load(reader.ReadSubtree());

			if (elementName == "titles")
			{
				metadata.Titles = ReadElement<OpenAireTitles>(element);
			}

			if (elementName == "creators")
			{
				metadata.Creators = ReadElement<OpenAireCreators>(element);
			}

			if (elementName == "contributors")
			{
				metadata.Contributors = ReadElement<OpenAireContributors>(element);
			}

			if (elementName == "alternateIdentifiers")
			{
				metadata.AlternateIdentifiers = ReadElement<OpenAireAlternateIdentifiers>(element);
			}

			if (elementName == "relatedIdentifiers")
			{
				metadata.RelatedIdentifiers = ReadElement<OpenAireRelatedIdentifiers>(element);
			}

			if (elementName == "dates")
			{
				metadata.Dates = ReadElement<OpenAireDates>(element);
			}

			if (elementName == "date")
			{
				metadata.PublicationDate = ReadElement<OpenAireDatesDate>(element);
			}

			if (elementName == "identifier")
			{
				metadata.ResourceIdentifier = ReadElement<OpenAireIdentifier>(element);
			}

			if (elementName == "rights")
			{
				metadata.AccessRigths = ReadElement<OpenAireRights>(element);
			}

			if (elementName == "subjects")
			{
				metadata.Subjects = ReadElement<OpenAireSubjects>(element);
			}

			if (elementName == "sizes")
			{
				metadata.Sizes = ReadElement<OpenAireSizes>(element);
			}
		}

		private void ReadDcTermsElement(string elementName, OpenAireMetadata metadata, XmlReader reader)
		{
			var element = XElement.Load(reader.ReadSubtree());

			if (elementName == "audience")
			{
				metadata.Audience.Add(element.Value);
			}
		}

		public void ReadDublinCoreElement(string elementName, OpenAireMetadata metadata, XmlReader reader)
		{
			ICollection<DublinCoreElement> collection = null;

			if (elementName == "description") { collection = metadata.Descriptions; }
			else if (elementName == "publisher") { collection = metadata.Publishers; }
			else if (elementName == "format") { collection = metadata.Formats; }
			else if (elementName == "source") { collection = metadata.Sources; }
			else if (elementName == "language") { collection = metadata.Languages; }
			else if (elementName == "coverage") { collection = metadata.Coverages; }

			collection?.Add(dublinCoreMetadataConverter.ReadElement(reader));
		}

		private TElement ReadElement<TElement>(XElement xElement)
			where TElement : IXmlDeserializableMetadataElement, new()
		{
			var element = new TElement();
			element.ParseFromXElement(xElement);

			return element;
		}
	}
}
