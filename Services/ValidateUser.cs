using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Dynamic_Eye.Services
{
    public class ValidateUser
    {
        private readonly UsersDbContext _context;

        public ValidateUser(UsersDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ValidateUserCredentials(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.username == username);

            if (user == null) return false;

            return VerifyPasswordHash(password, user.hash);
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                byte[] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }
            return true;
        }

        public static string HashPassword(string password)
        {
            // Generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            // Return the salt and the hashed password, for storage
            return $"{Convert.ToBase64String(salt)}:{hashed}";
        }

        public static bool VerifyPassword(string hashedPasswordWithSalt, string passwordToCheck)
        {
            // Split the hash and salt
            var parts = hashedPasswordWithSalt.Split(':');
            if (parts.Length != 2)
            {
                throw new FormatException("Unexpected hashed password format. Should be 'salt:hash'.");
            }
            var salt = Convert.FromBase64String(parts[0]);
            var hashedPassword = parts[1];

            // Hash the given password
            var hashToCheck = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: passwordToCheck,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            // Compare and return
            return hashedPassword == hashToCheck;
        }
    }
}