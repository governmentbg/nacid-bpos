using System.Linq;
using System.Xml.Linq;
using NacidRas.Integrations.OaiPmhProvider.Models.Metadata;
using System.Collections.Generic;

namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public class OpenAireContributorsContributor : IXmlSerializableMetadataElement
	{
		public OpenAireContributorsContributorContributorName ContributorName { get; set; }
		public string GivenName { get; set; }
		public string FamilyName { get; set; }
		public List<OpenAireNameIdentifier> NameIdentifiers { get; set; }
		public List<string> Affiliations { get; set; }
		public ContributorType ContributorType { get; set; }

		public OpenAireContributorsContributor()
		{
			Affiliations = new List<string>();
			NameIdentifiers = new List<OpenAireNameIdentifier>();
		}

		public XElement Serialize()
		{
			var elements = new List<XElement>();

			if (ContributorName != null)
			{
				elements.Add(ContributorName.Serialize());
			}

			if (NameIdentifiers.Any())
			{
				var identifiers = NameIdentifiers
					.Select(identifier => identifier.Serialize())
					.ToList();

				elements.AddRange(identifiers);
			}

			if (!string.IsNullOrEmpty(GivenName))
			{
				elements.Add(new XElement(OaiNamespaces.DataCiteNamespace + "givenName", GivenName));
			}

			if (!string.IsNullOrEmpty(FamilyName))
			{
				elements.Add(new XElement(OaiNamespaces.DataCiteNamespace + "familyName", FamilyName));
			}

			if (Affiliations.Any())
			{
				var identifiers = Affiliations
					.Select(affiliation => new XElement(OaiNamespaces.DataCiteNamespace + "affiliation", affiliation))
					.ToList();

				elements.AddRange(identifiers);
			}

			return new XElement(OaiNamespaces.DataCiteNamespace + "contributor", new XAttribute("contributorType", ContributorType.ToString()), elements);
		}
	}
}
