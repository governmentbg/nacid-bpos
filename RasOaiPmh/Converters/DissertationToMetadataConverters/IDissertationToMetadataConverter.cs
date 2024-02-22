using NacidRas.Integrations.OaiPmhProvider.Models;
using NacidRas.Ras;
using NacidRas.RasRegister;

namespace NacidRas.Integrations.OaiPmhProvider.Converters
{
	public interface IDissertationToMetadataConverter
	{
		string Prefix { get; }

		RecordMetadata MapToRecordMetadata(AcademicDegreePart part, Person author);
	}
}
