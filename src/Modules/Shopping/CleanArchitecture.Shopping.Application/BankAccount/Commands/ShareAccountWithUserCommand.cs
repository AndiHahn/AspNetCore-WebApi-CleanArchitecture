using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Core.Models.Result;
using CleanArchitecture.Shopping.Core.Interfaces;
using MediatR;

namespace CleanArchitecture.Shopping.Application.BankAccount.Commands
{
    public class ShareAccountWithUserCommand : IRequest<Result>
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

    internal class ShareAccountWithUserCommandHandler : IRequestHandler<ShareAccountWithUserCommand, Result>
    {
        private readonly IUserRepository userRepository;
        private readonly IBankAccountRepository bankAccountRepository;

        public ShareAccountWithUserCommandHandler(
            IUserRepository userRepository,
            IBankAccountRepository bankAccountRepository)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.bankAccountRepository = bankAccountRepository ?? throw new ArgumentNullException(nameof(bankAccountRepository));
        }

        public async Task<Result> Handle(ShareAccountWithUserCommand request, CancellationToken cancellationToken)
        {
            var account = await this.bankAccountRepository.GetByIdWithUsersAsync(request.AccountId, cancellationToken);
            if (account == null)
            {
                return Result.NotFound($"Account with id {request.AccountId} not found.");
            }

            if (!account.IsOwner(request.CurrentUserId))
            {
                return Result.Forbidden($"Current user does not have access to account {request.AccountId}");
            }

            var user = await this.userRepository.GetByIdAsync(request.ShareWithUserId);
            if (user == null)
            {
                return Result.NotFound($"User with id {request.ShareWithUserId} not found.");
            }

            account.ShareWithUser(user);

            await this.bankAccountRepository.UpdateAsync(account, cancellationToken);

            return Result.Success();
        }
    }
}
