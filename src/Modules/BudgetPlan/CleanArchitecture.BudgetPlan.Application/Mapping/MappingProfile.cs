using System.Reflection;
using CleanArchitecture.Shared.Application.Mapping;

namespace CleanArchitecture.BudgetPlan.Application.Mapping
{
    public class MappingProfile : BaseMappingProfile
    {
        public MappingProfile() : base(Assembly.GetExecutingAssembly())
        {
        }
    }
}
