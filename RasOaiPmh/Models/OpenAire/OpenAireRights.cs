using System.Collections.Generic;
using System.Xml.Linq;

namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public class OpenAireRights : IXmlSerializableMetadataElement
	{
		private readonly IDictionary<AccessRightsType, (string value, string uri)> accessRigthsTypeValuesMap = new Dictionary<AccessRightsType, (string value, string uri)>
		{
			{AccessRightsType.OpenAccess, ("open access", "http://purl.org/coar/access_right/c_abf2" )},
			{AccessRightsType.EmbargoedAccess, ("embargoed access", "http://purl.org/coar/access_right/c_f1cf") },
			{AccessRightsType.RestrictedAccess, ("restricted access", "http://purl.org/coar/access_right/c_16ec") },
			{AccessRightsType.MetadataOnlyAccess, ("metadata only access", "http://purl.org/coar/access_right/c_14cb") },
		};

		public AccessRightsType AccessRightsType { get; set; }

		public string Value => accessRigthsTypeValuesMap[AccessRightsType].value;
		public string Uri => accessRigthsTypeValuesMap[AccessRightsType].uri;

		public XElement Serialize()
		{
			return new XElement(OaiNamespaces.DataCiteNamespace + "rights", new XAttribute("rightsURI", Uri), Value);
		}
	}
}
