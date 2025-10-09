using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Введите имя:");
        string firstName = Console.ReadLine();

        Console.WriteLine("Введите фамилию:");
        string lastName = Console.ReadLine();

        Console.WriteLine("Введите год рождения:");
        int birthYear = int.Parse(Console.ReadLine());

        int currentYear = DateTime.Now.Year;
        int age = currentYear - birthYear;

        Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");
        Console.WriteLine("Работу выполнил Рублёв");
    }
}





