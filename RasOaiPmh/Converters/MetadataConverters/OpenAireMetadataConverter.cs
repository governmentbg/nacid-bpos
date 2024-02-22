using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NacidRas.Integrations.OaiPmhProvider.Models;
using NacidRas.Integrations.OaiPmhProvider.Models.Metadata.OpenAire;
using NacidRas.Integrations.OaiPmhProvider.Models.OpenAire;

namespace NacidRas.Integrations.OaiPmhProvider.Converters
{
	public class OpenAireMetadataConverter : BaseMetadataConverter<OpenAireMetadata>
	{
		public override string Prefix => OpenAireMetadata.MetadataFormatPrefix;

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

			if (!string.IsNullOrEmpty(metadata.CitationTitle)) { elements.Add(CreateElement(OaiNamespaces.OpenAireNamespace + "citationTitle", metadata.CitationTitle)); }
			if (metadata.CitationVolume.HasValue) { elements.Add(CreateElement(OaiNamespaces.OpenAireNamespace + "citationVolume", metadata.CitationVolume)); }
			if (metadata.CitationIssue.HasValue) { elements.Add(CreateElement(OaiNamespaces.OpenAireNamespace + "citationIssue", metadata.CitationIssue)); }
			if (metadata.CitationStartPage.HasValue) { elements.Add(CreateElement(OaiNamespaces.OpenAireNamespace + "citationStartPage", metadata.CitationStartPage)); }
			if (metadata.CitationEndPage.HasValue) { elements.Add(CreateElement(OaiNamespaces.OpenAireNamespace + "citationEndPage", metadata.CitationEndPage)); }
			if (metadata.CitationEdition.HasValue) { elements.Add(CreateElement(OaiNamespaces.OpenAireNamespace + "citationEdition", metadata.CitationEdition)); }
			if (!string.IsNullOrEmpty(metadata.CitationConferencePlace)) { elements.Add(CreateElement(OaiNamespaces.OpenAireNamespace + "citationConferencePlace", metadata.CitationConferencePlace)); }
			if (!string.IsNullOrEmpty(metadata.CitationConferenceDate)) { elements.Add(CreateElement(OaiNamespaces.OpenAireNamespace + "citationConferenceDate", metadata.CitationConferenceDate)); }

			AddElement(metadata.AccessRigths, elements);
			AddElement(metadata.Sizes, elements);
			AddElement(metadata.ResourceType, elements);
			AddElement(metadata.ResourceIdentifier, elements);
			AddElement(metadata.LicenseCondition, elements);
			AddElement(metadata.ResourceVersion, elements);
			AddElement(metadata.PublicationDate, elements);

			AddSerializedList(metadata.Languages, dc => dc.Serialize(OaiNamespaces.DcNamespace + "language"), elements);
			AddSerializedList(metadata.Audience, s => new XElement(OaiNamespaces.DcTermsNamespace + "audience", s), elements);
			AddSerializedList(metadata.Formats, s => new XElement(OaiNamespaces.DcNamespace + "format", s), elements);
			AddSerializedList(metadata.Publishers, dc => dc.Serialize(OaiNamespaces.DcNamespace + "publisher"), elements);
			AddSerializedList(metadata.Descriptions, dc => dc.Serialize(OaiNamespaces.DcNamespace + "description"), elements);
			AddSerializedList(metadata.Sources, dc => dc.Serialize(OaiNamespaces.DcNamespace + "source"), elements);
			AddSerializedList(metadata.Coverages, dc => dc.Serialize(OaiNamespaces.DcNamespace + "coverage"), elements);
			AddSerializedList(metadata.FileLocations, f => f.Serialize(), elements);

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
	}

}
