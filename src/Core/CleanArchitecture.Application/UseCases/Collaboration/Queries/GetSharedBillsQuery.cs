using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Core.Interfaces.Data;
using CleanArchitecture.Core.Models.Domain.Bill;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.UseCases.Collaboration.Queries
{
    public class GetSharedBillsQuery : IRequest<IEnumerable<BillModel>>
    {
    }

    public class GetSharedBillsQueryHandler : IRequestHandler<GetSharedBillsQuery, IEnumerable<BillModel>>
    {
        private readonly IMapper mapper;
        private readonly IBudgetContext context;
        private readonly ICurrentUserService currentUserService;

        public GetSharedBillsQueryHandler(
            IMapper mapper,
            IBudgetContext context,
            ICurrentUserService currentUserService)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        public async Task<IEnumerable<BillModel>> Handle(GetSharedBillsQuery request, CancellationToken cancellationToken)
        {
            Guid currentUserId = currentUserService.GetCurrentUserId();

            return await context.UserBill
                .Include(ub => ub.Bill)
                .Where(ub => ub.UserId == currentUserId && ub.Bill.CreatedByUserId != currentUserId)
                .Select(ub => mapper.Map<BillModel>(ub.Bill))
                .ToListAsync(cancellationToken);
        }
    }
}
