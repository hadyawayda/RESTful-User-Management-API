using Dynamic_Eye.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dynamic_Eye.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UsersDbContext _context;
        private readonly JwtSettings _jwtSettings;

        public AuthenticationService(UsersDbContext context, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<string> AuthenticateAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == email);
            if (user == null) return null!;

            var isValidUser = BCrypt.Net.BCrypt.Verify(password, user.hash);
            if (!isValidUser) return null!;

            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, email)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.SecretKey));

            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            JwtSecurityToken tokenDescriptor = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                signingCredentials: credentials,
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
