using System.ComponentModel.DataAnnotations;

namespace Dynamic_Eye.DTOs
{
    public class UserDto
    {
        public required string username { get; set; }

        [EmailAddress]
        public required string email { get; set; }

        public required string password { get; set; }
    }
}
