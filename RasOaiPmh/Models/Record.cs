using NacidRas.Integrations.OaiPmhProvider.Models.OAI;

namespace NacidRas.Integrations.OaiPmhProvider.Models
{
    public class Record
    {
        public Header Header { get; set; }

        public RecordMetadata RecordMetadata { get; set; }

        public string RecordXml { set; get; }
	}
}
