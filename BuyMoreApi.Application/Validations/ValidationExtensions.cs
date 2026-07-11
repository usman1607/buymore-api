using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;

namespace BuyMoreApi.Application.Validations
{
    public static class ValidationExtensions
    {
        public static IServiceCollection AddApplicationValidation(this IServiceCollection services)
        {
            // Automatically registers all validators in the assembly where Program is located
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}