using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceKit.EmailService
{
	public class EmailMessage
	{
		public EmailMessage()
		{
			ToAddresses = new List<EmailAddress>();
			FromAddresses = new List<EmailAddress>();
		}

		public EmailMessage(EmailAddress to, string subject, string content) : this()
		{
			ToAddresses.Add(to);
			Subject = subject;
			Content = content;
		}

		public List<EmailAddress> ToAddresses { get; set; }
		public List<EmailAddress> FromAddresses { get; set; }
		public string Subject { get; set; }
		public string Content { get; set; }
	}
}
