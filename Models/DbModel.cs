namespace Dynamic_Eye.Models
{
    public class User
    {
        public int id { get; set; }
        public string? username { get; set; }
        public string? email { get; set; }
        public string? hash { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
    }

}
