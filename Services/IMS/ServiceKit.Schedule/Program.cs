using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceKit.EmailService;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceKit.Model.WBS;

namespace ServiceKit.Schedule
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;

                    // Add the required Quartz.NET services
                    services.AddQuartz(q =>
                    {
                        // Use a Scoped container to create jobs. I'll touch on this later
                        q.UseMicrosoftDependencyInjectionJobFactory();

                        var jobKey = new JobKey("GeoSplit.SUP");

                        q.AddJob<Jobs.Pulse>(opts => opts.WithIdentity(jobKey));

                        q.AddTrigger(opts => opts
                            .ForJob(jobKey)
                            .WithIdentity("GeoSplit.SUP.QuartzTrigger")
                            .WithCronSchedule("0 0/5 * * * ?"));
                    });

                    services.AddQuartzHostedService(
                        q => q.WaitForJobsToComplete = true);


                    services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlServer(
                            configuration.GetConnectionString("DefaultConnection")));

                    services.AddSingleton<IEmailConfiguration>(configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
                    services.AddTransient<IEmailSender, EmailSender>();
                });
    }
}
