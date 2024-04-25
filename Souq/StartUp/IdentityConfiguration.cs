using Microsoft.AspNetCore.Identity;
using Souq.Data;
using Souq.Models;

namespace Souq.StartUp
{
    public static class IdentityConfiguration
    {
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {
            // configure identity 
            services.AddIdentity<ApplicationUser, IdentityRole>()
                       .AddEntityFrameworkStores<ApplicationDbContext>();

            //services.ConfigureApplicationCookie(
            //    options =>
            //    {
            //        options.LoginPath = "/Identity/Account/Login";
            //        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            //    });


            return services;
        }
    }
}

