﻿namespace RailWiki.Shared.Services.Users
{
    public class RegisterUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
