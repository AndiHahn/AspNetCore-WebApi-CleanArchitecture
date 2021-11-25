using System;

namespace CleanArchitecture.Core
{
    public class UserBankAccount
    {
        private UserBankAccount()
        {
        }

        public UserBankAccount(Guid bankAccountId, Guid userId)
        {
            this.BankAccountId = bankAccountId;
            this.UserId = userId;
        }

        public UserBankAccount(BankAccount bankAccount, User user)
            : this(bankAccount.Id, user.Id)
        {
            if (bankAccount == null)
            {
                throw new ArgumentNullException(nameof(bankAccount));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            this.BankAccount = bankAccount;
            this.User = user;
        }

        public Guid BankAccountId { get; private set; }

        public BankAccount BankAccount { get; private set; }

        public Guid UserId { get; private set; }

        public User User { get; private set; }
    }
}