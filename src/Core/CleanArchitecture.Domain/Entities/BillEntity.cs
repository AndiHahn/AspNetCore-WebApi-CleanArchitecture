using System;
using System.Collections.Generic;
using CleanArchitecture.Domain.Base;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Entities
{
    public class BillEntity : BaseEntity, IVersionableEntity
    {
        public Guid BankAccountId { get; set; }
        public Guid CreatedByUserId { get; set; }
        public string ShopName { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public Category Category { get; set; }
        public byte[] Version { get; set; }

        public virtual BankAccountEntity BankAccount { get; set; }
        public virtual ICollection<UserBillEntity> UserBills { get; set; }
    }
}
