using System;
using AutoMapper;
using CleanArchitecture.Shared.Application.Mapping;

namespace CleanArchitecture.Shopping.Application.Bill
{
    public class BillDto : IMappableDto<BillDto>
    {
        public Guid Id { get; set; }

        public Guid BankAccountId { get; set; }

        public Guid CreatedByUserId { get; set; }

        public string ShopName { get; set; }

        public double Price { get; set; }

        public DateTime Date { get; set; }

        public string Notes { get; set; }

        public CategoryDto Category { get; set; }

        public byte[] Version { get; set; }

        public void MappingConfig(Profile profile)
        {
            profile.CreateMap<Core.Bill.Bill, BillDto>()
                .ForMember(b => b.Category, b => b.MapFrom(m => m.Category.ToDto()));
        }
    }
}
