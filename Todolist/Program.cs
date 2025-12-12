using System;
using System.IO;
using Todolist.Commands;

class Program
{
    private static string dataDir = string.Empty;
    public static string ProfileFilePath { get; private set; } = string.Empty;
    public static string TodoFilePath { get; private set; } = string.Empty;

    private static void Main()
    {
        Console.WriteLine("Добро пожаловать в менеджер задач. Для выхода используйте команду exit. Группа 3834");
        Console.WriteLine("Перед вами консольное приложение ToDoList. Для списка команд введите help.\n");
        
        try
        {
            dataDir = Path.Combine(Directory.GetCurrentDirectory(), "data");
            ProfileFilePath = Path.Combine(dataDir, "profile.csv");

            FileManager.EnsureDataDirectory(dataDir);
            AppInfo.Profiles = FileManager.LoadProfiles(ProfileFilePath);

            SelectProfile();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка инициализации: {ex.Message}");
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
            return;
        }

        while (true)
        {
            Console.Write("\n>>> ");
            string? input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input))
                continue;

            string[] parts = input.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            string command = parts.Length > 0 ? parts[0].ToLower() : string.Empty;

            if (command == "exit")
            {
                try 
                {
                    FileManager.SaveProfiles(AppInfo.Profiles, ProfileFilePath);
                    FileManager.SaveTodos(AppInfo.Todos, TodoFilePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка сохранения данных: {ex.Message}");
                }
                Console.WriteLine("До встречи!");
                return;
            }

            if (command == "help")
            {
                PrintHelp();
                continue;
            }

            try
            {
                var cmd = CommandParser.Parse(input);
                if (cmd != null)
                {
                    cmd.Execute();
                    
                    if (cmd is AddCommand || cmd is DeleteCommand || cmd is UpdateCommand || 
                        cmd is StatusCommand || cmd is ProfileCommand)
                    {
                        AppInfo.UndoStack.Push(cmd);
                        AppInfo.RedoStack.Clear();
                    }
                }
                else
                {
                    Console.WriteLine("Неизвестная команда. Введите 'help' для подсказки.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка выполнения команды: {ex.Message}");
            }
        }
    }

    public static string Prompt(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine() ?? string.Empty;
    }

    public static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var s = Console.ReadLine();
            if (int.TryParse(s, out int v))
                return v;
            Console.WriteLine("Пожалуйста, введите число. Попробуйте снова.");
        }
    }

    private static void PrintHelp()
    {
        Console.WriteLine("Доступные команды:");
        Console.WriteLine(" help                         — показать помощь");
        Console.WriteLine(" profile                      — показать/сменить текущий профиль");
        Console.WriteLine(" add \"текст\"                  — добавить задачу (одна строка)");
        Console.WriteLine(" add --multiline / -m         — добавить задачу (многострочный ввод, окончание '!end')");
        Console.WriteLine(" view [flags]                 — показать задачи (по умолчанию только текст)");
        Console.WriteLine("    Flags:");
        Console.WriteLine("      --index, -i       — выводить индексы");
        Console.WriteLine("      --status, -s      — выводить статус");
        Console.WriteLine("      --update-date, -d — выводить дату обновления");
        Console.WriteLine("      --all, -a         — вывести всё");
        Console.WriteLine(" read <idx>                   — показать полный текст задачи и её статус");
        Console.WriteLine(" status <idx> <status>        — сменить статус задачи");
        Console.WriteLine("                               Возможные статусы: NotStarted, InProgress, Completed, Postponed, Failed");
        Console.WriteLine(" delete <idx>                 — удалить задачу");
        Console.WriteLine(" update <idx> \"текст\"         — обновить текст задачи");
        Console.WriteLine(" undo                         — отменить последнюю команду");
        Console.WriteLine(" redo                         — повторить отменённую команду");
        Console.WriteLine(" exit                         — выход");
    }

    public static void SelectProfile()
    {
        while (true)
        {
            Console.Write("Войти под существующим профилем? [y/n]: ");
            string answer = Console.ReadLine()?.Trim().ToLowerInvariant() ?? string.Empty;

            if (answer == "y")
            {
                if (AppInfo.Profiles.Count == 0)
                {
                    Console.WriteLine("Профили не найдены. Создайте новый.");
                    CreateNewProfile();
                }
                else
                {
                    if (!TryLogin())
                        continue;
                }
            }
            else if (answer == "n")
            {
                CreateNewProfile();
            }
            else
            {
                Console.WriteLine("Введите 'y' или 'n'.\n");
                continue;
            }

            AppInfo.UndoStack.Clear();
            AppInfo.RedoStack.Clear();

            TodoFilePath = Path.Combine(dataDir, $"todos_{AppInfo.CurrentProfile.Id}.csv");

            if (!File.Exists(TodoFilePath))
            {
                File.WriteAllText(TodoFilePath, string.Empty);
            }

            AppInfo.Todos = FileManager.LoadTodos(TodoFilePath);
            AttachTodoEventHandlers(AppInfo.Todos);
            return;
        }
    }

    private static void AttachTodoEventHandlers(TodoList todos)
    {
        todos.OnTodoAdded -= HandleTodoChanged;
        todos.OnTodoDeleted -= HandleTodoChanged;
        todos.OnTodoUpdated -= HandleTodoChanged;
        todos.OnStatusChanged -= HandleTodoChanged;

        todos.OnTodoAdded += HandleTodoChanged;
        todos.OnTodoDeleted += HandleTodoChanged;
        todos.OnTodoUpdated += HandleTodoChanged;
        todos.OnStatusChanged += HandleTodoChanged;
    }

    private static void HandleTodoChanged(TodoItem _)
    {
        if (!string.IsNullOrWhiteSpace(TodoFilePath))
        {
            FileManager.SaveTodos(AppInfo.Todos, TodoFilePath);
        }
    }

    private static bool TryLogin()
    {
        string login = Prompt("Логин: ") ?? string.Empty;
        string password = Prompt("Пароль: ") ?? string.Empty;

        var profile = AppInfo.Profiles.Find(p =>
            string.Equals(p.Login, login, StringComparison.OrdinalIgnoreCase) &&
            p.Password == password);

        if (profile == null)
        {
            Console.WriteLine("Неверный логин или пароль.\n");
            return false;
        }

        AppInfo.CurrentProfile = profile;
        Console.WriteLine($"\nВыполнен вход: {AppInfo.CurrentProfile.GetInfo()}\n");
        return true;
    }

    private static void CreateNewProfile()
    {
        Console.WriteLine("\nСоздание нового профиля.");
        string login = Prompt("Логин: ") ?? string.Empty;
        string password = Prompt("Пароль: ") ?? string.Empty;
        string firstName = Prompt("Имя: ") ?? string.Empty;
        string lastName = Prompt("Фамилия: ") ?? string.Empty;
        int birthYear = ReadInt("Год рождения: ");

        var newProfile = new Profile(login, password, firstName, lastName, birthYear);
        AppInfo.Profiles.Add(newProfile);
        AppInfo.CurrentProfile = newProfile;

        FileManager.SaveProfiles(AppInfo.Profiles, ProfileFilePath);
        Console.WriteLine($"\nПрофиль создан и выбран: {AppInfo.CurrentProfile.GetInfo()}\n");
    }
}

