using System;
using AutoMapper;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Core.Models.Domain.Account
{
    public class AccountModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public static void ApplyMappingConfiguration(IMapperConfigurationExpression config)
        {
            config.CreateMap<AccountEntity, AccountModel>();
        }
    }
}
