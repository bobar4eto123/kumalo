using System.ComponentModel.DataAnnotations;

namespace kumalo.Models
{
    public class UserLoginAndRegisterModel
    {
        [Required(ErrorMessage = "Username field is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password field is required.")]
        public string Password { get; set; }
    }
}
