using System;

namespace CleanArchitecture.Core
{
    public class UserBill
    {
        private UserBill()
        {
        }

        public UserBill(Guid billId, Guid userId)
        {
            this.BillId = billId;
            this.UserId = userId;
        }

        public UserBill(Bill bill, User user)
            : this(bill.Id, user.Id)
        {
            if (bill == null)
            {
                throw new ArgumentNullException(nameof(bill));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            this.Bill = bill;
            this.User = user;
        }

        public Guid BillId { get; private set; }

        public Bill Bill { get; private set; }

        public Guid UserId { get; private set; }

        public User User { get; private set; }
    }
}
