using System.Collections.Generic;
using System.Web;
using System.Xml.Linq;
using MetadataHarvesting.Models;

namespace MetadataHarvesting.Core.Converters
{
	public sealed class ResumptionTokenConverter : IResumptionTokenConverter
	{
		private readonly OaiConfiguration configuration;
		private readonly IDateConverter dateConverter;

		public ResumptionTokenConverter(OaiConfiguration configuration, IDateConverter dateConverter)
		{
			this.configuration = configuration;
			this.dateConverter = dateConverter;
		}

		public ResumptionToken Decode(string resumptionToken)
		{
			var decoded = new ResumptionToken { Token = resumptionToken };

			if (string.IsNullOrWhiteSpace(resumptionToken))
				return decoded;

			var nvc = HttpUtility.ParseQueryString(HttpUtility.UrlDecode(resumptionToken));

			foreach (string key in nvc)
			{
				switch (key.ToLowerInvariant())
				{
					case "metadataprefix":
						decoded.MetadataPrefix = nvc[key];
						break;
					case "from":
						if (dateConverter.TryDecode(nvc[key], out var fromDate))
							decoded.From = fromDate;
						break;
					case "until":
						if (dateConverter.TryDecode(nvc[key], out var untilDate))
							decoded.Until = untilDate;
						break;
					case "set":
						decoded.Set = nvc[key];
						break;
					case "expirationdate":
						if (dateConverter.TryDecode(nvc[key], out var expirationDate))
							decoded.ExpirationDate = expirationDate;
						break;
					case "completelistsize":
						if (int.TryParse(nvc[key], out int completeListSize))
							decoded.CompleteListSize = completeListSize;
						break;
					case "cursor":
						if (int.TryParse(nvc[key], out int cursor))
							decoded.Cursor = cursor;
						break;
					default:
						if (configuration.ResumptionTokenCustomParameterNames.Contains(key))
						{
							if (decoded.Custom.ContainsKey(key))
								decoded.Custom[key] = nvc[key];
							else
								decoded.Custom.Add(key, nvc[key]);
						}
						break;
				}
			}
			return decoded;
		}

		public string Encode(ResumptionToken resumptionToken)
		{
			var properties = new List<string>();

			if (!string.IsNullOrWhiteSpace(resumptionToken.MetadataPrefix))
			{
				properties.Add(EncodeOne("metadataPrefix", resumptionToken.MetadataPrefix));
			}

			if (resumptionToken.From.HasValue)
			{
				properties.Add(EncodeOne("from", dateConverter.Encode(configuration.Identify.Granularity, resumptionToken.From)));
			}

			if (resumptionToken.Until.HasValue)
			{
				properties.Add(EncodeOne("until", dateConverter.Encode(configuration.Identify.Granularity, resumptionToken.Until)));
			}

			if (!string.IsNullOrWhiteSpace(resumptionToken.Set))
			{
				properties.Add(EncodeOne("set", resumptionToken.Set));
			}

			if (resumptionToken.Cursor.HasValue)
			{
				properties.Add(EncodeOne("cursor", resumptionToken.Cursor.Value));
			}

			foreach (var custom in resumptionToken.Custom)
			{
				if (configuration.ResumptionTokenCustomParameterNames.Contains(custom.Key))
				{
					properties.Add(EncodeOne(custom.Key, custom.Value));
				}
			}

			return HttpUtility.UrlEncode(string.Join("&", properties.ToArray()));
		}

		public string EncodeOne(string name, object value)
		{
			return string.Concat(name, "=", value.ToString());
		}

		public XElement ToXElement(ResumptionToken resumptionToken)
		{
			var root = new XElement("resumptionToken", Encode(resumptionToken));

			if (resumptionToken.ExpirationDate.HasValue)
				root.Add(new XAttribute("expirationDate", dateConverter.Encode(configuration.Identify.Granularity, resumptionToken.ExpirationDate)));
			if (resumptionToken.CompleteListSize.HasValue)
				root.Add(new XAttribute("completeListSize", resumptionToken.CompleteListSize.Value));
			if (resumptionToken.Cursor.HasValue)
				root.Add(new XAttribute("cursor", resumptionToken.Cursor.Value));

			return root;
		}
	}
}
