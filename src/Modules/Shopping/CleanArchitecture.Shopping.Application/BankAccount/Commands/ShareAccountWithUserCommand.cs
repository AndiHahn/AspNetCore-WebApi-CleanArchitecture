using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Core.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Shopping.Application.BankAccount.Commands
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

    public class ShareAccountWithUserCommandValidator : AbstractValidator<ShareAccountWithUserCommand>
    {
        public ShareAccountWithUserCommandValidator()
        {
            RuleFor(c => c.AccountId).NotEmpty();
            RuleFor(c => c.ShareWithUserId).NotEmpty();
        }
    }

    internal class ShareAccountWithUserCommandHandler : ICommandHandler<ShareAccountWithUserCommand, Result>
    {
        private readonly IShoppingDbContext dbContext;

        public ShareAccountWithUserCommandHandler(
            IShoppingDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Result> Handle(ShareAccountWithUserCommand request, CancellationToken cancellationToken)
        {
            var account = await this.dbContext.BankAccount
                .Include(b => b.SharedWithUsers)
                .FirstOrDefaultAsync(b => b.Id == request.AccountId, cancellationToken);
            if (account is null)
            {
                return Result.NotFound($"Account with id {request.AccountId} not found.");
            }

            if (!account.IsOwner(request.CurrentUserId))
            {
                return Result.Forbidden($"Current user does not have access to account {request.AccountId}");
            }

            var user = await this.dbContext.User.FindByIdAsync(request.ShareWithUserId, cancellationToken);
            if (user is null)
            {
                return Result.NotFound($"User with id {request.ShareWithUserId} not found.");
            }

            account.ShareWithUser(user);

            this.dbContext.BankAccount.Update(account);

            await this.dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
