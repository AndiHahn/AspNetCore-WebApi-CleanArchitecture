using System;

namespace CleanArchitecture.Domain.Entities
{
    public class UserAccountEntity
    {
        public Guid AccountId { get; set; }
        public AccountEntity Account { get; set; }
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }

        public UserAccountEntity() { }
        public UserAccountEntity(Guid accountId, Guid userId)
        {
            AccountId = accountId;
            UserId = userId;
        }
    }
}