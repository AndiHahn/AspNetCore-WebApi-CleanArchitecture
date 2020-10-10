using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Services.User.Models;

namespace CleanArchitecture.Services.User.Extensions
{
    public static class UserMappingExtensions
    {
        public static UserModel ToModel(this UserEntity entity)
        {
            return new UserModel()
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Username = entity.UserName,
                Password = entity.Password
            };
        }
    }
}