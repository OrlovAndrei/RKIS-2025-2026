using System;

namespace ToddList
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Бурнашов и Хазиев");

            Console.Write("Введите имя: ");
            string firstName = Console.ReadLine();

            Console.Write("Введите фамилию: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите год рождения: ");
            string yearInput = Console.ReadLine();

            int birthYear = int.Parse(yearInput);
            int currentYear = DateTime.Now.Year;
            int age = currentYear - birthYear;

            string[] todos = new string[2];

            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст – {age}");
        }
    }
}