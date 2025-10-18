using System;

class Program
{
    private const int InitialTaskArraySize = 2;

    static void Main()
    {
        Console.WriteLine("Работу выполнили: Должиков и Бут, группа 3834");
        Console.WriteLine("Консольный ToDoList — тестовая версия.\n");

        string userFirstName = Prompt("Введите имя: ");
        string userLastName  = Prompt("Введите фамилию: ");
        int birthYear        = ReadInt("Введите год рождения: ");

        int userAge = DateTime.Now.Year - birthYear;
        Console.WriteLine($"\nПрофиль создан: {userFirstName} {userLastName}, возраст – {userAge}\n");

        // Основной массив задач (пока только один массив, дальнейшие этапы добавят другие)
        string[] todos = new string[InitialTaskArraySize];
        int taskCount = 0;

        Console.WriteLine("Введите команду (help для списка команд).");

        while (true)
        {
            Console.Write("\n>>> ");
            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                continue;

            string command = ExtractCommand(input);

            if (command == "help")
            {
                PrintHelp();
                continue;
            }

            if (command == "profile")
            {
                PrintProfile(userFirstName, userLastName, birthYear);
                continue;
            }

            if (command == "add")
            {
                HandleAddSimple(ref todos, ref taskCount, input);
                continue;
            }

            if (command == "view")
            {
                HandleViewSimple(todos, taskCount);
                continue;
            }

            if (command == "exit")
            {
                Console.WriteLine("До свидания!");
                break;
            }

            Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
        }
    }

    // --- Утилиты и выделённые методы ---

    static string Prompt(string text)
    {
        Console.Write(text);
        return Console.ReadLine();
    }

    static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string s = Console.ReadLine();
            if (int.TryParse(s, out int v))
                return v;
            Console.WriteLine("Неверный ввод. Попробуйте ещё раз.");
        }
    }

    static string ExtractCommand(string input)
    {
        string[] parts = input.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        return parts.Length > 0 ? parts[0].ToLower() : string.Empty;
    }

    static void PrintHelp()
    {
        Console.WriteLine("Доступные команды:");
        Console.WriteLine(" help    — список команд");
        Console.WriteLine(" profile — показать профиль пользователя");
        Console.WriteLine(" add \"текст\" — добавить задачу");
        Console.WriteLine(" view    — показать список задач");
        Console.WriteLine(" exit    — выйти");
    }

    static void PrintProfile(string firstName, string lastName, int birthYear)
    {
        Console.WriteLine($"{firstName} {lastName}, {birthYear} г.р.");
    }

    // Простой add/view (пока без статусов и дат)
    static void HandleAddSimple(ref string[] todos, ref int taskCount, string input)
    {
        string[] parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
        {
            Console.WriteLine("Ошибка: укажите текст задачи. Пример: add \"Сделать задание\"");
            return;
        }

        string taskText = parts[1].Trim().Trim('"');

        if (taskCount >= todos.Length)
        {
            ExpandArray(ref todos);
        }

        todos[taskCount++] = taskText;
        Console.WriteLine($"Задача добавлена: \"{taskText}\"");
    }

    static void HandleViewSimple(string[] todos, int taskCount)
    {
        Console.WriteLine("Ваши задачи:");
        if (taskCount == 0)
        {
            Console.WriteLine(" (список пуст)");
            return;
        }

        for (int i = 0; i < taskCount; i++)
        {
            if (!string.IsNullOrWhiteSpace(todos[i]))
                Console.WriteLine($" {i + 1}. {todos[i]}");
        }
    }

    static void ExpandArray(ref string[] array)
    {
        int newSize = array.Length * 2;
        string[] newArray = new string[newSize];
        for (int i = 0; i < array.Length; i++)
            newArray[i] = array[i];
        array = newArray;
    }
}
