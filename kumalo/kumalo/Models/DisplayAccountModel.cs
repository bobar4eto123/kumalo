using kumalo.Data.Entities;
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
        public string Age { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public List<string> ReceivedLikesFrom { get; set; }

        public DisplayAccountModel(User userToBeConverted)
        {
            this.Id = userToBeConverted.Id;
            this.Role = userToBeConverted.Role;
            this.PictureUrl = userToBeConverted.PictureUrl;
            this.FirstName = userToBeConverted.FirstName;
            this.LastName = userToBeConverted.LastName;
            this.Age = userToBeConverted.Age;
            this.City = userToBeConverted.City;
            this.PhoneNumber = userToBeConverted.PhoneNumber;
            this.Description = userToBeConverted.Description;
            this.ReceivedLikesFrom = userToBeConverted.ReceivedLikesFrom;
        }
    }
}
