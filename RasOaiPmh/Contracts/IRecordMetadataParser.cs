using NacidRas.Integrations.OaiPmhProvider.Models;

namespace NacidRas.Integrations.OaiPmhProvider.Contracts
{
	public interface IRecordMetadataParser
	{
		string Prefix { get; }

		RecordMetadata ParseRecordMetadata(string xml);
	}
}
