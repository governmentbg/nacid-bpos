using OpenScience.Data.Nomenclatures.Models;

namespace OpenScience.Data.Publications.Models
{
	public class PublicationRelatedIdentifier : PublicationEntity
	{
		public string Value { get; set; }

		public int TypeId { get; set; }
		public ResourceIdentifierType Type { get; set; }

		public int RelationTypeId { get; set; }
		public RelationType RelationType { get; set; }

		public string RelatedMetadataScheme { get; set; }
		public string SchemeURI { get; set; }
		public string SchemeType { get; set; }

		public int? ResourceTypeGeneralId { get; set; }
		public ResourceTypeGeneral ResourceTypeGeneral { get; set; }
	}
}
