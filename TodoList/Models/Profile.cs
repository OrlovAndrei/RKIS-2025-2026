using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TodoList.Models
{
    [Table("Profiles")]
    public class Profile
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Login { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        public int BirthYear { get; set; }

        public ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();

        [NotMapped]
        public int Age => DateTime.Now.Year - BirthYear;

        [NotMapped]
        public string Name => $"{FirstName} {LastName}";

        public Profile() { }

        public Profile(string login, string password, string firstName, string lastName, int birthYear)
        {
            Id = Guid.NewGuid();
            Login = login;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
        }

        [JsonConstructor]
        public Profile(Guid id, string login, string password, string firstName, string lastName, int birthYear)
        {
            Id = id;
            Login = login;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
        }

        public string GetInfo() => $"{FirstName} {LastName}, возраст {Age} (логин: {Login})";

        public bool CheckPassword(string password) => Password == password;

        public void Update(string firstName, string lastName, int birthYear)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
        }
    }
}