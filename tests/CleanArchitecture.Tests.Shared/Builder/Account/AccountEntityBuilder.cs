using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Tests.Shared.Builder.Account
{
    public class AccountEntityBuilder
    {
        private readonly AccountEntity account;

        public AccountEntityBuilder()
        {
            account = new AccountEntity()
            {
                Name = "Name"
            };
        }

        public AccountEntity Build()
        {
            return account;
        }
    }
}
