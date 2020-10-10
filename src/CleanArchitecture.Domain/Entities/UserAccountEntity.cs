namespace CleanArchitecture.Domain.Entities
{
    public class UserAccountEntity
    {
        public int AccountId { get; set; }
        public AccountEntity Account { get; set; }
        public int UserId { get; set; }
        public UserEntity User { get; set; }

        public UserAccountEntity() { }
        public UserAccountEntity(int accountId, int userId)
        {
            AccountId = accountId;
            UserId = userId;
        }
    }
}