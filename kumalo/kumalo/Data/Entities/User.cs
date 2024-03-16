using System.ComponentModel.DataAnnotations;

namespace kumalo.Data.Entities
{
    public class User
    {
        public User(string username, string password)
        {
            this.Username = username;
            this.Password = password;
            this.FirstName = "";
            this.LastName = "";
            this.Age = 0;
            this.City = "";
            this.PhoneNumber = "";
            this.Description = "";
            this.LikesCount = 0;
        }

        [Required]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FirstName {  get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Description {  get; set; }

        [Required]
        public int LikesCount { get; set; }
    }
}
