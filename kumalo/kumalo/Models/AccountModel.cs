using System.ComponentModel.DataAnnotations;

namespace kumalo.Models
{
    public class AccountModel
    {
        [Required(ErrorMessage = "First name field is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name field is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Age field is required")]
        [Range(1, 150, ErrorMessage = "Please enter a valid age between 1 and 150")]
        public int Age { get; set; }

        [Required(ErrorMessage = "City name field is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Phone number field is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must contain exactly 10 numeric characters")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Description field is required")]
        public string Description { get; set; }
    }
}
