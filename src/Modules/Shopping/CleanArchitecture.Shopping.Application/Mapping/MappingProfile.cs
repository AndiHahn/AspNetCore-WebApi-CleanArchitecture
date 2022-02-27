using CleanArchitecture.Shared.Application.Mapping;
using System.Reflection;

namespace CleanArchitecture.Shopping.Application.Mapping
{
    public class MappingProfile : BaseMappingProfile
    {
        public MappingProfile() : base(Assembly.GetExecutingAssembly())
        {
        }
    }
}
