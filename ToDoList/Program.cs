using System;
using System.Threading;

void PrintWithDelay(string text, int delay = 100)
{
    foreach (char c in text)
    {
        Console.Write(c);
        Thread.Sleep(delay);
    }
    Console.WriteLine();
}

void PrintAsciiArt()
{
    Console.WriteLine(@"
  _____         _       _     _     _ 
 |  __ \       | |     | |   (_)   | |
 | |  | | ___  | |_ ___| |__  _  __| |
 | |  | |/ _ \ | __/ __| '_ \| |/ _` |
 | |__| |  __/ | || (__| | | | | (_| |
 |_____/ \___|  \__\___|_| |_|_|\__,_|
");
}

void PrintLoadingAnimation()
{
    string loading = "Загрузка системы";
    for (int i = 0; i < 3; i++)
    {
        Console.Write(loading);
        for (int dots = 0; dots < 3; dots++)
        {
            Console.Write(".");
            Thread.Sleep(300);
        }
        Thread.Sleep(300);
        Console.Clear();
    }
}

void PrintGoodbyeAnimation()
{
    string goodbye = "До свидания!";
    for (int i = 0; i < goodbye.Length; i++)
    {
        Console.Write(goodbye[i]);
        Thread.Sleep(150);
    }
    Console.WriteLine();
}

void PrintScaryAnimation()
{
    string[] frames = new string[]
    {
        @"
  .-.      .-.      .-.      .-.      .-.
 (o o)    (o o)    (o o)    (o o)    (o o)
 | O \    | O \    | O \    | O \    | O \
  \   \   \   \   \   \   \   \   \   \   \
   `~~~`   `~~~`   `~~~`   `~~~`   `~~~`   ",
        @"
  .-.      .-.      .-.      .-.      .-.
 (O O)    (O O)    (O O)    (O O)    (O O)
 | o /    | o /    | o /    | o /    | o /
  \   \   \   \   \   \   \   \   \   \   \
   `~~~`   `~~~`   `~~~`   `~~~`   `~~~`   "
    };

    for (int i = 0; i < 6; i++)
    {
        Console.Clear();
        Console.WriteLine(frames[i % 2]);
        Thread.Sleep(400);
    }
    Console.Clear();
}

PrintLoadingAnimation();
PrintAsciiArt();
Thread.Sleep(500);
Console.Clear();

Console.WriteLine("Задание выполнено Ждановым и Емелиным");

Console.Write("Введите ваше имя: ");
string? firstName = Console.ReadLine();

if (firstName == null || firstName == "")
{
    throw new Exception("Вы не ввели ваше имя!");
}

Console.Write("Введите вашу фамилию: ");
string? lastName = Console.ReadLine();

if (lastName == null || lastName == "")
{
    throw new Exception("Вы не ввели фамилию!");
}

Console.Write("Введите ваш год рождения: ");
string? yearBirthString = Console.ReadLine();

if (yearBirthString == null || yearBirthString == "")
{
    throw new Exception("Вы не ввели год рождения!");
}

int yearBirth;

if (!int.TryParse(yearBirthString, out yearBirth))
{
    throw new Exception("Вы ввели не цифровое значение! - требуется год рождения.");
}

if (yearBirth < 1800 || yearBirth > DateTime.Now.Year)
{
    throw new Exception("Вы ввели некорректный год рождения!");
}

int age = DateTime.Now.Year - yearBirth;

Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст – {age}");

string[] todos = new string[2];

bool work = true;

while (work)
{

    Console.Write("\nвведите комманду:");
    Console.Write("\nдля помощи используйте help: ");
    string? command = Console.ReadLine();

    if (command == null)
    {
        continue;
    }

    string trimmedCommand = command.Trim();

    switch (trimmedCommand)
    {
        case "help":
            Console.WriteLine("---Список доступных комманд---");
            Console.WriteLine("help - выводит текущее сообщение");
            Console.WriteLine("profile - выводит данные пользователя");
            Console.WriteLine("add - добавляет новую задачу (add текст задачи)");
            Console.WriteLine("view - выводит все задачи");
            Console.WriteLine("delete - удаляет профиль пользователя после подтверждения");
            Console.WriteLine("blackout - удаляет всех пользователей после подтверждения кодом доступа");
            Console.WriteLine("exit - заверешние программы");
            break;

        case "profile":
            PrintWithDelay($"Профиль: {firstName} {lastName}, {yearBirth}");
            break;

        case "exit":
            PrintGoodbyeAnimation();
            work = false;
            break;

        case "view":
            Console.WriteLine("---Список задач---");
            bool anyTask = false;
            foreach (string task in todos)
            {
                if (task != null)
                {
                    Console.WriteLine(task);
                    anyTask = true;
                }
            }
            if (!anyTask)
            {
                Console.WriteLine("Задачи отсутствуют.");
            }
            break;

        case "delete":
            Console.Write("Вы действительно хотите удалить профиль? (да/нет): ");
            string? confirmDelete = Console.ReadLine();
            if (confirmDelete != null && confirmDelete.ToLower() == "да")
            {
                PrintWithDelay("Удаление профиля...");
                Thread.Sleep(1000);
                firstName = null;
                lastName = null;
                yearBirth = 0;
                age = 0;
                todos = new string[2];
                PrintWithDelay("Профиль удален.");
                work = false;
            }
            else
            {
                PrintWithDelay("Удаление отменено.");
            }
            break;

        case "blackout":
            Console.WriteLine("ВНИМАНИЕ! Вы собираетесь удалить всех пользователей!");
            Console.Write("Введите код доступа для подтверждения: ");
            string? accessCode = Console.ReadLine();
            if (accessCode == "0802")
            {
                PrintScaryAnimation();
                PrintWithDelay("Все пользователи удалены...");
                firstName = null;
                lastName = null;
                yearBirth = 0;
                age = 0;
                todos = new string[2];
                work = false;
            }
            else
            {
                PrintWithDelay("Неверный код доступа! Операция отменена.");
            }
            break;
    }

    if (trimmedCommand.StartsWith("add "))
    {
        var s = command.Split(" ", 2);

        var element = -1;

        for (int i = 0; i < todos.Length; i++)
        {
            if (todos[i] == null)
            {
                element = i;
                break;
            }
        }

        if (element == -1)
        {
            string[] newtodos = new string[todos.Length * 2];

            for (int i = 0; i < todos.Length; i++)
            {
                newtodos[i] = todos[i];
            }
            element = todos.Length;

            todos = newtodos;
        }

        todos[element] = s[1];
        PrintWithDelay("Задача добавлена!");
    }
}
