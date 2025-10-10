using System;
using System.Text;

namespace TodoList
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("Работу выполнили Буряк Степан Геннадьевич и Голубев Данил Сергеевич");

            Console.Write("Введите имя: ");
            string? firstName = Console.ReadLine();

            Console.Write("Введите фамилию: ");
            string? lastName = Console.ReadLine();

            Console.Write("Введите год рождения: ");
            string? birthYearInput = Console.ReadLine();

            if (!int.TryParse(birthYearInput, out int birthYear))
            {
                Console.WriteLine("Некорректный год рождения. Пожалуйста, введите число.");
                return;
            }

            int currentYear = DateTime.Now.Year;
            int age = currentYear - birthYear;

            firstName = string.IsNullOrWhiteSpace(firstName) ? "Имя" : firstName.Trim();
            lastName = string.IsNullOrWhiteSpace(lastName) ? "Фамилия" : lastName.Trim();

            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");
        }
    }
}
