using System;
using AutoMapper;
using CleanArchitecture.Core;

namespace CleanArchitecture.Application.Bill
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

        public Category Category { get; set; }

        public byte[] Version { get; set; }

        public static void ApplyMappingConfiguration(IMapperConfigurationExpression config)
        {
            config.CreateMap<Core.Bill, BillDto>();
        }
    }
}