using System.Xml.Linq;
using MetadataHarvesting.Models;

namespace MetadataHarvesting.Core.Converters
{
	public interface IMetadataEncoder
	{
		string Prefix { get; }

		XElement Encode(RecordMetadata metadata);
	}
}
