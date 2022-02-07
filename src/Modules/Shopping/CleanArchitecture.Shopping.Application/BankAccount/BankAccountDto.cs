using System;
using AutoMapper;

namespace CleanArchitecture.Shopping.Application.BankAccount
{
    public class BankAccountDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public static void ApplyMappingConfiguration(IMapperConfigurationExpression config)
        {
            config.CreateMap<Core.BankAccount.BankAccount, BankAccountDto>();
        }
    }
}
