using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Validations;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Exceptions;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Interfaces.Repositories;
using MediatR;

namespace CleanArchitecture.Application.UseCases.Collaboration.Commands
{
    public class ShareAccountWithUserCommand : IRequest
    {
        public Guid AccountId { get; }
        public Guid ShareWithUserId { get; }
        public Guid CurrentUserId { get; }

        public ShareAccountWithUserCommand(Guid accountId, Guid shareWithUserId, Guid currentUserId)
        {
            AccountId = accountId;
            ShareWithUserId = shareWithUserId;
            CurrentUserId = currentUserId;
        }
    }

    public class ShareAccountWithUserCommandHandler : IRequestHandler<ShareAccountWithUserCommand>
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
            var account = await EnsureCurrentUserHasAccessToAccountAsync(request.AccountId, request.CurrentUserId);
            var user = (await userRepository.GetByIdAsync(request.ShareWithUserId))
                .AssertEntityFound(request.ShareWithUserId);

            if (account.UserBankAccounts.Any(ua => ua.UserId == request.ShareWithUserId))
            {
                throw new BadRequestException($"User {request.ShareWithUserId} already has access to account {request.AccountId}");
            }

            account.UserBankAccounts.Add(new UserBankAccountEntity
            {
                BankAccountId = account.Id,
                UserId = user.Id
            });

            await bankAccountRepository.UpdateAsync(account);

            return Unit.Value;
        }

        private async Task<BankAccountEntity> EnsureCurrentUserHasAccessToAccountAsync(
            Guid accountId,
            Guid currentUserId)
        {
            var account = (await bankAccountRepository.GetByIdWithUsersAsync(accountId))
                .AssertEntityFound(accountId);

            if (account.UserBankAccounts.All(ua => ua.UserId != currentUserId))
            {
                throw new ForbiddenException($"Current user does not have access to account {accountId}");
            }

            return account;
        }
    }
}
