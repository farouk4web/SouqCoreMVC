using Microsoft.EntityFrameworkCore;
using Souq.Data;

namespace Souq.StartUp
{
    public static class DataBaseConfiguration
    {
        public static IServiceCollection RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                    );

            return services;
        }
    }
}

