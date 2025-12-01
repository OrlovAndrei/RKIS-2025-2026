using System;

namespace TodoList
{
    /// <summary>
    /// Профиль пользователя приложения.
    /// </summary>
    internal class Profile
    {
        private string _firstName;
        private string _lastName;
        private int _birthYear;

        public string FirstName
        {
            get => _firstName;
            set => _firstName = string.IsNullOrWhiteSpace(value) ? "Имя" : value.Trim();
        }

        public string LastName
        {
            get => _lastName;
            set => _lastName = string.IsNullOrWhiteSpace(value) ? "Фамилия" : value.Trim();
        }

        public int BirthYear
        {
            get => _birthYear;
            set => _birthYear = value;
        }

        public Profile(string firstName, string lastName, int birthYear)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
        }

        public string GetInfo()
        {
            int currentYear = DateTime.Now.Year;
            int age = currentYear - _birthYear;
            return $"{FirstName} {LastName}, возраст {age}";
        }
    }
}
