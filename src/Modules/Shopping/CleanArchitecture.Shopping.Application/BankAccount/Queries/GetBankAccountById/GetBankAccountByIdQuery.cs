using System;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;

namespace CleanArchitecture.Shopping.Application.BankAccount.Queries.GetBankAccountById
{
    public class GetBankAccountByIdQuery : IQuery<Result<BankAccountDto>>
    {
        public GetBankAccountByIdQuery(Guid currentUserId, Guid id)
        {
            this.CurrentUserId = currentUserId;
            this.Id = id;
        }

        public Guid CurrentUserId { get; }

        public Guid Id { get; set; }
    }
}
