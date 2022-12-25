using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.EmailService
{
	public class EmailSender : IEmailSender
	{
		private readonly IEmailConfiguration _emailConfiguration;

		public EmailSender(IEmailConfiguration emailConfiguration)
		{
			_emailConfiguration = emailConfiguration;
		}

		public void Send(EmailMessage emailMessage)
		{
			if (emailMessage.ToAddresses == null || emailMessage.ToAddresses.Count() == 0)
				return;
			var message = new MimeMessage();
			message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
			message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
			if (message.From.Count == 0)
				message.From.Add(new MailboxAddress(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpUsername));

			message.Subject = emailMessage.Subject;
			//We will say we are sending HTML. But there are options for plaintext etc. 
			message.Body = new TextPart(TextFormat.Html)
			{
				Text = GetMessageBody(emailMessage)
			};

			//Be careful that the SmtpClient class is the one from Mailkit not the framework!
			using (var emailClient = new SmtpClient())
			{
				AuthenticateEmailClient(emailClient);
				emailClient.Send(message);

				emailClient.Disconnect(true);
			}
		}
		public async Task SendAsync(EmailMessage emailMessage)
		{
			if (emailMessage.ToAddresses == null || emailMessage.ToAddresses.Count() == 0)
				return;
			var message = new MimeMessage();
			message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
			message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
			if (message.From.Count == 0)
				message.From.Add(new MailboxAddress(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpUsername ));

			message.Subject = emailMessage.Subject;
			//We will say we are sending HTML. But there are options for plaintext etc. 
			message.Body = new TextPart(TextFormat.Html)
			{
				Text = GetMessageBody(emailMessage)
			};

			//Be careful that the SmtpClient class is the one from Mailkit not the framework!
			using (var emailClient = new SmtpClient())
			{
				AuthenticateEmailClient(emailClient);
				await emailClient.SendAsync(message);

				emailClient.Disconnect(true);
			}
		}

        private void AuthenticateEmailClient(SmtpClient emailClient)
        {
			//The last parameter here is to use SSL (Which you should!)
			if (_emailConfiguration.SmtpPort == 587)
				emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
			else
				emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, true);

			//Remove any OAuth functionality as we won't be using it. 
			emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

			emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);
		}

		public async Task SendAsync(string to, string subject, string content)
        {
			await SendAsync(new EmailMessage(new EmailAddress() { Name = to, Address = to }, subject, content));
        }

		public void Send(string to, string subject, string content)
		{
			Send(new EmailMessage(new EmailAddress() { Name = to, Address = to }, subject, content));
		}

		private string GetMessageBody(EmailMessage emailMessage)
		{
			var SystemName = _emailConfiguration.SystemName;
			var IsError = false;
			var template = Properties.Resources.Email
				.Replace("{email_subject}", emailMessage.Subject)
				.Replace("{email_title}", SystemName)
				.Replace("{source}", SystemName)
				.Replace("{created}", DateTime.Now.ToString())
				.Replace("{detail_info}", emailMessage.Content)
				.Replace("{detail_color}", IsError ? "#FF7070" : "#000000")
				.Replace("{footer_info}", $"{DateTime.Now}");
			return template;
		}

	}
}
