using System.Collections.Generic;
using System.Xml.Linq;

namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public class OpenAireVersion : IXmlSerializableMetadataElement
	{
		private readonly IDictionary<Version, string> versionUriMap = new Dictionary<Version, string>
		{
	{Version.AuthorsOriginal,"http://purl.org/coar/version/c_b1a7d7d4d402bcce"},
	{Version.SubmittedManuscriptUnderReview,"http://purl.org/coar/version/c_71e4c1898caa6e32"},
	{Version.AcceptedManuscript,"http://purl.org/coar/version/c_ab4af688f83e57aa"},
	{Version.Proof,"http://purl.org/coar/version/c_fa2ee174bc00049f"},
	{Version.VersionOfRecord,"http://purl.org/coar/version/c_970fb48d4fbd8a85"},
	{Version.CorrectedVersionOfRecord,"http://purl.org/coar/version/c_e19f295774971610"},
	{Version.EnhancedVersionOfRecord,"http://purl.org/coar/version/c_dc82b40f9837b551"},
	{Version.NotApplicable,"http://purl.org/coar/version/c_be7fb7dd8ff6fe43"},
  };

		private readonly IDictionary<Version, string> versionValueMap = new Dictionary<Version, string>
		{
	  {Version.AuthorsOriginal,"AO"},
	  {Version.SubmittedManuscriptUnderReview,"SMUR"},
	  {Version.AcceptedManuscript,"AM"},
	  {Version.Proof,"P"},
	  {Version.VersionOfRecord,"VoR"},
	  {Version.CorrectedVersionOfRecord,"CVoR"},
	  {Version.EnhancedVersionOfRecord,"EVoR"},
	  {Version.NotApplicable,"NA"},
	};

		public Version? VersionType { get; set; }
		public string Uri => VersionType.HasValue ? versionUriMap[VersionType.Value] : string.Empty;
		public string Value { get; set; }

		public XElement Serialize()
		{
			var attributes = new List<XAttribute>();

			if (VersionType.HasValue)
			{
				attributes.Add(new XAttribute("uri", Uri));
			}

			return new XElement(OaiNamespaces.OpenAireNamespace + "version", attributes, VersionType.HasValue ? versionValueMap[VersionType.Value] : Value);
		}
	}
}
