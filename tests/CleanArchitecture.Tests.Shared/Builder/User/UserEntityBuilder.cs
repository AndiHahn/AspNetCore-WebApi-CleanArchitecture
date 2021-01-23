using System;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Tests.Shared.Builder.User
{
    public class UserEntityBuilder
    {
        private readonly UserEntity user;

        public UserEntityBuilder(Guid userId = default)
        {
            user = new UserEntity()
            {
                Id = userId != default ? userId : Guid.NewGuid(),
                FirstName = "FirstName",
                LastName = "LastName",
                UserName = "UserName",
                Password = "Password",
                Salt = "Salt"
            };
        }

        public UserEntity Build()
        {
            return user;
        }
    }
}
