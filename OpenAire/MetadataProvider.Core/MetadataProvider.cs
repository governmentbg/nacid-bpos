using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using MetadataHarvesting.Core.Converters;
using MetadataHarvesting.Models;

namespace MetadataProvider.Core
{
	/// <summary>
	/// A Data Provider is a participant in the OAI-PMH framework. It administers systems that support the OAI-PMH as a means of exposing metadata.
	/// </summary>
	public class MetadataProvider
	{
		private readonly OaiConfiguration configuration;
		private readonly IDateConverter dateConverter;
		private readonly IResumptionTokenConverter resumptionTokenConverter;

		private readonly IMetadataFormatRepository metadataFormatRepository;
		private readonly IRecordRepository recordRepository;
		private readonly ISetRepository setRepository;

		public MetadataProvider(OaiConfiguration configuration,
			IMetadataFormatRepository metadataFormatRepository,
			IRecordRepository recordRepository,
			ISetRepository setRepository,
			IDateConverter dateConverter,
			IResumptionTokenConverter resumptionTokenConverter)
		{
			this.configuration = configuration;
			this.dateConverter = dateConverter;
			this.resumptionTokenConverter = resumptionTokenConverter;
			this.metadataFormatRepository = metadataFormatRepository;
			this.recordRepository = recordRepository;
			this.setRepository = setRepository;
		}

		/// <summary>
		/// Transforms the provided OAI-PMH arguments to an XML document.
		/// </summary>
		/// <param name="date">The date to be included in the response.</param>
		/// <param name="arguments">The OAI-PMH arguments.</param>
		/// <returns>The transformed OAI-PMH arguments as an XML document.</returns>
		public async Task<XDocument> ToXDocument(DateTime date, ArgumentContainer arguments)
		{
			if (Enum.TryParse(arguments.Verb, out OaiVerb parsedVerb))
			{
				switch (parsedVerb)
				{
					case OaiVerb.Identify:
						return CreateIdentify(date, arguments);
					case OaiVerb.ListMetadataFormats:
						return CreateListMetadataFormats(date, arguments);
					case OaiVerb.ListRecords:
						return await CreateListIdentifiersOrRecords(date, arguments, parsedVerb);
					case OaiVerb.ListIdentifiers:
						return await CreateListIdentifiersOrRecords(date, arguments, parsedVerb);
					case OaiVerb.GetRecord:
						return await CreateGetRecord(date, arguments);
					case OaiVerb.ListSets:
						return await CreateListSets(date, arguments);
					default:
						return CreateErrorDocument(date, OaiVerb.None, arguments, OaiErrors.BadVerb);
				}
			}

			return CreateErrorDocument(date, OaiVerb.None, arguments, OaiErrors.BadVerb);
		}

		/// <summary>
		/// Transforms the provided OAI-PMH arguments to a string.
		/// </summary>
		/// <param name="date">The date to be included in the response.</param>
		/// <param name="arguments">The OAI-PMH arguments.</param>
		/// <returns>The transformed OAI-PMH arguments as a string.</returns>
		public async Task<string> ToString(DateTime now, ArgumentContainer arguments)
		{
			var document = await ToXDocument(now, arguments);

			return ToString(document);
		}

		/// <summary>
		/// Transforms the provided XML document to a string.
		/// </summary>
		/// <param name="document">The XML document to be transformed.</param>
		/// <returns>The transformed XML document as a string.</returns>
		public string ToString(XDocument document)
		{
			string declaration = document.Declaration == null ? new XDeclaration("1.0", "utf-8", "no").ToString() : document.Declaration.ToString();

			return (string.Concat(declaration, Environment.NewLine, document.ToString()));
		}

		/// <summary>
		/// Creates the response in the form of an XML document.
		/// </summary>
		/// <param name="date">The date to be used in the response.</param>
		/// <param name="oaiElements">First OAI element should be the request. 
		/// The second OAI element should be either an error or a verb.</param>
		/// <returns>Complete response as XML document.</returns>
		private XDocument CreateXml(DateTime date, XElement[] oaiElements)
		{
			foreach (var oaiElement in oaiElements)
				SetDefaultXNamespace(oaiElement, OaiNamespaces.OaiNamespace);

			return new XDocument(new XDeclaration("1.0", "utf-8", "no"),
				new XElement(OaiNamespaces.OaiNamespace + "OAI-PMH",
					new XAttribute(XNamespace.Xmlns + "xsi", OaiNamespaces.XsiNamespace),
					new XAttribute(OaiNamespaces.XsiNamespace + "schemaLocation", OaiNamespaces.OaiSchemaLocation),
						new XElement(OaiNamespaces.OaiNamespace + "responseDate",
							dateConverter.Encode(configuration.Identify.Granularity, date)),
							oaiElements));
		}

		private void SetDefaultXNamespace(XElement xelem, XNamespace xmlns)
		{
			if (xelem.Name.NamespaceName == string.Empty)
				xelem.Name = xmlns + xelem.Name.LocalName;

			foreach (var currentElement in xelem.Elements())
				SetDefaultXNamespace(currentElement, xmlns);
		}

		private XElement CreateRequest(OaiVerb verb, ArgumentContainer arguments)
		{
			var attributes = new List<object>();

			switch (verb)
			{
				case OaiVerb.None:
					// don't add any attributes
					break;
				default:
					attributes.Add(new XAttribute("verb", verb.ToString()));
					if (!string.IsNullOrWhiteSpace(arguments.Identifier))
						attributes.Add(new XAttribute("identifier", arguments.Identifier));
					if (!string.IsNullOrWhiteSpace(arguments.MetadataPrefix))
						attributes.Add(new XAttribute("metadataPrefix", arguments.MetadataPrefix));
					if (!string.IsNullOrWhiteSpace(arguments.ResumptionToken))
						attributes.Add(new XAttribute("resumptionToken", arguments.ResumptionToken));
					if (!string.IsNullOrWhiteSpace(arguments.From) && dateConverter.TryDecode(arguments.From, out _))
						attributes.Add(new XAttribute("from", arguments.From));
					if (!string.IsNullOrWhiteSpace(arguments.Until) && dateConverter.TryDecode(arguments.Until, out _))
						attributes.Add(new XAttribute("until", arguments.Until));
					if (!string.IsNullOrWhiteSpace(arguments.Set))
						attributes.Add(new XAttribute("set", arguments.Set));
					break;
			}

			return new XElement("request", configuration.Identify.BaseUrl, attributes);
		}

		private XDocument CreateErrorDocument(DateTime date, OaiVerb verb, ArgumentContainer arguments, XElement error)
		{
			IList<XElement> root = new List<XElement>();
			root.Add(CreateRequest(verb, arguments));
			root.Add(error);

			return CreateXml(date, root.ToArray());
		}

		private XElement CreateHeaderXElement(Header header)
		{
			var element = new XElement("header");

			TryAddXElement(element, "identifier", header.Identifier);
			TryAddXElement(element, "datestamp", dateConverter.Encode(configuration.Identify.Granularity, header.Datestamp));
			foreach (var setSpec in header.SetSpecs)
				TryAddXElement(element, "setSpec", setSpec);
			TryAddXElement(element, "status", header.Status);

			return element;
		}

		private XElement CreateMetadataXElement(RecordMetadata metadata)
		{
			return new XElement("metadata", metadata.Content);
		}

		private void TryAddXElement(XElement root, XName name, string content)
		{
			if (!string.IsNullOrWhiteSpace(content))
				root.Add(new XElement(name, content));
		}

		private string GetDisplayGranularity()
		{
			if (string.IsNullOrWhiteSpace(configuration.Identify.Granularity))
			{
				return string.Empty;
			}
			else
			{
				string granularity = configuration.Identify.Granularity.Replace("'", "").ToLowerInvariant();
				switch (granularity)
				{
					case "yyyy-mm-dd":
						return "YYYY-MM-DD";
					case "yyyy-mm-ddthh:mm:ssz":
						return "YYYY-MM-DDThh:mm:ssZ";
					default:
						return string.Empty;
				}
			}
		}

		private async Task<XDocument> CreateGetRecord(DateTime date, ArgumentContainer arguments)
		{
			var verb = OaiVerb.GetRecord;

			var allowedArguments = OaiArgument.Identifier | OaiArgument.MetadataPrefix;

			if (!OaiErrors.ValidateArguments(arguments, allowedArguments, out XElement errorElement))
				return CreateErrorDocument(date, verb, arguments, errorElement);

			// Check if required identifier is included in the request
			if (string.IsNullOrWhiteSpace(arguments.Identifier))
				return CreateErrorDocument(date, verb, arguments, OaiErrors.BadIdentifierArgument);

			// Check if required metadata prefix is included in the request
			if (string.IsNullOrWhiteSpace(arguments.MetadataPrefix))
				return CreateErrorDocument(date, verb, arguments, OaiErrors.BadMetadataArgument);

			// Check if metadata prefix is supported
			var metadataPrefix = metadataFormatRepository.GetMetadataFormat(arguments.MetadataPrefix);
			if (metadataPrefix == null)
				return CreateErrorDocument(date, verb, arguments, OaiErrors.CannotDisseminateFormat);

			var record = await recordRepository.GetRecord(arguments.Identifier, arguments.MetadataPrefix);

			if (record == null)
				return CreateErrorDocument(date, verb, arguments, OaiErrors.IdDoesNotExist);

			var root = new List<XElement> { CreateRequest(verb, arguments) };

			var content = new XElement(verb.ToString());
			root.Add(content);

			var recordElement = new XElement("record");
			content.Add(recordElement);

			// Header
			if (record.Header != null)
				recordElement.Add(CreateHeaderXElement(record.Header));
			// Metadata
			if (record.RecordMetadata != null)
				recordElement.Add(CreateMetadataXElement(record.RecordMetadata));

			return CreateXml(date, root.ToArray());
		}

		private XDocument CreateIdentify(DateTime date, ArgumentContainer arguments)
		{
			var verb = OaiVerb.Identify;

			var allowedArguments = OaiArgument.None;

			if (!OaiErrors.ValidateArguments(arguments, allowedArguments, out XElement errorElement))
				return CreateErrorDocument(date, verb, arguments, errorElement);

			var root = new List<XElement> { CreateRequest(verb, arguments) };

			var content = new XElement(verb.ToString());
			root.Add(content);

			TryAddXElement(content, "repositoryName", configuration.Identify.RepositoryName);
			TryAddXElement(content, "baseURL", configuration.Identify.BaseUrl);
			TryAddXElement(content, "protocolVersion", configuration.Identify.ProtocolVersion);

			if (configuration.Identify.AdminEmails != null)
			{
				foreach (var adminEmail in configuration.Identify.AdminEmails)
					TryAddXElement(content, "adminEmail", adminEmail);
			}

			TryAddXElement(content, "earliestDatestamp", configuration.Identify.EarliestDatestamp);
			TryAddXElement(content, "deletedRecord", configuration.Identify.DeletedRecord);
			TryAddXElement(content, "granularity", GetDisplayGranularity());

			if (configuration.Identify.Compressions != null)
			{
				foreach (var compression in configuration.Identify.Compressions)
					TryAddXElement(content, "compression", compression);
			}

			if (configuration.Identify.Descriptions != null)
			{
				foreach (var description in configuration.Identify.Descriptions)
					TryAddXElement(content, "description", description);
			}

			return CreateXml(date, root.ToArray());
		}

		private XDocument CreateListMetadataFormats(DateTime date, ArgumentContainer arguments)
		{
			var verb = OaiVerb.ListMetadataFormats;

			var allowedArguments = OaiArgument.Identifier;

			if (!OaiErrors.ValidateArguments(arguments, allowedArguments, out XElement errorElement))
				return CreateErrorDocument(date, verb, arguments, errorElement);

			if (!string.IsNullOrWhiteSpace(arguments.Identifier) && recordRepository.GetRecord(arguments.Identifier, arguments.MetadataPrefix) == null)
				return CreateErrorDocument(date, verb, arguments, OaiErrors.IdDoesNotExist);

			var formats = metadataFormatRepository.GetMetadataFormats().OrderBy(e => e.Prefix);

			if (!formats.Any())
				return CreateErrorDocument(date, verb, arguments, OaiErrors.NoMetadataFormats);

			var root = new List<XElement> { CreateRequest(verb, arguments) };

			var content = new XElement(verb.ToString());
			root.Add(content);

			foreach (var format in formats)
			{
				XElement formatElement = new XElement("metadataFormat");
				content.Add(formatElement);
				TryAddXElement(formatElement, "metadataPrefix", format.Prefix);
				TryAddXElement(formatElement, "schema", format.Schema?.ToString());
				TryAddXElement(formatElement, "metadataNamespace", format.Namespace?.ToString());
			}

			return CreateXml(date, root.ToArray());
		}

		private async Task<XDocument> CreateListIdentifiersOrRecords(DateTime date, ArgumentContainer arguments, OaiVerb verb, ResumptionToken resumptionToken = null)
		{
			var allowedArguments = OaiArgument.MetadataPrefix | OaiArgument.ResumptionToken |
										   OaiArgument.From | OaiArgument.Until | OaiArgument.Set;

			if (!OaiErrors.ValidateArguments(arguments, allowedArguments, out XElement errorElement))
				return CreateErrorDocument(date, verb, arguments, errorElement);

			// Set
			if (!string.IsNullOrWhiteSpace(arguments.Set) && !configuration.SupportSets)
				return CreateErrorDocument(date, verb, arguments, OaiErrors.NoSetHierarchy);

			// From
			var fromDate = DateTime.MinValue;
			if (!string.IsNullOrWhiteSpace(arguments.From) && !dateConverter.TryDecode(arguments.From, out fromDate))
				return CreateErrorDocument(date, verb, arguments, OaiErrors.BadFromArgument);

			// Until
			var untilDate = DateTime.MaxValue;
			if (!string.IsNullOrWhiteSpace(arguments.Until) && !dateConverter.TryDecode(arguments.Until, out untilDate))
				return CreateErrorDocument(date, verb, arguments, OaiErrors.BadUntilArgument);

			// The from argument must be less than or equal to the until argument. 
			if (fromDate > untilDate)
				return CreateErrorDocument(date, verb, arguments, OaiErrors.BadFromUntilCombinationArgument);

			// Decode ResumptionToken
			if (resumptionToken == null && !string.IsNullOrWhiteSpace(arguments.ResumptionToken))
			{
				if (!OaiErrors.ValidateArguments(arguments, OaiArgument.ResumptionToken))
					return CreateErrorDocument(date, verb, arguments, OaiErrors.BadArgumentExclusiveResumptionToken);

				try
				{
					var decodedResumptionToken = resumptionTokenConverter.Decode(arguments.ResumptionToken);
					if (decodedResumptionToken.ExpirationDate >= DateTime.UtcNow)
						return CreateErrorDocument(date, verb, arguments, OaiErrors.BadResumptionToken);

					var resumptionTokenArguments = new ArgumentContainer(
						verb.ToString(), decodedResumptionToken.MetadataPrefix, arguments.ResumptionToken, null,
						dateConverter.Encode(configuration.Identify.Granularity, decodedResumptionToken.From),
						dateConverter.Encode(configuration.Identify.Granularity, decodedResumptionToken.Until),
						decodedResumptionToken.Set);

					return await CreateListIdentifiersOrRecords(date, resumptionTokenArguments, verb, decodedResumptionToken);
				}
				catch (Exception)
				{
					return CreateErrorDocument(date, verb, arguments, OaiErrors.BadResumptionToken);
				}
			}

			// Check if required metadata prefix is included in the request
			if (string.IsNullOrWhiteSpace(arguments.MetadataPrefix))
				return CreateErrorDocument(date, verb, arguments, OaiErrors.BadMetadataArgument);

			// Check if metadata prefix is supported
			var metadataPrefix = metadataFormatRepository.GetMetadataFormat(arguments.MetadataPrefix);
			if (metadataPrefix == null)
				return CreateErrorDocument(date, verb, arguments, OaiErrors.CannotDisseminateFormat);

			var recordContainer = await (verb == OaiVerb.ListRecords ?
				 recordRepository.GetRecords(arguments, resumptionToken) :
				 recordRepository.GetIdentifiers(arguments, resumptionToken));

			if (recordContainer == null || !recordContainer.Items.Any())
				return CreateErrorDocument(date, verb, arguments, OaiErrors.NoRecordsMatch);

			var root = new List<XElement> { CreateRequest(verb, arguments) };

			var content = new XElement(verb.ToString());
			root.Add(content);

			foreach (var record in recordContainer.Items)
			{
				var recordElement = content;

				if (verb == OaiVerb.ListRecords)
				{
					recordElement = new XElement("record");
					content.Add(recordElement);
				}

				// Header
				if (record.Header != null)
					recordElement.Add(CreateHeaderXElement(record.Header));
				// Metadata
				if (record.RecordMetadata != null && verb == OaiVerb.ListRecords)
					recordElement.Add(CreateMetadataXElement(record.RecordMetadata));
			}

			AddResumptionToken(recordContainer, date, content);

			return CreateXml(date, root.ToArray());
		}

		private async Task<XDocument> CreateListSets(DateTime date, ArgumentContainer arguments, ResumptionToken resumptionToken = null)
		{
			var verb = OaiVerb.ListSets;

			var allowedArguments = OaiArgument.ResumptionToken;

			if (!OaiErrors.ValidateArguments(arguments, allowedArguments, out XElement errorElement))
				return CreateErrorDocument(date, verb, arguments, errorElement);

			// Set
			if (!configuration.SupportSets)
				return CreateErrorDocument(date, verb, arguments, OaiErrors.NoSetHierarchy);

			// Decode ResumptionToken
			if (resumptionToken == null && !string.IsNullOrWhiteSpace(arguments.ResumptionToken))
			{
				if (!OaiErrors.ValidateArguments(arguments, OaiArgument.ResumptionToken))
					return CreateErrorDocument(date, verb, arguments, OaiErrors.BadArgumentExclusiveResumptionToken);

				var decodedResumptionToken = resumptionTokenConverter.Decode(arguments.ResumptionToken);
				if (decodedResumptionToken.ExpirationDate >= DateTime.UtcNow)
					return CreateErrorDocument(date, verb, arguments, OaiErrors.BadResumptionToken);

				var resumptionTokenArguments = new ArgumentContainer(
					verb.ToString(), decodedResumptionToken.MetadataPrefix, arguments.ResumptionToken, null,
					dateConverter.Encode(configuration.Identify.Granularity, decodedResumptionToken.From),
					dateConverter.Encode(configuration.Identify.Granularity, decodedResumptionToken.Until),
					decodedResumptionToken.Set);

				return await CreateListSets(date, resumptionTokenArguments, decodedResumptionToken);
			}

			var setContainer = await setRepository.GetSetsAsync(arguments, resumptionToken);

			var root = new List<XElement> { CreateRequest(verb, arguments) };

			var content = new XElement(verb.ToString());
			root.Add(content);

			if (setContainer != null)
			{
				foreach (var set in setContainer.Items)
				{
					var setElement = new XElement("set");
					content.Add(setElement);
					TryAddXElement(setElement, "setSpec", set.Spec);
					TryAddXElement(setElement, "setName", set.Name);
					TryAddXElement(setElement, "setDescription", set.Description);
					foreach (var additionalDescription in set.AdditionalDescriptions)
						setElement.Add(new XElement("setDescription", additionalDescription));
				}

				AddResumptionToken(setContainer, date, content);
			}

			return CreateXml(date, root.ToArray());
		}

		private void AddResumptionToken<T>(ListContainer<T> container, DateTime date, XElement content)
		{
			if (container.ResumptionToken == null) return;

			container.ResumptionToken.ExpirationDate = date.AddHours(configuration.ResumptionTokenExpirationHours);

			var convertedResumptionToken = resumptionTokenConverter.ToXElement(container.ResumptionToken);
			content.Add(convertedResumptionToken);
		}
	}
}
