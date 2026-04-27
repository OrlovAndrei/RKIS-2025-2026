using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Models
{
    public class Profile
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Login { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Password { get; set; }

        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        [Range(1900, 2023)]
        public int BirthYear { get; set; }

        public int Age => DateTime.Now.Year - BirthYear;

        public virtual ICollection<TodoItem> Todos { get; set; } = new List<TodoItem>();

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

        public string GetInfo()
        {
            return $"Имя:{FirstName} Фамилия:{LastName} (возраст: {Age}, логин: {Login})";
        }
    }
}