using System.Reflection;
using AutoMapper;
using CleanArchitecture.Application.CrudServices;
using CleanArchitecture.Core.Interfaces.CrudServices;
using CleanArchitecture.Core.Models.Domain.Account;
using CleanArchitecture.Core.Models.Domain.Bill;
using CleanArchitecture.Core.Models.Domain.BillCategory;
using CleanArchitecture.Core.Models.Domain.FixedCost;
using CleanArchitecture.Core.Models.Domain.Income;
using CleanArchitecture.Core.Models.Domain.User;
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
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IIncomeService, IncomeService>();
            services.AddScoped<IFixedCostService, FixedCostService>();
            services.AddScoped<IBillCategoryService, BillCategoryService>();
        }

        private static void ConfigureModelMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(config =>
            {
                AccountModel.ApplyMappingConfiguration(config);
                BillModel.ApplyMappingConfiguration(config);
                BillCreateModel.ApplyMappingConfiguration(config);
                BillCategoryModel.ApplyMappingConfiguration(config);
                BillCategoryCreateModel.ApplyMappingConfiguration(config);
                FixedCostModel.ApplyMappingConfiguration(config);
                FixedCostCreateModel.ApplyMappingConfiguration(config);
                IncomeModel.ApplyMappingConfiguration(config);
                IncomeCreateModel.ApplyMappingConfiguration(config);
                UserModel.ApplyMappingConfiguration(config);
            });
        }
    }
}