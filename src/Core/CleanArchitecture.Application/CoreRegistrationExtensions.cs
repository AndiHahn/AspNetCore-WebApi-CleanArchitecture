using System.Reflection;
using CleanArchitecture.Application.BankAccount;
using CleanArchitecture.Application.Bill;
using CleanArchitecture.Application.User;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Application
{
    public static class CoreRegistrationExtensions
    {
        public static void RegisterApplicationCore(this IServiceCollection services)
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