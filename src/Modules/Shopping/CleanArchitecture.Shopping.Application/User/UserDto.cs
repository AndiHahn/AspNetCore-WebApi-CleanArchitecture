using System;
using AutoMapper;

namespace CleanArchitecture.Shopping.Application.User
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public static void ApplyMappingConfiguration(IMapperConfigurationExpression config)
        {
            config.CreateMap<Core.User.User, UserDto>();
        }
    }
}
