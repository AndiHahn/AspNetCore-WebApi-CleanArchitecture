using System;
using CleanArchitecture.Domain.Base;

namespace CleanArchitecture.Domain.Entities
{
    public class BillEntity : BaseEntity, IVersionableEntity
    {
        public Guid AccountId { get; set; }
        public Guid UserId { get; set; }
        public Guid BillCategoryId { get; set; }
        public string ShopName { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }
        public string PictureURL { get; set; }
        public string Notes { get; set; }
        public byte[] Version { get; set; }

        public AccountEntity Account { get; set; }
        public UserEntity User { get; set; }
        public BillCategoryEntity BillCategory { get; set; }
    }
}
