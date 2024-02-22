using OpenScience.Data.Nomenclatures.Models;

namespace OpenScience.Data.Publications.Models
{
	public class PublicationAudience : PublicationEntity
	{
		public int TypeId { get; set; }
		public AudienceType Type { get; set; }
	}
}
