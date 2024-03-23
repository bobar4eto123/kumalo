using System.ComponentModel.DataAnnotations;

namespace kumalo.Models
{
    public class UserRegisterModel
    {
        [Required(ErrorMessage = "Username field is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password field is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role field is required.")]
        public string Role { get; set; }
    }
}
