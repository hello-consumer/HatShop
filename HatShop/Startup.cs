﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HatShop.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using HatShop.Services;
using Microsoft.Extensions.Logging;

namespace HatShop
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

            services.AddTransient(serviceProvider =>
            {
                string connectionString = Configuration.GetConnectionString("ABCConnection");
                System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString);
                return connection;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddTransient((s) =>
            {
                return new FluentEmail.Mailgun.MailgunSender(Configuration.GetValue<string>("MailGun:Domain"), Configuration.GetValue<string>("MailGun:Key"));
            });

            services.AddTransient<SendGrid.ISendGridClient>((s) =>
            {
                return new SendGrid.SendGridClient(Configuration.GetValue<string>("SendGrid:Key"));
            });

            services.AddTransient<IEmailSender>((s) => { return new EmailService(
                s.GetService<FluentEmail.Mailgun.MailgunSender>(),
                s.GetService<SendGrid.ISendGridClient>(),
                s.GetService<ILogger<EmailService>>()
                );
            });

services.AddTransient<Braintree.IBraintreeGateway>((s) =>
{
    return new Braintree.BraintreeGateway(
        Configuration.GetValue<string>("Braintree:Environment"),
        Configuration.GetValue<string>("Braintree:MerchantId"),
        Configuration.GetValue<string>("Braintree:PublicKey"),
        Configuration.GetValue<string>("Braintree:PrivateKey")
    );
});

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<HatUser>((config) => { config.SignIn.RequireConfirmedEmail = true; })
                .AddRoles<IdentityRole>()   //Turn on Role Support
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();





            services.AddTransient(serviceProvider =>
            {
                string connectionString = Configuration.GetConnectionString("ABCConnection");
                System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString);
                return connection;
            });


            services.AddMvc(
            //    config => {
            //    var policy = new AuthorizationPolicyBuilder()
            //     .RequireAuthenticatedUser()
            //     .Build();
            //    config.Filters.Add(new AuthorizeFilter(policy));
            //}
                ).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseAuthentication();
            

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
