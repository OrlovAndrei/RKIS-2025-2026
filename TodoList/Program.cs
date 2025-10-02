using System;

class TodoList
{
    static void Main()
    {
        Console.WriteLine("выполнил работу Турищев Иван");
        int yaerNow = DateTime.Now.Year;
        System.Console.WriteLine(yaerNow);
        System.Console.Write("Введите ваше имя: ");
        string userName = Console.ReadLine() ?? "Неизвестно";
        if (userName.Length > 0) userName = "Неизвестно";
        System.Console.Write($"{userName}, введите год вашего рождения: ");
        string yaerBirth = Console.ReadLine() ?? "Неизвестно";
        if (yaerBirth == "") yaerBirth = "Неизвестно";
        int age = -1;
        if (int.TryParse(yaerBirth, out age) && age < yaerNow)
        {
            System.Console.WriteLine($"Добавлен пользователь {userName}, возрастом {yaerNow-age}");
        }
        else System.Console.WriteLine("Пользователь не ввел возраст");
    }
}