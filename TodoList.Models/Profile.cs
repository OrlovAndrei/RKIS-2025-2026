using System;
using System.Collections.Generic;

namespace TodoList.Models
{
    public record Profile
    {
        public Guid Id { get; set; }
        public string Login { get; set; } = "";
        public string Password { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public int BirthYear { get; set; }

        public virtual ICollection<TodoItem> Todos { get; set; } = new List<TodoItem>();

        public Profile() { }

        public Profile(Guid id, string login, string password, string firstName, string lastName, int birthYear)
        {
            Id = id;
            Login = login;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
        }

        public int GetAge() => DateTime.Now.Year - BirthYear;

        public void ShowProfile()
        {
            Console.WriteLine($"{FirstName} {LastName}, {BirthYear} год рождения ({GetAge()} лет)");
        }
    }
}
