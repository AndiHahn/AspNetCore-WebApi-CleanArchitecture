using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Core.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.BankAccount
{
    public class GetBankAccountsQuery : IRequest<IEnumerable<BankAccountDto>>
    {
        public GetBankAccountsQuery(Guid currentUserId)
        {
            CurrentUserId = currentUserId;
        }

        public Guid CurrentUserId { get; }
    }

    internal class GetBankAccountsQueryHandler : IRequestHandler<GetBankAccountsQuery, IEnumerable<BankAccountDto>>
    {
        private readonly IMapper mapper;
        private readonly IBankAccountRepository bankAccountRepository;

        public GetBankAccountsQueryHandler(
            IMapper mapper,
            IBankAccountRepository bankAccountRepository)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.bankAccountRepository = bankAccountRepository ?? throw new ArgumentNullException(nameof(bankAccountRepository));
        }

        public Task<IEnumerable<BankAccountDto>> Handle(GetBankAccountsQuery request, CancellationToken cancellationToken)
        {
            return this.bankAccountRepository
                .ListOwnAsync(request.CurrentUserId, cancellationToken)
                .ContinueWith(r => r.Result.Select(this.mapper.Map<BankAccountDto>));
        }
    }
}
