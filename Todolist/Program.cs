using System;

class Program
{

    static void Main()
    {
        Console.WriteLine("Работу выполнили: Должиков и Бут, группа 3834");
        Console.WriteLine("Консольный ToDoList — полнофункциональная версия.\n");

        string firstName = Prompt("Введите имя: ") ?? string.Empty;
        string lastName  = Prompt("Введите фамилию: ") ?? string.Empty;
        int birthYear    = ReadInt("Введите год рождения: ");
        
        Profile profile = new Profile(firstName, lastName, birthYear);
        Console.WriteLine($"\nПрофиль создан: {profile.GetInfo()}\n");

        TodoList todoList = new TodoList();

        Console.WriteLine("Введите команду (help для списка команд).");

        while (true)
        {
            Console.Write("\n>>> ");
            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                continue;

            // Специальные команды, не требующие парсинга
            string[] parts = input.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            string command = parts.Length > 0 ? parts[0].ToLower() : string.Empty;

            if (command == "exit")
            {
                Console.WriteLine("До свидания!");
                return;
            }

            if (command == "help")
            {
                PrintHelp();
                continue;
            }

            // Основной поток: парсинг и выполнение команды
            var cmd = CommandParser.Parse(input, todoList, profile);
            if (cmd != null)
            {
                cmd.Execute();
            }
            else
            {
                Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
            }
        }
    }

    // --- Вспомогательные методы ввода/валидации ---
    static string Prompt(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }

    static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var s = Console.ReadLine();
            if (int.TryParse(s, out int v))
                return v;
            Console.WriteLine("Неверный ввод. Попробуйте ещё раз.");
        }
    }

    static void PrintHelp()
    {
        Console.WriteLine("Доступные команды:");
        Console.WriteLine(" help                         — список команд");
        Console.WriteLine(" profile                      — показать профиль пользователя");
        Console.WriteLine(" add \"текст\"                  — добавить задачу (однострочно)");
        Console.WriteLine(" add --multiline / -m         — добавить задачу (многострочно, завершить ввод командой !end)");
        Console.WriteLine(" view [flags]                 — показать задачи (по умолчанию — только текст)");
        Console.WriteLine("    Flags:");
        Console.WriteLine("      --index, -i       — показывать индекс задачи");
        Console.WriteLine("      --status, -s      — показывать статус задачи");
        Console.WriteLine("      --update-date, -d — показывать дату последнего изменения");
        Console.WriteLine("      --all, -a         — показывать все поля одновременно");
        Console.WriteLine(" read <idx>                   — показать полный текст задачи, статус и дату изменения");
        Console.WriteLine(" done <idx>                   — отметить задачу выполненной (idx — номер задачи)");
        Console.WriteLine(" delete <idx>                 — удалить задачу по индексу");
        Console.WriteLine(" update <idx> \"новый\"         — обновить текст задачи");
        Console.WriteLine(" exit                         — выйти");
    }

    // --- Вспомогательные методы ---
}
