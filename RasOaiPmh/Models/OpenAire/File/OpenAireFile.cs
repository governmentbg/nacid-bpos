using System.Collections.Generic;
using System.Xml.Linq;

namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public class OpenAireFile : IXmlSerializableMetadataElement
	{
		private readonly IDictionary<AccessRightsType, string> accessRightsTypeUrisMap = new Dictionary<AccessRightsType, string>
		{
			{AccessRightsType.OpenAccess,"http://purl.org/coar/access_right/c_abf2" },
			{AccessRightsType.EmbargoedAccess,"http://purl.org/coar/access_right/c_f1cf" },
			{AccessRightsType.RestrictedAccess,"http://purl.org/coar/access_right/c_16ec" },
			{AccessRightsType.MetadataOnlyAccess,"http://purl.org/coar/access_right/c_14cb" },
		};

		public string MimeType { get; set; }
		public AccessRightsType AccessRightsType { get; set; }
		public ObjectType ObjectType { get; set; }
		public string Value { get; set; }

		public XElement Serialize()
		{
			return new XElement(OaiNamespaces.OpenAireNamespace + "file",
				new XAttribute("accessRightsURI", accessRightsTypeUrisMap[AccessRightsType]),
				new XAttribute("mimeType", MimeType),
				new XAttribute("objectType", ObjectType.ToString().ToLower()),
				Value);
		}
	}
}
