using System;

namespace CleanArchitecture.Services.Bill.Models
{
    public class BillUpdateModel
    {
#nullable enable
        public int? AccountId { get; set; }
        public int? UserId { get; set; }
        public int? CategoryId { get; set; }
        public string? ShopName { get; set; }
        public double? Price { get; set; }
        public DateTime? Date { get; set; }
        public string? Notes { get; set; }
#nullable disable
        public byte[] Version { get; set; }
    }
}