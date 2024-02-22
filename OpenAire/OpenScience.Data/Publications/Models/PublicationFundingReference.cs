using OpenScience.Data.Nomenclatures.Models;
using OpenScience.Data.Publications.Enums;

namespace OpenScience.Data.Publications.Models
{
	public class PublicationFundingReference : PublicationEntity
	{
		public string Name { get; set; }

		public string Identifier { get; set; }
		public int? SchemeId { get; set; }
		public OrganizationalIdentifierScheme Scheme { get; set; }

		public string AwardNumber { get; set; }
		public string AwardURI { get; set; }
		public string AwardTitle { get; set; }
	}
}
