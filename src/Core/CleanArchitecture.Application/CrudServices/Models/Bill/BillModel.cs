using System;
using AutoMapper;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Application.CrudServices.Models.Bill
{
    public class BillModel
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
            config.CreateMap<BillEntity, BillModel>();
        }
    }

    public enum BillSort
    {
        ShopName,
        Price,
        Date,
        Notes
    }

    public enum BillFilter
    {
        ShopName,
        Price,
        Date,
        Notes
    }
}