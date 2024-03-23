using System.ComponentModel.DataAnnotations;

namespace kumalo.Data.Entities
{
    public class User
    {
        [Required]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PictureUrl {  get; set; }

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

        public User(string username, string password, string firstName, string lastName, int age, string city, string PhoneNumber, string description)
        {
            this.Username = username;
            this.Password = password;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Age = age;
            this.City = city;
            this.PhoneNumber = PhoneNumber;
            this.Description = description;
            this.LikesCount = 0;
        }


    }
}
