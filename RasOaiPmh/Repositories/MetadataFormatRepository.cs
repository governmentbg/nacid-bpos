using System.Collections.Generic;
using System.Linq;
using NacidRas.Integrations.OaiPmhProvider.Contracts;
using NacidRas.Integrations.OaiPmhProvider.Models;

namespace NacidRas.Integrations.OaiPmhProvider.Repositories
{
	public class MetadataFormatRepository : IMetadataFormatRepository
	{
		private readonly IDictionary<string, MetadataFormat> supportedMetadataFormats =
			new Dictionary<string, MetadataFormat>();

		public MetadataFormatRepository()
		{
			var dc = new MetadataFormat("oai_dc", OaiNamespaces.OaiDcNamespace, OaiNamespaces.OaiDcSchema,
				OaiNamespaces.OaiDcSchemaLocation);

			supportedMetadataFormats.Add(dc.Prefix, dc);

			var oaire = new MetadataFormat("oai_openaire", null, null,
				null);

			supportedMetadataFormats.Add(oaire.Prefix, oaire);
		}

		public MetadataFormat GetMetadataFormat(string prefix)
		{
			return supportedMetadataFormats.ContainsKey(prefix) ? supportedMetadataFormats[prefix] : null;
		}

		public IEnumerable<MetadataFormat> GetMetadataFormats()
		{
			return supportedMetadataFormats
				.Select(f => f.Value)
				.ToList();
		}
	}
}
