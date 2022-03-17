using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Shopping.Application.BankAccount.Queries
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

    internal class GetBankAccountByIdQueryHandler : IQueryHandler<GetBankAccountByIdQuery, Result<BankAccountDto>>
    {
        private readonly IShoppingDbContext dbContext;
        private readonly IMapper mapper;

        public GetBankAccountByIdQueryHandler(
            IShoppingDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<BankAccountDto>> Handle(GetBankAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var account = await this.dbContext.BankAccount
                .Include(b => b.SharedWithUsers)
                .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);
            if (account is null)
            {
                return Result<BankAccountDto>.NotFound($"Bank account with id {request.Id} not found.");
            }

            if (!account.HasAccess(request.CurrentUserId))
            {
                return Result<BankAccountDto>.Forbidden($"Current user does not have access to account {request.Id}.");
            }

            return this.mapper.Map<BankAccountDto>(account);
        }
    }
}
