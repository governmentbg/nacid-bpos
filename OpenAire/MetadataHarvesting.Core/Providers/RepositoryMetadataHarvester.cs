using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using MetadataHarvesting.Core.Exceptions;
using MetadataHarvesting.Core.Parsers;
using MetadataHarvesting.Core.Providers;
using MetadataHarvesting.Models;

namespace MetadataHarvesting.Core
{
	public class RepositoryMetadataHarvester : IRepositoryMetadataHarvester
	{
		private readonly PmhResponseParser parser;
		private readonly IHttpClientFactory httpClientFactory;

		public RepositoryMetadataHarvester(string baseUrl, PmhResponseParser parser, IHttpClientFactory httpClientFactory)
		{
			this.parser = parser;
			this.httpClientFactory = httpClientFactory;
			this.BaseUrl = baseUrl;
		}

		private string BaseUrl { get; }

		public string ResponseDate { set; get; }

		public async Task<Identify> Identify()
		{
			var requestUrl = $"{BaseUrl}?verb=Identify";

			return await ExecuteRequest<Identify>(requestUrl, "Identify", parser.ParseIdentify);
		}

		public async Task<ListMetadataFormats> ListMetadataFormats(string sidentifier = null)
		{
			var requestUrl = $"{BaseUrl}?verb=ListMetadataFormats";

			if (sidentifier != null)
			{
				requestUrl = $"{requestUrl}&identifier={sidentifier}";
			}

			return await ExecuteRequest<ListMetadataFormats>(requestUrl, "ListMetadataFormats",
				parser.ParseListMetadataFormats);
		}

		public async Task<ListContainer<Identifiers>> ListIdenifiers(string prefix = null, string set = null, string from = null, string until = null, ResumptionToken resumptionToken = null)
		{
			var requestUrl = BuildListRecordsOrIdentifiersUrl("ListIdentifiers", prefix, set, from, until, resumptionToken);

			return await ExecuteRequest<ListContainer<Identifiers>>(requestUrl, "ListIdentifiers", parser.ParseListIdentifier);
		}

		public async Task<Record> GetRecord(string identifier = null, string prefix = null)
		{
			prefix = prefix ?? "oai_dc";

			var requestUrl = $"{BaseUrl}?verb=GetRecord&metadataPrefix={prefix}&identifier={identifier}";

			return await ExecuteRequest<Record>(requestUrl, "GetRecord", (result, reader) => parser.ParseGetRecord(result, reader, prefix));
		}

		public async Task<ListContainer<Set>> ListSets(ResumptionToken resumptionToken)
		{
			var requestUrl = $"{BaseUrl}?verb=ListSets";

			if (resumptionToken != null)
			{
				requestUrl = $"{requestUrl}&resumptionToken={resumptionToken.Token}";
			}

			return await ExecuteRequest<ListContainer<Set>>(requestUrl, "ListSets", parser.ParseListSets);
		}

		public async Task<ListContainer<Record>> ListRecords(string prefix = null, string set = null, string from = null, string until = null, ResumptionToken resumptionToken = null)
		{
			var requestUrl = BuildListRecordsOrIdentifiersUrl("ListRecords", prefix, set, from, until, resumptionToken);

			return await ExecuteRequest<ListContainer<Record>>(requestUrl, "ListRecords", (result, reader) => parser.ParseListRecord(result, reader, prefix));
		}

		private string BuildListRecordsOrIdentifiersUrl(string verb, string prefix = null, string set = null, string from = null, string until = null, ResumptionToken resumptionToken = null)
		{
			var requestUrl = $"{BaseUrl}?verb={verb}";
			prefix = prefix ?? "oai_dc";

			if (resumptionToken == null)
			{
				requestUrl = $"{requestUrl}&metadataPrefix={prefix}";

				if (!string.IsNullOrEmpty(set))
				{
					requestUrl = $"{requestUrl}&set={set}";
				}
				if (!string.IsNullOrEmpty(from))
				{
					requestUrl = $"{requestUrl}&from={from}";
				}
				if (!string.IsNullOrEmpty(until))
				{
					requestUrl = $"{requestUrl}&until={until}";
				}
			}
			else
			{
				requestUrl = $"{requestUrl}&resumptionToken={resumptionToken.Token}";
			}

			return requestUrl;
		}

		private async Task<string> GetXml(string requestUrl)
		{
			try
			{
				var client = httpClientFactory.CreateClient();

				using (var response = await client.GetAsync(requestUrl))
				{
					response.EnsureSuccessStatusCode();
					var xml = await response.Content.ReadAsStringAsync();

					return xml;
				}
			}
			catch (Exception e)
			{
				var errorMessage = $"Unable to connect to {BaseUrl}; Error message: {e.Message}; Exception {e}";

				throw new RepositoryConnectionFailureException(errorMessage);
			}
		}

		private TResult ParseXmlResult<TResult>(XmlTextReader reader, string readerName, Action<TResult, XmlTextReader> customParseLogic)
			where TResult : class, new()
		{
			var result = new TResult();
			while (reader.Read())
			{
				if (reader.NodeType != XmlNodeType.Element)
				{
					continue;
				}

				if (reader.Name == "responseDate")
				{
					ResponseDate = reader.ReadString();
				}
				else if (reader.Name == readerName)
				{
					customParseLogic.Invoke(result, reader);
				}
				else if (reader.Name == "error")
				{
					var errorName = reader.GetAttribute("code");

					if (errorName == OaiErrorCodes.NoRecordsMatch)
					{
						break;
					}

					var errorDescription = reader.ReadString();
					var message = $"{errorName}: {errorDescription}";

					throw new HarvestingException(message);
				}
			}

			reader.Close();

			return result;
		}

		private async Task<TResult> ExecuteRequest<TResult>(string requestUrl, string readerName, Action<TResult, XmlTextReader> customParseLogic)
			where TResult : class, new()
		{
			var xml = await GetXml(requestUrl);

			if (string.IsNullOrEmpty(xml))
			{
				return null;
			}

			var reader = new XmlTextReader(xml, XmlNodeType.Document, null);

			return ParseXmlResult(reader, readerName, customParseLogic);
		}
	}
}