using System;
using System.Collections.Generic;
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
        public string Name => $"{FirstName} {LastName}";

        [NotMapped]
        public int Age => DateTime.Now.Year - BirthYear;

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

        public string GetInfo() => $"{Name}, возраст {Age} (логин: {Login})";

        public bool CheckPassword(string password) => Password == password;
    }
}