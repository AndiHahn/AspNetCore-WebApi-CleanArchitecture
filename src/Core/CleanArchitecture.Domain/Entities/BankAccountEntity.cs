using System;
using System.Collections.Generic;
using CleanArchitecture.Domain.Base;

namespace CleanArchitecture.Domain.Entities
{
    public class BankAccountEntity : BaseEntity
    {
        public Guid OwnerId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<BillEntity> Bills { get; set; }
        public virtual ICollection<UserBankAccountEntity> UserBankAccounts { get; set; }

        public BankAccountEntity() { }
        public BankAccountEntity(string name)
        {
            Name = name;
        }
    }
}
