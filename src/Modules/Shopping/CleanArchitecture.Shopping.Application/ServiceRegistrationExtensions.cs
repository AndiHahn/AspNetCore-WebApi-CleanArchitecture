using System.Reflection;
using CleanArchitecture.Shopping.Application.Mapping;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Shopping.Application
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