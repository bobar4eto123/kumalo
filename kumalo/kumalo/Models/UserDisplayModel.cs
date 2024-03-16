using System.ComponentModel.DataAnnotations;

namespace kumalo.Models
{
    public class UserDisplayModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int LikesCount { get; set; }
    }
}
