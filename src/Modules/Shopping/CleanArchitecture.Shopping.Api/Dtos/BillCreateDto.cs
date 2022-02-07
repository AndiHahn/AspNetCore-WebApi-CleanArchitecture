using System;
using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Shopping.Application.Bill;

#nullable enable

namespace CleanArchitecture.Shopping.Api.Dtos
{
    public class BillCreateDto
    {
        [Required]
        public Guid BankAccountId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string ShopName { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public CategoryDto Category { get; set; }

        public DateTime? Date { get; set; }

        public string? Notes { get; set; }
    }
}
