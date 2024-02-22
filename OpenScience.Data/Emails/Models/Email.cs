using OpenScience.Data.Base.Models;
using OpenScience.Data.Nomenclatures.Models;
using System.Collections.Generic;

namespace OpenScience.Data.Emails.Models
{
	public class Email : Entity
	{
		public int TypeId { get; set; }
		public EmailType Type { get; set; }

		public string Subject { get; set; }

		public string Body { get; set; }

		public IList<EmailAddressee> Addressees { get; set; } = new List<EmailAddressee>();

		private Email()
		{

		}

		public Email(int typeId, string subject, string body)
		{
			this.TypeId = typeId;
			this.Subject = subject;
			this.Body = body;
		}
	}
}
