using System.Collections.Generic;
using System.Linq;
using MetadataHarvesting.Models;
using MetadataHarvesting.Models.DublinCore;
using MetadataHarvesting.Models.Metadata.OpenAire;

namespace MetadataProvider.Core.Repositories
{
	public class MetadataFormatRepository : IMetadataFormatRepository
	{
		private readonly IDictionary<string, MetadataFormat> supportedMetadataFormats =
			new Dictionary<string, MetadataFormat>
			{
				{DublinCoreMetadata.MetadataFormatPrefix,new MetadataFormat(DublinCoreMetadata.MetadataFormatPrefix, OaiNamespaces.OaiDcNamespace, OaiNamespaces.OaiDcSchema,OaiNamespaces.OaiDcSchemaLocation) },
				{OpenAireMetadata.MetadataFormatPrefix,new MetadataFormat(OpenAireMetadata.MetadataFormatPrefix, null, null,
					null) }
			};

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
