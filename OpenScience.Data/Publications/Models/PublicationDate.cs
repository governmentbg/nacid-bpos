using OpenScience.Data.Publications.Enums;
using System;

namespace OpenScience.Data.Publications.Models
{
	public class PublicationDate : PublicationEntity
	{
		public DateTime Value { get; set; } 

		public DateType Type { get; set; }

		public string Note { get; set; }
	}
}
