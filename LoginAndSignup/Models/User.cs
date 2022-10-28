using System.ComponentModel.DataAnnotations;

namespace LoginAndSignup.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }
        public string firstname{ get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string token { get; set; }
        public string role { get; set; }
    }
}
