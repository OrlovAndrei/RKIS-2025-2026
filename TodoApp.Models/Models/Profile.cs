using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Models
{
    [Table("Profiles")]
    public class Profile
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(64)]
        public string Login { get; set; } = string.Empty;

        [Required]
        [MaxLength(128)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MaxLength(64)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(64)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public int BirthYear { get; set; }

        public virtual ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [NotMapped]
        public int Age => DateTime.Now.Year - BirthYear;

        public Profile() { }

        public Profile(string firstName, string lastName, int birthYear) : this()
        {
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
        }

        public Profile(string login, string password, string firstName, string lastName, int birthYear) : this()
        {
            Login = login;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
        }

        public Profile(Guid id, string login, string password, string firstName, string lastName, int birthYear)
        {
            Id = id;
            Login = login;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
        }

        public string GetInfo()
        {
            int age = DateTime.Now.Year - BirthYear;
            if (!string.IsNullOrEmpty(Login))
                return $"{FirstName} {LastName}, возраст {age} (логин: {Login})";
            return $"{FirstName} {LastName}, возраст {age}";
        }

        public bool CheckPassword(string password) => Password == password;
    }
}