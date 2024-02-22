using MetadataHarvesting.Models;

namespace MetadataHarvesting.Core.Converters
{
	public interface IMetadataParser
	{
		string Prefix { get; }

		RecordMetadata ParseMetadata(string xml);
	}
}
