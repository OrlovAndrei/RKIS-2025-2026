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

            string[] parts = input.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            string command = parts.Length > 0 ? parts[0].ToLower() : string.Empty;
            string args = parts.Length > 1 ? parts[1] : string.Empty;

            ICommand cmd = null;

            switch (command)
            {
                case "help":
                    PrintHelp();
                    break;

                case "profile":
                    cmd = new ProfileCommand(profile);
                    break;

                case "add":
                    cmd = new AddCommand(todoList, args);
                    break;

                case "view":
                    cmd = new ViewCommand(todoList, args);
                    break;

                case "read":
                    if (TryParseIndex(args, todoList.Count, out int readIndex))
                    {
                        cmd = new ReadCommand(todoList, readIndex);
                    }
                    break;

                case "done":
                    if (TryParseIndex(args, todoList.Count, out int doneIndex))
                    {
                        cmd = new DoneCommand(todoList, doneIndex);
                    }
                    break;

                case "delete":
                    if (TryParseIndex(args, todoList.Count, out int deleteIndex))
                    {
                        cmd = new DeleteCommand(todoList, deleteIndex);
                    }
                    break;

                case "update":
                    if (!string.IsNullOrWhiteSpace(args))
                    {
                        string[] parts = args.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 2 && int.TryParse(parts[0], out int updateIndex))
                        {
                            string newText = parts[1].Trim().Trim('"');
                            cmd = new UpdateCommand(todoList, updateIndex, newText);
                        }
                        else
                        {
                            Console.WriteLine("Ошибка: неверный формат. Пример: update 2 \"Новый текст\"");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: укажите индекс и новый текст. Пример: update 2 \"Новый текст\"");
                    }
                    break;

                case "exit":
                    Console.WriteLine("До свидания!");
                    return;

                default:
                    Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
                    break;
            }

            if (cmd != null)
            {
                cmd.Execute();
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
    static bool TryParseIndex(string arg, int taskCount, out int indexOneBased)
    {
        indexOneBased = -1;
        if (string.IsNullOrWhiteSpace(arg))
        {
            Console.WriteLine("Ошибка: укажите индекс задачи.");
            return false;
        }

        if (!int.TryParse(arg.Trim(), out int idxOneBased))
        {
            Console.WriteLine("Ошибка: индекс должен быть числом.");
            return false;
        }

        indexOneBased = idxOneBased;
        if (indexOneBased < 1 || indexOneBased > taskCount)
        {
            Console.WriteLine("Ошибка: индекс вне диапазона.");
            return false;
        }

        return true;
    }
}
