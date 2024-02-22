using OpenScience.Data.Base.Models;
using OpenScience.Data.Emails.Enums;
using System;

namespace OpenScience.Data.Emails.Models
{
	public class EmailAddressee : Entity
	{
		public int EmailId { get; set; }
		public Email Email { get; set; }

		public EmailAddresseeType AddresseeType { get; set; }

		public string Address { get; set; }

		public DateTime SentDate { get; set; }

		public EmailStatus Status { get; set; }

		private EmailAddressee()
		{

		}

		public EmailAddressee(EmailAddresseeType type, string address)
		{
			this.AddresseeType = type;
			this.Address = address;
			this.SentDate = DateTime.UtcNow;
			this.Status = EmailStatus.Pending;
		}
	}
}
