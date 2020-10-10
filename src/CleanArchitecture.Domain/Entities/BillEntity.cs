using System;
using CleanArchitecture.Domain.Base;

namespace CleanArchitecture.Domain.Entities
{
    public class BillEntity : VersionableEntity
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public int BillCategoryId { get; set; }
        public string ShopName { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }
        public string PictureURL { get; set; }
        public string Notes { get; set; }

        public AccountEntity Account { get; set; }
        public UserEntity User { get; set; }
        public BillCategoryEntity BillCategory { get; set; }
    }
}
