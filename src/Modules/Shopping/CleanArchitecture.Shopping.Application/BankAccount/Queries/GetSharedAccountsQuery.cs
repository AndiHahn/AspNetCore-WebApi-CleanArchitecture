using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Core.Interfaces;

namespace CleanArchitecture.Shopping.Application.BankAccount.Queries
{
    public class GetSharedAccountsQuery : IQuery<Result<IEnumerable<BankAccountDto>>>
    {
        public GetSharedAccountsQuery(Guid currentUserId)
        {
            CurrentUserId = currentUserId;
        }

        public Guid CurrentUserId { get; }
    }

    internal class GetSharedAccountsQueryHandler : IQueryHandler<GetSharedAccountsQuery, Result<IEnumerable<BankAccountDto>>>
    {
        private readonly IMapper mapper;
        private readonly IBankAccountRepository bankAccountRepository;

        public GetSharedAccountsQueryHandler(
            IMapper mapper,
            IBankAccountRepository bankAccountRepository)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.bankAccountRepository = bankAccountRepository;
        }

        public Task<Result<IEnumerable<BankAccountDto>>> Handle(GetSharedAccountsQuery request, CancellationToken cancellationToken)
        {
            return this.bankAccountRepository
                .ListSharedAsync(request.CurrentUserId, cancellationToken)
                .ContinueWith(
                    r => Result<IEnumerable<BankAccountDto>>.Success(r.Result.Select(this.mapper.Map<BankAccountDto>)),
                    cancellationToken);
        }
    }
}
