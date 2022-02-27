using AutoMapper;
using System.Reflection;

namespace CleanArchitecture.Shared.Application.Mapping
{
    public abstract class BaseMappingProfile : Profile
    {
        protected BaseMappingProfile(Assembly assembly)
        {
            assembly
                .GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMappableDto<>)))
                .ToList()
                .ForEach(t =>
                {
                    var dtoInstance = Activator.CreateInstance(t);
                    var methodInfo = t.GetMethod(nameof(IMappableDto<object>.MappingConfig));
                    if (methodInfo is null)
                    {
                        var mappableInterface = t.GetInterfaces().First(i => i.Name == typeof(IMappableDto<>).Name);
                        var genericType = mappableInterface.GetGenericArguments()[0];
                        var mappableType = typeof(IMappableDto<>).MakeGenericType(genericType);
                        methodInfo = mappableType.GetMethod(nameof(IMappableDto<object>.MappingConfig));
                    }
                    
                    methodInfo?.Invoke(dtoInstance, new object[] { this });
                });
        }
    }
}
