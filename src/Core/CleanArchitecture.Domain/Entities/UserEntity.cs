﻿using System;
using System.Collections.Generic;
using CleanArchitecture.Domain.Base;

namespace CleanArchitecture.Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<UserBillEntity> UserBills { get; set; }
        public virtual ICollection<UserBankAccountEntity> UserAccounts { get; set; }

        public UserEntity()
        {
        }

        public UserEntity(Guid id)
        {
            Id = id;
        }
    }
}