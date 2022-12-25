using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceKit.EmailService
{
    public interface IEmailSender
    {
        public void Send(EmailMessage emailMessage);
        public void Send(string to, string subject, string content);

        public Task SendAsync(EmailMessage emailMessage);
        public Task SendAsync(string to, string subject, string content);
    }
}
