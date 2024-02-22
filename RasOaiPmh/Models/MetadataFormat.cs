using System.Xml.Linq;

namespace NacidRas.Integrations.OaiPmhProvider.Models
{
	public class MetadataFormat
	{
		public MetadataFormat(string prefix, XNamespace ns, XNamespace schema, XNamespace schemaLocation)
		{
			Prefix = prefix;
			Namespace = ns;
			Schema = schema;
			SchemaLocation = schemaLocation;
		}

		public string Prefix { get; }

		public XNamespace Namespace { get; }

		public XNamespace Schema { get; }

		public XNamespace SchemaLocation { get; }
	}
}
