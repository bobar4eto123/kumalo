using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace kumalo.Models
{
    public class UserLoginAndRegisterModel
    {
        [Required(ErrorMessage = "Username field is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Username field is required.")]
        public string Password { get; set; }
    }
}
