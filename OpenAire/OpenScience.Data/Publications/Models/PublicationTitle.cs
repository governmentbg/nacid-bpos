using OpenScience.Data.Nomenclatures.Models;

namespace OpenScience.Data.Publications.Models
{
	public class PublicationTitle : PublicationEntity
	{
		public string Value { get; set; }
		
		public int? TypeId { get; set; }
		public TitleType Type { get; set; }

		public int? LanguageId { get; set; }
		public Language Language { get; set; }
	}
}
