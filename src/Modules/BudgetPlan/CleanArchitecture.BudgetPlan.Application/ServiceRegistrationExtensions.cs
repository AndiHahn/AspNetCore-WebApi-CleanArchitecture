using CleanArchitecture.BudgetPlan.Application.Mapping;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CleanArchitecture.BudgetPlan.Application
{
    public static class ServiceRegistrationExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(config => config.AddProfile<MappingProfile>());
        }
    }
}
