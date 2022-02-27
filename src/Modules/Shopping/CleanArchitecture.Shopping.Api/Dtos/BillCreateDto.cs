using System;
using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Shopping.Application.Bill;

#nullable enable

namespace CleanArchitecture.Shopping.Api.Dtos
{
    public class BillCreateDto
    {
        public Guid BankAccountId { get; set; }

        public string ShopName { get; set; }

        public double Price { get; set; }

        public CategoryDto Category { get; set; }

        public DateTime? Date { get; set; }

        public string? Notes { get; set; }
    }
}
