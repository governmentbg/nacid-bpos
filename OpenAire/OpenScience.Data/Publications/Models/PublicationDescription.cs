using OpenScience.Data.Nomenclatures.Models;

namespace OpenScience.Data.Publications.Models
{
	public class PublicationDescription : PublicationEntity
	{
		public string Value { get; set; }

		public int? LanguageId { get; set; }
		public Language Language { get; set; }
	}
}
