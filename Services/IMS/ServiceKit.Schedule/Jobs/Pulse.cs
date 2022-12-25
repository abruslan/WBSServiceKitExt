using Microsoft.Extensions.Logging;
using Quartz;
using ServiceKit.EmailService;
using ServiceKit.Model.WBS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceKit.Schedule.Jobs
{
    public class Pulse : IJob
    {
        private readonly ILogger<Pulse> _logger;
        private readonly IEmailSender _emailSender;
        private readonly AppDbContext _context;

        public Pulse(ILogger<Pulse> logger, IEmailSender emailSender, AppDbContext context)
        {
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Notification job started");
            _emailSender.Send("ab.ruslan@gmail.com","Пульс","Пульс");

            _logger.LogInformation("Notification job completed");
            return Task.CompletedTask;
        }

    }
}
