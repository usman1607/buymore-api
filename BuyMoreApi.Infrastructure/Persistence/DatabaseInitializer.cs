using Microsoft.EntityFrameworkCore;
using BuyMoreApi.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BuyMoreApi.Application.Utilities;

namespace BuyMoreApi.Infrastructure.Persistence
{
    public static class DatabaseInitializer
    {
        public async static Task MigrateAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();

            try
            {
                logger.LogInformation("Applying database migrations...");
                await context.Database.MigrateAsync();
                logger.LogInformation("Database migrations applied successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error applying database migrations");
                throw;
            }
        }

        public async static Task SeedUserData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();

            try
            {
                // Seed data in order
                if (!context.Users.Any())
                {
                    // Seed initial user data
                    
                    var user =new User 
                    { 
                        FirstName = "Admin", 
                        LastName = "User", 
                        Email = "admin@yopmail.com",
                        EncryptedPassword = Util.EncryptPassword("Admin@123"), // Use the utility method to encrypt the password
                        Role = Domain.Enums.Role.Admin,
                        Address = "Admin Address",
                        PhoneNumber = "1234567890",
                        CreatedBy = "System",
                        CreatedDate = DateTime.UtcNow
                    };
                    
                    context.Users.Add(user);
                    await context.SaveChangesAsync();
                }
                
                logger.LogInformation("Database initialization completed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error initializing database");
                throw;
            }            
        }
    }
}