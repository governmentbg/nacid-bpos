using NacidRas.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace NacidRas.Ras
{
	public class Person : EntityVersion
	{
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }

		public string FirstNameAlt { get; set; }
		public string MiddleNameAlt { get; set; }
		public string LastNameAlt { get; set; }

		// FirstName + MiddleName + LastName + Uin
		public string Name { get; set; }

		public DateTime? BirthDate { get; set; }

		public PersonType? Type { get; set; }
		public int? CountryId { get; set; }
		[Skip]
		public Country Country { get; set; }
		public string Uin { get; set; }
		public bool isRasOfficial { get; set; }
		public int _ExternalId { get; set; }

		public List<PersonIdn> PersonIdns { get; set; }

		public Person()
		{
			PersonIdns = new List<PersonIdn>();
		}

		public void HideUin()
		{
			if (Uin == null) return;

			if (Uin.Length > 6)
			{
				var positionsToReplace = Uin.Length - 6;
				var sb = new StringBuilder(Uin);
				sb.Remove(6, positionsToReplace);
				sb.Insert(6, new String('*', positionsToReplace));
				Uin = sb.ToString();
			}
			else
			{
				Uin = "******";
			}
		}

		public string GetHideUin()
		{
			if ((string.IsNullOrWhiteSpace(Uin))) return "";

			if (Uin.Length > 6)
			{
				var positionsToReplace = Uin.Length - 6;
				var sb = new StringBuilder(Uin);
				sb.Remove(6, positionsToReplace);
				sb.Insert(6, new String('*', positionsToReplace));
				Uin = sb.ToString();
			}
			else
			{
				Uin = "******";
			}

			return Uin;
		}

		public void Normalize()
		{
			Uin = Uin?.Trim();
			FixNamesToPascalCase();
		}

		public void FixNamesToPascalCase()
		{
			FirstName = !string.IsNullOrWhiteSpace(FirstName) ? ToPascalCase(FirstName).Trim() : FirstName;
			MiddleName = !string.IsNullOrWhiteSpace(MiddleName) ? ToPascalCase(MiddleName).Trim() : MiddleName;
			LastName = !string.IsNullOrWhiteSpace(LastName) ? ToPascalCase(LastName).Trim() : LastName;
			FirstNameAlt = !string.IsNullOrWhiteSpace(FirstNameAlt) ? ToPascalCase(FirstNameAlt).Trim() : FirstNameAlt;
			MiddleNameAlt = !string.IsNullOrWhiteSpace(MiddleNameAlt) ? ToPascalCase(MiddleNameAlt).Trim() : MiddleNameAlt;
			LastNameAlt = !string.IsNullOrWhiteSpace(LastNameAlt) ? ToPascalCase(LastNameAlt).Trim() : LastNameAlt;

			Name = FirstName + ' ' + MiddleName + ' ' + LastName;
		}

		private string ToPascalCase(string the_string)
		{
			// If there are 0 or 1 characters, just return the string.
			if (the_string == null) return the_string;
			if (the_string.Length < 2) return the_string.ToUpper();

			// Start with the first character.
			string result = the_string.Substring(0, 1).ToUpper();

			// Add the remaining characters.
			for (int i = 1; i < the_string.Length; i++)
			{
				if (the_string[i - 1] == '-' || the_string[i - 1] == '(' || the_string[i - 1] == ' ')
				{
					result += (char.ToUpper(the_string[i]));
				}
				else
				{
					result += (char.ToLower(the_string[i]));
				}
			}

			return result;
		}
	}

	public enum PersonType
	{
		Bulgarian = 1,
		Foreigner = 2
	}
}
