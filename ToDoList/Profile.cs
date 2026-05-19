using System;

namespace ToDoList
{
    public class Profile
    {
        public string FirstName { get;  set; }
        public string LastName { get; private set; }
        public int BirthYear { get; private set; }
		public Guid Id { get; set; } = Guid.NewGuid();


		public Profile(string firstName, string lastName, int birthYear)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
        }

        public string GetInfo()
        {
            int age = DateTime.Now.Year - BirthYear;
            return $"{FirstName} {LastName}, возраст {age}";
        }
    }
}