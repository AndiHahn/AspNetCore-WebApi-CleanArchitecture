using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.CrudServices.Models.Bill;
using CleanArchitecture.Domain.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.UseCases.Collaboration.Queries
{
    public class GetSharedBillsQuery : IRequest<IEnumerable<BillModel>>
    {
        public Guid CurrentUserId { get; }

        public GetSharedBillsQuery(Guid currentUserId)
        {
            CurrentUserId = currentUserId;
        }
    }

    public class GetSharedBillsQueryHandler : IRequestHandler<GetSharedBillsQuery, IEnumerable<BillModel>>
    {
        private readonly IMapper mapper;
        private readonly IBillRepository billRepository;

        public GetSharedBillsQueryHandler(
            IMapper mapper,
            IBillRepository billRepository)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.billRepository = billRepository ?? throw new ArgumentNullException(nameof(billRepository));
        }

        public async Task<IEnumerable<BillModel>> Handle(GetSharedBillsQuery request, CancellationToken cancellationToken)
        {
            Guid currentUserId = request.CurrentUserId;

            var sharedBills = await billRepository.GetSharedBillsAsync(currentUserId);

            return sharedBills.Select(mapper.Map<BillModel>);
        }
    }
}
