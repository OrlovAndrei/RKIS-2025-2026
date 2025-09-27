using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Работу выполнили Должиков и Бут, группа 3834");

        Console.Write("Введите имя: ");
        string name = Console.ReadLine();

        Console.Write("Введите фамилию: ");
        string surname = Console.ReadLine();

        Console.Write("Введите год рождения: ");
        int year = int.Parse(Console.ReadLine());

        int age = DateTime.Now.Year - year;

        Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст – {age}");
    }
}
