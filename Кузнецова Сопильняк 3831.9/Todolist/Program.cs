// See https://aka.ms/new-console-template for more information
Console.WriteLine("Powered by Yarr and Angelina");

using System;

class Program
{
    static void Main()
    {
        // Запрос данных у пользователя
        Console.Write("Введите имя: ");
        string name = Console.ReadLine();
        
        Console.Write("Введите фамилию: ");
        string surname = Console.ReadLine();
        
        Console.Write("Введите год рождения: ");
        string yearInput = Console.ReadLine();
        
        // Преобразование года рождения в число
        int birthYear;
        while (!int.TryParse(yearInput, out birthYear))
        {
            Console.Write("Ошибка! Введите корректный год рождения: ");
            yearInput = Console.ReadLine();
        }
        
        // Получение текущего года
        int currentYear = DateTime.Now.Year;
        
        // Вычисление возраста
        int age = currentYear - birthYear;
        
        // Вывод результата
        Console.WriteLine($"Добро пожаловать пользователь {name} {surname}, возраст - {age}");
    }
}