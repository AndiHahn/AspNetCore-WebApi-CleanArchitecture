using AutoMapper;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.CrudServices.Models.BankAccount
{
    public class BankAccountCreateModel
    {
#nullable enable
        public string? Name { get; set; }
#nullable disable

        public static void ApplyMappingConfiguration(IMapperConfigurationExpression config)
        {
            config.CreateMap<BankAccountCreateModel, BankAccountEntity>()
                .ForMember(m => m.Name, opt => opt.MapFrom(b => b.Name ?? string.Empty));
        }
    }
}
