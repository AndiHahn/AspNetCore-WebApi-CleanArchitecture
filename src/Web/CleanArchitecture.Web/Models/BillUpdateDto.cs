using System;

#nullable enable

namespace CleanArchitecture.Web.Api.Models
{
    public class BillUpdateDto
    {
        public string? ShopName { get; set; }

        public double? Price { get; set; }

        public DateTime? Date { get; set; }

        public string? Notes { get; set; }

        public CategoryDto? Category { get; set; }

        public byte[] Version { get; set; }
    }
}
