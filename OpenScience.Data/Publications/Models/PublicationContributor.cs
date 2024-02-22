using OpenScience.Data.Nomenclatures.Models;
using OpenScience.Data.Publications.Enums;
using System.Collections.Generic;

namespace OpenScience.Data.Publications.Models
{
	public class PublicationContributor : PublicationEntity
	{
		public int? TypeId { get; set; }
		public ContributorType Type { get; set; }

		public NameType NameType { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }

		public string InstitutionAffiliationName { get; set; }

		public ICollection<PublicationContributorIdentifier> Identifiers { get; set; } = new List<PublicationContributorIdentifier>();
	}
}
