using System;
using CleanArchitecture.Shared.Application.Mapping;

namespace CleanArchitecture.Shopping.Application.BankAccount
{
    public class BankAccountDto : IMappableDto<Core.BankAccount>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
