using System.ComponentModel.DataAnnotations;

namespace Dynamic_Eye.DTOs
{
    public class UserAuthDto
    {
        [EmailAddress]
        public required string email { get; set; }

        public required string password { get; set; }
    }
}
