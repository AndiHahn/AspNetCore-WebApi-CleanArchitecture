using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Tests.Shared.Builder.Account
{
    public class AccountEntityBuilder
    {
        private readonly BankAccountEntity bankAccount;

        public AccountEntityBuilder()
        {
            bankAccount = new BankAccountEntity()
            {
                Name = "Name"
            };
        }

        public BankAccountEntity Build()
        {
            return bankAccount;
        }
    }
}
