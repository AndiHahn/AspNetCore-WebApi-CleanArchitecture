using System;

namespace CleanArchitecture.Services.Bill.Models
{
    public class BillModel
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string User { get; set; }
        public string ShopName { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public byte[] Version { get; set; }
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