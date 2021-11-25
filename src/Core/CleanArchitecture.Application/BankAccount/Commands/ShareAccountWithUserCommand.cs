using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.BankAccount
{
    public class ShareAccountWithUserCommand : IRequest
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

    internal class ShareAccountWithUserCommandHandler : IRequestHandler<ShareAccountWithUserCommand>
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

        public async Task<Unit> Handle(ShareAccountWithUserCommand request, CancellationToken cancellationToken)
        {
            var account = await this.bankAccountRepository.GetByIdWithUsersAsync(request.AccountId, cancellationToken);
            if (account == null)
            {
                throw new NotFoundException($"Account with id {request.AccountId} not found.");
            }

            if (!account.IsOwner(request.CurrentUserId))
            {
                throw new ForbiddenException($"Current user does not have access to account {request.AccountId}");
            }

            var user = await this.userRepository.GetByIdAsync(request.ShareWithUserId);
            if (user == null)
            {
                throw new NotFoundException($"User with id {request.ShareWithUserId} not found.");
            }

            account.ShareWithUser(user);

            await this.bankAccountRepository.UpdateAsync(account, cancellationToken);

            return Unit.Value;
        }
    }
}
