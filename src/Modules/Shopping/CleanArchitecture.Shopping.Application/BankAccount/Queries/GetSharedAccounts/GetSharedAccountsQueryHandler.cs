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

namespace CleanArchitecture.Shopping.Application.BankAccount.Queries.GetSharedAccounts
{
    internal class GetSharedAccountsQueryHandler : IQueryHandler<GetSharedAccountsQuery, Result<IEnumerable<BankAccountDto>>>
    {
        private readonly IMapper mapper;
        private readonly IShoppingDbContext dbContext;

        public GetSharedAccountsQueryHandler(
            IMapper mapper,
            IShoppingDbContext dbContext)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task<Result<IEnumerable<BankAccountDto>>> Handle(GetSharedAccountsQuery request, CancellationToken cancellationToken)
        {
            return this.dbContext.BankAccount
                .Where(ba => ba.SharedWithUsers.Any(ub => ub.UserId == request.CurrentUserId))
                .ToListAsync(cancellationToken)
                .ContinueWith(
                    r => Result<IEnumerable<BankAccountDto>>.Success(r.Result.Select(this.mapper.Map<BankAccountDto>)),
                    cancellationToken);
        }
    }
}
