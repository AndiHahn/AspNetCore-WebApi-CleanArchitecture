using System;
using System.Collections.Generic;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;

namespace CleanArchitecture.Shopping.Application.BankAccount.Queries.GetBankAccounts
{
    public class GetBankAccountsQuery : IQuery<Result<IEnumerable<BankAccountDto>>>
    {
        public GetBankAccountsQuery(Guid currentUserId)
        {
            CurrentUserId = currentUserId;
        }

        public Guid CurrentUserId { get; }
    }
}
