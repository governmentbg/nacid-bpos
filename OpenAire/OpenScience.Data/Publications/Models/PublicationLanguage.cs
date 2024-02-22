using OpenScience.Data.Nomenclatures.Models;

namespace OpenScience.Data.Publications.Models
{
	public class PublicationLanguage : PublicationEntity
	{
		public int LanguageId { get; set; }
		public Language Language { get; set; }
	}
}
