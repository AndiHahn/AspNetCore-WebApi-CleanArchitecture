using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.CrudServices.Models.BankAccount;
using CleanArchitecture.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.UseCases.Collaboration.Queries
{
    public class GetSharedAccountsQuery : IRequest<IEnumerable<BankAccountModel>>
    {
        public Guid CurrentUserId { get; }

        public GetSharedAccountsQuery(Guid currentUserId)
        {
            CurrentUserId = currentUserId;
        }
    }

    public class GetSharedAccountsQueryHandler : IRequestHandler<GetSharedAccountsQuery, IEnumerable<BankAccountModel>>
    {
        private readonly IMapper mapper;
        private readonly IBudgetContext context;

        public GetSharedAccountsQueryHandler(
            IMapper mapper,
            IBudgetContext context)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<BankAccountModel>> Handle(GetSharedAccountsQuery request, CancellationToken cancellationToken)
        {
            Guid currentUserId = request.CurrentUserId;

            var sharedAccounts = await context.UserBankAccount
                .Include(uba => uba.BankAccount)
                .Where(uba => uba.UserId == currentUserId &&
                              uba.BankAccount.OwnerId != currentUserId)
                .Select(uba => uba.BankAccount)
                .ToListAsync(cancellationToken);

            return sharedAccounts.Select(mapper.Map<BankAccountModel>);
        }
    }
}
