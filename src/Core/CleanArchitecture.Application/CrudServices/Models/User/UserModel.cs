using System;
using AutoMapper;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.CrudServices.Models.User
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static void ApplyMappingConfiguration(IMapperConfigurationExpression config)
        {
            config.CreateMap<UserEntity, UserModel>();
        }
    }

    public enum UserSort
    {
    }

    public enum UserFilter
    {
    }
}
