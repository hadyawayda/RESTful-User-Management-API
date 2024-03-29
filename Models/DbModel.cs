using System.ComponentModel.DataAnnotations;

namespace Dynamic_Eye.Models
{
    public class User
    {
        public int id { get; set; }
        public string? username { get; set; }
        //[Required]
        //public string Username { get; set; }
        public string? email { get; set; }
        //[Required]
        //[EmailAddress]
        //public string Email { get; set; }
        public string hash { get; set; }
        //public byte[] PasswordHash { get; set; }

        //public byte[] PasswordSalt { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
    }

}
