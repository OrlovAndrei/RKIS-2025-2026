using System;

namespace Todolist
{
    public class Profile
    {
        // Приватные поля
        private string firstName;
        private string lastName;
        private int birthYear;

        // Публичные свойства
        public string FirstName
        {
            get => firstName;
            set => firstName = value ?? "";
        }

        public string LastName
        {
            get => lastName;
            set => lastName = value ?? "";
        }

        public int BirthYear
        {
            get => birthYear;
            set => birthYear = value;
        }

        // Вычисляемое свойство
        public int Age => DateTime.Now.Year - birthYear;

        // Конструктор
        public Profile(string firstName = "", string lastName = "", int birthYear = 0)
        {
            this.firstName = firstName ?? "";
            this.lastName = lastName ?? "";
            this.birthYear = birthYear;
        }

        // Публичные методы
        public string GetInfo()
        {
            return $"{firstName} {lastName}, возраст {Age}";
        }

        public string GetDetailedInfo()
        {
            return $"Имя: {firstName}\nФамилия: {lastName}\nГод рождения: {birthYear}\nВозраст: {Age}";
        }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(firstName) && 
                   !string.IsNullOrWhiteSpace(lastName) && 
                   birthYear > 1900 && birthYear <= DateTime.Now.Year;
        }
    }
}