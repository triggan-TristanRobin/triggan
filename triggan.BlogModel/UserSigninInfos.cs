using System.ComponentModel.DataAnnotations;

namespace triggan.BlogModel
{
    public class UserSigninInfos
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}