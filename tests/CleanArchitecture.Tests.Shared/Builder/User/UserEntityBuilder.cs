using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Tests.Shared.Builder.User
{
    public class UserEntityBuilder
    {
        private readonly UserEntity user;

        public UserEntityBuilder()
        {
            user = new UserEntity()
            {
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
