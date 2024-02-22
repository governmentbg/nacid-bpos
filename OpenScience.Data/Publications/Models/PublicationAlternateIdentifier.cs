using OpenScience.Data.Nomenclatures.Models;

namespace OpenScience.Data.Publications.Models
{
	public class PublicationAlternateIdentifier : PublicationEntity
	{
		public string Value { get; set; }

		public int TypeId { get; set; }
		public ResourceIdentifierType Type { get; set; }
	}
}
