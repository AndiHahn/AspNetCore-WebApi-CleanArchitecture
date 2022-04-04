using System;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;

namespace CleanArchitecture.Shopping.Application.BankAccount.Commands.ShareAccountWithUser
{
    public class ShareAccountWithUserCommand : ICommand<Result>
    {
        public ShareAccountWithUserCommand(Guid accountId, Guid shareWithUserId, Guid currentUserId)
        {
            AccountId = accountId;
            ShareWithUserId = shareWithUserId;
            CurrentUserId = currentUserId;
        }

        public Guid CurrentUserId { get; }

        public Guid AccountId { get; }

        public Guid ShareWithUserId { get; }
    }
}
