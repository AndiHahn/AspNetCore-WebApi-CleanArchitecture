using CleanArchitecture.BudgetPlan.Application.FixedCost;
using CleanArchitecture.BudgetPlan.Application.Income;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CleanArchitecture.BudgetPlan.Application
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
                IncomeDto.ApplyMappingConfiguration(config);
                FixedCostDto.ApplyMappingConfiguration(config);
            });
        }
    }
}
