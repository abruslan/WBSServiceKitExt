using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServiceKit.EmailService;
using ServiceKit.Model.WBS;
using ServiceKit.WBS.Configuration;
using ServiceKit.IdentityService;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http;
using ServiceKit.ExternalSystem.Common;
using ServiceKit.WBS.Common;
using Microsoft.AspNetCore.Server.IISIntegration;
using JT.AspNetBaseRoleProvider;

namespace ServiceKit.WBS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<UserService>();

            services.AddRazorPages();

            services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddSingleton<IEmailConfiguration>(Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddSingleton<IWebServiceConfiguration>(Configuration.GetSection("OneCConfiguration").Get<OneCConfiguration>());

            services.AddAuthentication(IISDefaults.AuthenticationScheme);

            #region Add User Roles
            //MapToolRoleProvider is a Singleton, therfore the repository that it use to read userRoles from needs to also be Singleton
            //Below we use the AppDbContext inside a scope and return it to a Singleton
            //TODO: UserRoles are only read when application start... Might need a solution for this?
            services.AddSingleton<IUserRoleRepositoryLight>(sp =>
            {
                using (var scope = sp.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<AppDbContext>();
                    var lastItem = dbContext.UserRoles.ToList();
                    return new UserRoleRepositoryLight(lastItem);
                }
            });
            services.AddJTRoleAuthorization<RoleProvider>();
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
