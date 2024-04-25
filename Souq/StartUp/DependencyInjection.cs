using Souq.Services.Authantication;
using Souq.Services.Categories;
using Souq.Services.Files;
using Souq.Services.Mailing;
using Souq.Services.Manage;
using Souq.Services.Orders;
using Souq.Services.Paymob;
using Souq.Services.Paypal;
using Souq.Services.Products;
using Souq.Services.Reviews;
using Souq.Services.ShoppingCart;
using Souq.Services.UserWishlist;
using Souq.Settings;

namespace Souq.StartUp
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterCustomServises(this IServiceCollection services)
        {
            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<IProductsService, ProductsService>();


            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IWishlistService, WishlistService>();


            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IReviewService, ReviewService>();


            services.AddScoped<IPaymobService, PaymobService>();
            services.AddScoped<IPaypalService, PaypalService>();


            services.AddScoped<IAuthanticationService, AuthanticationService>();
            services.AddScoped<IManageService, ManageService>();


            services.AddSingleton<IMailingService, MailingService>();
            services.AddSingleton<IFileService, FileService>();


            return services;
        }

        public static IServiceCollection RegisterSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailingSetting>(configuration.GetSection(nameof(MailingSetting)));

            services.Configure<GeneralSettings>(configuration.GetSection(nameof(GeneralSettings)));
            services.Configure<OrderSettings>(configuration.GetSection(nameof(OrderSettings)));

            services.Configure<PaymobSettings>(configuration.GetSection(nameof(PaymobSettings)));
            services.Configure<PayPalSettings>(configuration.GetSection(nameof(PayPalSettings)));

            return services;
        }
    }
}

