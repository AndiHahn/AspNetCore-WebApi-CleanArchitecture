using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Shopping.Application.BankAccount.Queries
{
    public class GetBankAccountsQuery : IQuery<Result<IEnumerable<BankAccountDto>>>
    {
        public GetBankAccountsQuery(Guid currentUserId)
        {
            CurrentUserId = currentUserId;
        }

        public Guid CurrentUserId { get; }
    }

    internal class GetBankAccountsQueryHandler : IQueryHandler<GetBankAccountsQuery, Result<IEnumerable<BankAccountDto>>>
    {
        private readonly IMapper mapper;
        private readonly IShoppingDbContext dbContext;

        public GetBankAccountsQueryHandler(
            IMapper mapper,
            IShoppingDbContext dbContext)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task<Result<IEnumerable<BankAccountDto>>> Handle(GetBankAccountsQuery request, CancellationToken cancellationToken)
        {
            return this.dbContext.BankAccount
                .Where(b => b.OwnerId == request.CurrentUserId)
                .ToListAsync(cancellationToken)
                .ContinueWith(
                    r => Result<IEnumerable<BankAccountDto>>.Success(r.Result.Select(this.mapper.Map<BankAccountDto>)),
                    cancellationToken);
        }
    }
}
