using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Application.Validations;
using CleanArchitecture.Core.Interfaces.Data;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.UseCases.Collaboration.Commands
{
    public class ShareAccountWithUserCommand : IRequest
    {
        public Guid AccountId { get; }
        public Guid ShareWithUserId { get; }

        public ShareAccountWithUserCommand(Guid accountId, Guid shareWithUserId)
        {
            AccountId = accountId;
            ShareWithUserId = shareWithUserId;
        }
    }

    public class ShareAccountWithUserCommandHandler : IRequestHandler<ShareAccountWithUserCommand>
    {
        private readonly IBudgetContext context;
        private readonly ICurrentUserService currentUserService;

        public ShareAccountWithUserCommandHandler(
            IBudgetContext context,
            ICurrentUserService currentUserService)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        public async Task<Unit> Handle(ShareAccountWithUserCommand request, CancellationToken cancellationToken)
        {
            var account = await EnsureCurrentUserHasAccessToAccountAsync(request.AccountId);
            var user = (await context.User.FindAsync(request.ShareWithUserId)).AssertEntityFound(request.ShareWithUserId);

            if (account.UserBankAccounts.Any(ua => ua.UserId == request.ShareWithUserId))
            {
                throw new BadRequestException($"User {request.ShareWithUserId} already has access to account {request.AccountId}");
            }

            account.UserBankAccounts.Add(new UserBankAccountEntity
            {
                BankAccountId = account.Id,
                UserId = user.Id
            });

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private async Task<BankAccountEntity> EnsureCurrentUserHasAccessToAccountAsync(Guid accountId)
        {
            var account = (await context.BankAccount
                    .Include(a => a.UserBankAccounts)
                    .FirstOrDefaultAsync(a => a.Id == accountId))
                .AssertEntityFound(accountId);

            Guid currentUserId = currentUserService.GetCurrentUserId();
            if (account.UserBankAccounts.All(ua => ua.UserId != currentUserId))
            {
                throw new ForbiddenException($"Current user does not have access to account {accountId}");
            }

            return account;
        }
    }
}
