using System;
using CleanArchitecture.Shared.Application.Mapping;

namespace CleanArchitecture.Shopping.Application.User
{
    public class UserDto : IMappableDto<UserDto>
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }
    }
}
