using OpenScience.Data.Publications.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenScience.Data.Publications.Models
{
	public class PublicationCreator : PublicationEntity
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public string Language { get; set; }

		// Always Personal
		public NameType NameType { get; set; } = NameType.Personal;

		[NotMapped]
		public string DisplayName
		{
			get
			{
				if (this.NameType == NameType.Personal)
				{
					if (!string.IsNullOrWhiteSpace(this.FirstName))
					{
						if (!string.IsNullOrWhiteSpace(this.LastName))
						{
							return $"{this.LastName}, {this.FirstName}";
						}
						else
						{
							return this.FirstName;
						}
					}
					else if (!string.IsNullOrWhiteSpace(this.LastName))
					{
						return this.LastName;
					}
				}
				else if (this.NameType == NameType.Organizational)
				{
					return this.FirstName;
				}

				return null;
			}
		}

		public ICollection<PublicationCreatorIdentifier> Identifiers { get; set; } = new List<PublicationCreatorIdentifier>();
		public ICollection<PublicationCreatorAffiliation> Affiliations { get; set; } = new List<PublicationCreatorAffiliation>();
	}
}
