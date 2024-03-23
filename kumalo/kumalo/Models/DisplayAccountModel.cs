using System.ComponentModel.DataAnnotations;

namespace kumalo.Models
{
    public class DisplayAccountModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string PictureUrl { get; set; }

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
        public List<string> ReceivedLikesFrom { get; set; }
    }
}
