using System;

namespace CleanArchitecture.Domain.Entities
{
    public class UserBankAccountEntity
    {
        public Guid BankAccountId { get; set; }
        public BankAccountEntity BankAccount { get; set; }
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }

        public UserBankAccountEntity() { }
        public UserBankAccountEntity(Guid bankAccountId, Guid userId)
        {
            BankAccountId = bankAccountId;
            UserId = userId;
        }
    }
}