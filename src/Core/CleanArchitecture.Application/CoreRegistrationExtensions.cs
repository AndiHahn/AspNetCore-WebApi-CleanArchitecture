using System.Reflection;
using CleanArchitecture.Application.CrudServices;
using CleanArchitecture.Application.CrudServices.Models.BankAccount;
using CleanArchitecture.Application.CrudServices.Models.Bill;
using CleanArchitecture.Application.CrudServices.Models.User;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Application
{
    public static class CoreRegistrationExtensions
    {
        public static void RegisterCore(this IServiceCollection services)
        {
            services.RegisterServices();
            services.ConfigureModelMapper();
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }

        private static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBillService, BillService>();
            services.AddScoped<IBankAccountService, BankAccountService>();
        }

        private static void ConfigureModelMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(config =>
            {
                BankAccountModel.ApplyMappingConfiguration(config);
                BillModel.ApplyMappingConfiguration(config);
                BillCreateModel.ApplyMappingConfiguration(config);
                UserModel.ApplyMappingConfiguration(config);
                BankAccountCreateModel.ApplyMappingConfiguration(config);
            });
        }
    }
}