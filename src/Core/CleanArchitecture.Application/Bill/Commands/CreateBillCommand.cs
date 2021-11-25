using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Core;
using CleanArchitecture.Core.Interfaces;
using MediatR;

#nullable enable

namespace CleanArchitecture.Application.Bill
{
    public class CreateBillCommand : IRequest<BillDto>
    {
        public CreateBillCommand(
            Guid currentUserId,
            Guid bankAccountId,
            string shopName,
            double price,
            Category category,
            DateTime? date,
            string? notes)
        {
            this.CurrentUserId = currentUserId;
            this.BankAccountId = bankAccountId;
            this.ShopName = shopName;
            this.Price = price;
            this.Category = category;
            this.Date = date;
            this.Notes = notes;
        }

        public Guid CurrentUserId { get; }

        public Guid BankAccountId { get; }

        public string ShopName { get; }

        public double Price { get; }

        public Category Category { get; }

        public DateTime? Date { get; }

        public string? Notes { get; }
    }

    internal class CreateBillCommandHandler : IRequestHandler<CreateBillCommand, BillDto>
    {
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;
        private readonly IBillRepository billRepository;
        private readonly IBankAccountRepository bankAccountRepository;

        public CreateBillCommandHandler(
            IMapper mapper,
            IUserRepository userRepository,
            IBillRepository billRepository,
            IBankAccountRepository bankAccountRepository)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.billRepository = billRepository ?? throw new ArgumentNullException(nameof(billRepository));
            this.bankAccountRepository = bankAccountRepository ?? throw new ArgumentNullException(nameof(bankAccountRepository));
        }

        public async Task<BillDto> Handle(CreateBillCommand request, CancellationToken cancellationToken)
        {
            var account = await this.bankAccountRepository.GetByIdAsync(request.BankAccountId);

            var user = await this.userRepository.GetByIdAsync(request.CurrentUserId);

            var bill = await this.billRepository.AddAsync(
                new Core.Bill(user, account, request.ShopName, request.Price, request.Date, request.Notes, request.Category),
                cancellationToken);

            return this.mapper.Map<BillDto>(bill);
        }
    }
}
