﻿namespace Dynamic_Eye.Services
{
    public interface IAuthenticationService
    {
        Task<string> AuthenticateAsync(string email, string password);
    }
}
