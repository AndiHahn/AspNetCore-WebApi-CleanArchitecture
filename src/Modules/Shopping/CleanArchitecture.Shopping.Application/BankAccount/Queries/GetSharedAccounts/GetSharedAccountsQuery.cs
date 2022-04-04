using System;
using System.Collections.Generic;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;

namespace CleanArchitecture.Shopping.Application.BankAccount.Queries.GetSharedAccounts
{
    public class GetSharedAccountsQuery : IQuery<Result<IEnumerable<BankAccountDto>>>
    {
        public GetSharedAccountsQuery(Guid currentUserId)
        {
            CurrentUserId = currentUserId;
        }

        public Guid CurrentUserId { get; }
    }
}
