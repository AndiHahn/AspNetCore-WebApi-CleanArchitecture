using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Shopping.Application.BankAccount.Commands.ShareAccountWithUser
{
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
