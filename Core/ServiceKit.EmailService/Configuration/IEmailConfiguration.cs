using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceKit.EmailService
{
	public interface IEmailConfiguration
	{
		string SystemName { get; }
		string SmtpServer { get; }
		int SmtpPort { get; }
		string SmtpUsername { get; set; }
		string SmtpPassword { get; set; }

		string PopServer { get; }
		int PopPort { get; }
		string PopUsername { get; }
		string PopPassword { get; }
	}
}
