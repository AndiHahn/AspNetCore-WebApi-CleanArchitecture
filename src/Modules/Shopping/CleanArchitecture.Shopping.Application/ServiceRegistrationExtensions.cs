using System.Reflection;
using CleanArchitecture.Shared.Application.Behaviors;
using CleanArchitecture.Shopping.Application.Mapping;
using FluentValidation.AspNetCore;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Shopping.Application
{
    public static class ServiceRegistrationExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IRequestPreProcessor<>), typeof(RequestLogger<>));
            services.AddAutoMapper(config => config.AddProfile<MappingProfile>());
            services.AddFluentValidation(config => config.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
        }
    }
}