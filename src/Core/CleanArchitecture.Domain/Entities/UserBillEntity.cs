using System;

namespace CleanArchitecture.Domain.Entities
{
    public class UserBillEntity
    {
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }
        public Guid BillId { get; set; }
        public BillEntity Bill { get; set; }
    }
}
