using System;

class Program
{
    static void Main()
    {
        // Запрос данных
        Console.Write("Введите имя: ");
        string firstName = Console.ReadLine();

        Console.Write("Введите фамилию: ");
        string lastName = Console.ReadLine();

        Console.Write("Введите год рождения: ");
        string yearInput = Console.ReadLine();

        // Перевод года рождения
        int birthYear = int.Parse(yearInput);
        int currentYear = DateTime.Now.Year;
        int age = currentYear - birthYear;
        
        Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");
    }
}