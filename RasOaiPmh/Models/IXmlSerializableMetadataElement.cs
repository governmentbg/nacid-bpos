using System.Xml.Linq;

namespace NacidRas.Integrations.OaiPmhProvider.Models
{
	public interface IXmlSerializableMetadataElement
	{
		XElement Serialize();
	}
}
