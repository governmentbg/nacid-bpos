using HandlebarsDotNet;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using OpenScience.Common.Configuration;
using OpenScience.Data;
using OpenScience.Data.Emails.Enums;
using OpenScience.Data.Emails.Models;
using OpenScience.Data.Nomenclatures.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenScience.Services.Emails
{
	public class EmailService
	{
		private readonly AppDbContext context;

		public EmailService(AppDbContext context)
		{
			this.context = context;
		}

		public async Task<Email> ComposeEmailAsync(string alias, object templateData, params string[] recipients)
		{
			EmailType type = await context.EmailTypes
				.AsNoTracking()
				.SingleOrDefaultAsync(e => e.Alias == alias);

			string body = type.Body;
			if(templateData != null)
			{
				var template = Handlebars.Compile(type.Body);
				body = template(templateData);
			}
			
			var email = new Email(type.Id, type.Subject, body);

			EmailAddresseeType emailAddresseeType;
			for (var i = 0; i <= recipients.Length - 1; i++)
			{
				if(i == 0)
				{
					emailAddresseeType = EmailAddresseeType.To;
				}
				else
				{
					emailAddresseeType = EmailAddresseeType.Bcc;
				}

				var addressee = new EmailAddressee(emailAddresseeType, recipients[i].Trim());
				email.Addressees.Add(addressee);
			}

			return email;
		}

		public bool SendEmail(Email email, string senderName, string senderMail, ISmtpConfiguration smtpConfiguration)
		{
			var pendingAddressees = email.Addressees
				.Where(e => e.Status == EmailStatus.Pending)
				.ToList();

			var mimeMessage = TryComposeMimeMessage(senderName, senderMail, pendingAddressees, email.Subject, email.Body);
			if (mimeMessage == null)
			{
				return false;
			}

			using (var smtpClient = new SmtpClient())
			{
				try
				{
					smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

					smtpClient.Connect(smtpConfiguration.SmtpHost, smtpConfiguration.SmtpPort, smtpConfiguration.SmtpUseSsl);

					if (smtpConfiguration.SmtpShouldAuthenticate)
					{
						smtpClient.Authenticate(smtpConfiguration.SmtpUsername, smtpConfiguration.SmtpPassword);
					}

					smtpClient.Send(mimeMessage);

					foreach (var addressee in pendingAddressees)
					{
						addressee.Status = EmailStatus.Sent;
					}

					return true;
				}
				catch (Exception)
				{
					return false;
				}
				finally
				{
					smtpClient.Disconnect(true);
				}
			}
		}

		private MimeMessage TryComposeMimeMessage(string senderName, string senderMail, List<EmailAddressee> addresses, string subject, string body)
		{
			try
			{
				var mimeMessage = new MimeMessage();
				mimeMessage.From.Add(new MailboxAddress(senderName, senderMail));

				var toAddresses = addresses.Select(e => new MailboxAddress(e.Address)).ToList();
				if (addresses.Count == 1)
				{
					mimeMessage.To.Add(toAddresses[0]);
				}
				else
				{
					mimeMessage.Bcc.AddRange(toAddresses);
				}

				mimeMessage.Subject = subject;
				mimeMessage.Body = new TextPart(TextFormat.Html) {
					Text = body
				};

				return mimeMessage;
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
