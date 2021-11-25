﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Core.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.BankAccount
{
    public class GetSharedAccountsQuery : IRequest<IEnumerable<BankAccountDto>>
    {
        public GetSharedAccountsQuery(Guid currentUserId)
        {
            CurrentUserId = currentUserId;
        }

        public Guid CurrentUserId { get; }
    }

    internal class GetSharedAccountsQueryHandler : IRequestHandler<GetSharedAccountsQuery, IEnumerable<BankAccountDto>>
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

        public Task<IEnumerable<BankAccountDto>> Handle(GetSharedAccountsQuery request, CancellationToken cancellationToken)
        {
            return this.bankAccountRepository
                .ListSharedAsync(request.CurrentUserId, cancellationToken)
                .ContinueWith(r => r.Result.Select(this.mapper.Map<BankAccountDto>));
        }
    }
}
