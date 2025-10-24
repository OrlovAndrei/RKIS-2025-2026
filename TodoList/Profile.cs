using System;

namespace Todolist
{
    public class Profile
    {
        // Свойства
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int BirthYear { get; set; }

        // Вычисляемое свойство для возраста
        public int Age => DateTime.Now.Year - BirthYear;

        // Конструктор
        public Profile(string firstName = "", string lastName = "", int birthYear = 0)
        {
            FirstName = firstName ?? "";
            LastName = lastName ?? "";
            BirthYear = birthYear;
        }

        // Метод GetInfo - возвращает строку с информацией
        public string GetInfo()
        {
            return $"{FirstName} {LastName}, возраст {Age}";
        }

        // Дополнительный метод для красивого вывода в профиле
        public string GetDetailedInfo()
        {
            return $"Имя: {FirstName}\nФамилия: {LastName}\nГод рождения: {BirthYear}\nВозраст: {Age}";
        }

        // Метод для проверки валидности профиля
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(FirstName) && 
                   !string.IsNullOrWhiteSpace(LastName) && 
                   BirthYear > 1900 && BirthYear <= DateTime.Now.Year;
        }
    }
}