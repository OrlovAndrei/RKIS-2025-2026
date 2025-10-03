using System;

class Program
{
        // Массив в 2 элемента
        static string[] todos = new string[2];
    static string firstName = "";
    static string lastName = "";
    static int birthYear = 0;
    static void Main()
    {
        bool isValid = true;
        int currentYear = DateTime.Now.Year;

        Console.Write("Работу сделали Приходько и Бочкарёв\n");
        Console.Write("Введите свое имя: ");
        string? name = Console.ReadLine();
        Console.Write("Введите свою фамилию: ");
        string? secondName = Console.ReadLine();
        Console.Write("Введите свой год рождения: ");
        int birthYear = 0;

        try
        {
            birthYear = int.Parse(Console.ReadLine());
        }
        catch (Exception)
        {
            isValid = false;
        }


        if ((isValid == true) && (birthYear <= currentYear))
        {
            int age = currentYear - birthYear;
            Console.WriteLine($"Добавлен пользователь:{name},{secondName},возраст - {age}");
        }
        else
        {
            Console.WriteLine("Неверно введен год рождения"); // это ошибка, которая выводится при неправильном введении года рождения, например если введут "арбуз" или введут год больше 2025
        }
    }
}
