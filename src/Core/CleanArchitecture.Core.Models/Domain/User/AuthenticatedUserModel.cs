using System;

namespace CleanArchitecture.Core.Models.Domain.User
{
    public class AuthenticatedUserModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string Token { get; set; }
    }
}