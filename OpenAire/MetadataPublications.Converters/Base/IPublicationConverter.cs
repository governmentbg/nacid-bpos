using MetadataHarvesting.Models;
using OpenScience.Data.Publications.Models;

namespace MetadataPublications.Converters
{
	public interface IPublicationConverter
	{
		string Prefix { get; }

		Publication ConvertFromMetadata(RecordMetadata metadata);

		RecordMetadata ConvertToMetadata(Publication publication);
	}
}
