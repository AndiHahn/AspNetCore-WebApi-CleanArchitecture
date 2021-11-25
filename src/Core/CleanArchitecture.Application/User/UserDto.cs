using System;
using AutoMapper;
using CleanArchitecture.Core;

namespace CleanArchitecture.Application.User
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public static void ApplyMappingConfiguration(IMapperConfigurationExpression config)
        {
            config.CreateMap<Core.User, UserDto>();
        }
    }
}
