using System;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;

namespace CleanArchitecture.Shopping.Application.BankAccount.Commands.CreateBankAccount
{
    public class CreateBankAccountCommand : ICommand<Result<BankAccountDto>>
    {
        public CreateBankAccountCommand(Guid currentUserId, string name)
        {
            this.CurrentUserId = currentUserId;
            this.Name = name;
        }

        public Guid CurrentUserId { get; }

        public string Name { get; }
    }
}
