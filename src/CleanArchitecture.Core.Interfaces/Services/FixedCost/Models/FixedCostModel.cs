using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Extensions;

namespace CleanArchitecture.Core.Interfaces.Services.FixedCost.Models
{
    public class FixedCostModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public Duration Duration { get; set; }
        public CostCategory CostCategory { get; set; }
        public Guid AccountId { get; set; }

        public static void ApplyMappingConfiguration(IMapperConfigurationExpression config)
        {
            config.CreateMap<FixedCostEntity, FixedCostModel>();
        }

        public double ToSharedValue(
            IList<AccountEntity> allAccounts,
            IList<Guid> selectedAccountIds)
        {
            AccountEntity sharedAccount = allAccounts.FirstOrDefault(a => a.IsSharedAccount);

            //if no shared account -> return full value
            if (sharedAccount != null && AccountId != sharedAccount.Id)
            {
                return Value.ToMonthlyValue(Duration);
            }

            //if all accounts are selected -> return full value
            if (allAccounts.Count == selectedAccountIds.Count)
            {
                return Value.ToMonthlyValue(Duration);
            }

            //if only shared account selected -> return full value
            if (selectedAccountIds.Count == 1 && selectedAccountIds.First() == sharedAccount.Id)
            {
                return Value.ToMonthlyValue(Duration);
            }

            //if any account and shared account selected -> return half value
            double sharedValue = Value.ToMonthlyValue(Duration) / 2;
            return sharedValue.ToTwoDecimals();
        }
    }
}