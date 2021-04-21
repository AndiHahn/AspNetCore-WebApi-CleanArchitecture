using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Core.Interfaces.Data;
using CleanArchitecture.Core.Models.Domain.BankAccount;
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

            return await context.UserBankAccount
                .Include(uba => uba.BankAccount)
                .Where(uba => uba.UserId == currentUserId && uba.BankAccount.OwnerId != currentUserId)
                .Select(uba => mapper.Map<BankAccountModel>(uba.BankAccount))
                .ToListAsync(cancellationToken);
        }
    }
}
