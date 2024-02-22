using System.Xml.Linq;
using NacidRas.Integrations.OaiPmhProvider.Models;

namespace NacidRas.Integrations.OaiPmhProvider.Converters
{
	public interface IMetadataEncoder
	{
		string Prefix { get; }

		XElement Encode(RecordMetadata metadata);
	}
}
