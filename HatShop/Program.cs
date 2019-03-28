using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace HatShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webhost = CreateWebHostBuilder(args).Build();
            if (args.Contains("createroles"))
            {
                IConfiguration configuration = (IConfiguration)webhost.Services.GetService(typeof(IConfiguration));
                string connectionString = configuration.GetConnectionString("DefaultConnection");
                var optionsBuilder = new DbContextOptionsBuilder<Data.ApplicationDbContext>();
                optionsBuilder.UseSqlServer(connectionString);
                HatShop.Data.ApplicationDbContext applicationDbContext = new Data.ApplicationDbContext(optionsBuilder.Options);
                if (!applicationDbContext.Roles.Any(x => x.Name == "Administrator"))
                {
                    applicationDbContext.Roles.Add(new Microsoft.AspNetCore.Identity.IdentityRole("Administrator"));
                }
                if (!applicationDbContext.Roles.Any(x => x.Name == "ProductManager"))
                {
                    applicationDbContext.Roles.Add(new Microsoft.AspNetCore.Identity.IdentityRole("ProductManager"));
                }
                if (!applicationDbContext.Roles.Any(x => x.Name == "MarketingManager"))
                {
                    applicationDbContext.Roles.Add(new Microsoft.AspNetCore.Identity.IdentityRole("MarketingManager"));
                }
                applicationDbContext.SaveChanges();

                Console.WriteLine("Creating roles...");
            }
            else if (args.Contains("assignusertorole"))
            {
                string username = args.SkipWhile(x => x != "assignusertorole").ElementAt(1);
                string rolename = args.SkipWhile(x => x != "assignusertorole").ElementAt(2);

                IConfiguration configuration = (IConfiguration)webhost.Services.GetService(typeof(IConfiguration));
                string connectionString = configuration.GetConnectionString("DefaultConnection");
                var optionsBuilder = new DbContextOptionsBuilder<Data.ApplicationDbContext>();
                optionsBuilder.UseSqlServer(connectionString);
                HatShop.Data.ApplicationDbContext applicationDbContext = new Data.ApplicationDbContext(optionsBuilder.Options);

                var role = applicationDbContext.Roles.Single(x => x.Name == rolename);
                var user = applicationDbContext.Users.Single(x => x.UserName == username);
                applicationDbContext.UserRoles.Add(new Microsoft.AspNetCore.Identity.IdentityUserRole<string> { RoleId = role.Id, UserId = user.Id });
                applicationDbContext.SaveChanges();

            }
            else if (args.Contains("addproducts"))
            {
                IConfiguration configuration = (IConfiguration)webhost.Services.GetService(typeof(IConfiguration));
                string connectionString = configuration.GetConnectionString("DefaultConnection");
                var optionsBuilder = new DbContextOptionsBuilder<Data.ApplicationDbContext>();
                optionsBuilder.UseSqlServer(connectionString);
                HatShop.Data.ApplicationDbContext applicationDbContext = new Data.ApplicationDbContext(optionsBuilder.Options);

                applicationDbContext.Categories.AddRange(Data.MockShopData.categories);
                applicationDbContext.SaveChanges();
            }
            else
            {
                webhost.Run();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
