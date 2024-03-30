using System.ComponentModel.DataAnnotations;

namespace Dynamic_Eye.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }

        public required string username { get; set; }

        [EmailAddress]
        public required string email { get; set; }

        public required string hash { get; set; }

        public DateTime created { get; set; }

        public DateTime updated { get; set; }
    }
}
