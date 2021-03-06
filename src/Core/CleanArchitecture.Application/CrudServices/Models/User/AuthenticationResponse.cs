﻿using System;

namespace CleanArchitecture.Application.CrudServices.Models.User
{
    public class AuthenticationResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiryDate { get; set; }
    }
}