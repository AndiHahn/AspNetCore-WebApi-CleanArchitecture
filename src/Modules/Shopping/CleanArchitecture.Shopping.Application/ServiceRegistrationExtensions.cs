using System.Reflection;
using CleanArchitecture.Shopping.Application.BankAccount;
using CleanArchitecture.Shopping.Application.Bill;
using CleanArchitecture.Shopping.Application.User;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Shopping.Application
{
    public static class ServiceRegistrationExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.ConfigureDtoMapper();
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }

        private static void ConfigureDtoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(config =>
            {
                BankAccountDto.ApplyMappingConfiguration(config);
                UserDto.ApplyMappingConfiguration(config);
                BillDto.ApplyMappingConfiguration(config);
            });
        }
    }
}