using BuyMoreApi.Application.Authentication;
using BuyMoreApi.Application.Monitoring;
using BuyMoreApi.Application.Notifications;
using BuyMoreApi.Application.Payments;
using BuyMoreApi.Application.Payments.Paystack;
using BuyMoreApi.Application.Repositories;
using BuyMoreApi.Application.Services.Implementations;
using BuyMoreApi.Application.Services.Interfaces;
using BuyMoreApi.Application.Storage;
using BuyMoreApi.Infrastructure.Monitoring;
using BuyMoreApi.Infrastructure.Notifications;
using BuyMoreApi.Infrastructure.Payments;
using BuyMoreApi.Infrastructure.Persistence;
using BuyMoreApi.Infrastructure.Persistence.Repositories;
using BuyMoreApi.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuyMoreApi.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            // Register your services and repositories here
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<ICustomerPaymentService, CustomerPaymentService>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<MonitoringOptions>(configuration.GetSection("Monitoring"));
            services.Configure<MailOptions>(configuration.GetSection("Mail"));
            services.Configure<NotificationOptions>(configuration.GetSection("Notification"));
            services.Configure<PaystackOptions>(configuration.GetSection("Paystack"));
            services.Configure<FileStorageOptions>(configuration.GetSection("FileStorage"));

            services.AddSingleton<IMetricsService, MetricsService>();
            services.AddScoped<IMailService, SmtpMailService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IPaymentService, PaystackPaymentService>();
            services.AddScoped<IPaystackWebhookService, PaystackWebhookService>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IBaseRepository, BaseRepository>();

            services.AddScoped<LocalFileStorage>();
            services.AddScoped<AwsS3FileStorage>();
            services.AddScoped<AzureBlobFileStorage>();
            services.AddScoped<FileStorageFactory>();
            services.AddScoped(sp => sp.GetRequiredService<FileStorageFactory>().Create());

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MyConnectionString")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            return services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));
        }        
    }
}
