using System;
using CleanArchitecture.Domain.Base;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Entities
{
    public class FixedCostEntity : BaseEntity
    {
        public Guid AccountId { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public Duration Duration { get; set; }
        public CostCategory CostCategory { get; set; }

        public AccountEntity Account { get; set; }
    }
}