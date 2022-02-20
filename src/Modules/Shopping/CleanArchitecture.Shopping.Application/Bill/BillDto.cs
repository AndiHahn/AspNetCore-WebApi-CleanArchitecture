using System;
using AutoMapper;

namespace CleanArchitecture.Shopping.Application.Bill
{
    public class BillDto
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

        public static void ApplyMappingConfiguration(IMapperConfigurationExpression config)
        {
            config.CreateMap<Core.Bill.Bill, BillDto>()
                .ForMember(b => b.Category, b => b.MapFrom(m => m.Category.ToDto()));
        }
    }
}
