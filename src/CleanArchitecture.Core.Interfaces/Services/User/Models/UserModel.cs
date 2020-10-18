using AutoMapper;
using CleanArchitecture.Domain.Entities;
using Newtonsoft.Json;

namespace CleanArchitecture.Core.Interfaces.Services.User.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public static void ApplyMappingConfiguration(IMapperConfigurationExpression config)
        {
            config.CreateMap<UserEntity, UserModel>();
        }
    }
}
