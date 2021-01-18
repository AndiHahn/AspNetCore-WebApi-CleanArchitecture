using System.Collections.Generic;
using CleanArchitecture.Domain.Base;

namespace CleanArchitecture.Domain.Entities
{
    public class AccountEntity : BaseEntity
    {
        public string Name { get; set; }
        public bool IsSharedAccount { get; set; }

        public virtual ICollection<BillEntity> Bills { get; set; }
        public virtual ICollection<UserAccountEntity> UserAccounts { get; set; }
        public virtual ICollection<FixedCostEntity> FixedCosts { get; set; }
        public virtual ICollection<IncomeEntity> Income { get; set; }

        public AccountEntity() { }
        public AccountEntity(string name)
        {
            Name = name;
        }
    }
}