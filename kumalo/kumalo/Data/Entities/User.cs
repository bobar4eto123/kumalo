using System.ComponentModel.DataAnnotations;

namespace kumalo.Data.Entities
{
    public class User
    {
        [Required]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Role { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PasswordSalt {  get; set; }

        [Required]
        public string PictureUrl {  get; set; }

        [Required]
        public string FirstName {  get; set; }

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

        public User(string username, string password, string role)
        {
            this.Role = role;
            this.Username = username;
            this.Password = password;
            this.PictureUrl = "";
            this.FirstName = "";
            this.LastName = "";
            this.Age = "";
            this.City = "";
            this.PhoneNumber = "";
            this.Description = "";
            this.ReceivedLikesFrom = new List<string>();
        }

    }
}
