using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace kumalo.Models
{
    public class UserLoginAndRegisterModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
