using System.Reflection;
using AutoMapper;
using CleanArchitecture.Core.CrudServices;
using CleanArchitecture.Core.Interfaces.Services.Account;
using CleanArchitecture.Core.Interfaces.Services.Account.Models;
using CleanArchitecture.Core.Interfaces.Services.Bill;
using CleanArchitecture.Core.Interfaces.Services.Bill.Models;
using CleanArchitecture.Core.Interfaces.Services.BillCategory;
using CleanArchitecture.Core.Interfaces.Services.BillCategory.Models;
using CleanArchitecture.Core.Interfaces.Services.FixedCost;
using CleanArchitecture.Core.Interfaces.Services.FixedCost.Models;
using CleanArchitecture.Core.Interfaces.Services.Income;
using CleanArchitecture.Core.Interfaces.Services.Income.Models;
using CleanArchitecture.Core.Interfaces.Services.User;
using CleanArchitecture.Core.Interfaces.Services.User.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Core
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