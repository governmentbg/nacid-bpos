using OpenScience.Data.Publications.Enums;

namespace OpenScience.Data.Publications.Models
{
	public class PublicationFileLocation : PublicationEntity
	{
		public string AccessRightsUri { get; set; }

		public string MimeType { get; set; }

		public ObjectType ObjectType { get; set; }

		public string FileUrl { get; set; }
	}
}
