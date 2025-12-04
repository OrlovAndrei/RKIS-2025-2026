using System;
using System.IO;
using Todolist.Commands;

class Program
{
    private static string dataDir = string.Empty;
    
    // Пути к файлам данных (статические, чтобы команды могли их использовать)
    public static string ProfileFilePath { get; private set; } = string.Empty;
    public static string TodoFilePath { get; private set; } = string.Empty;

    private static void Main()
    {
        Console.WriteLine("Работу выполнили: Должиков и Бут, группа 3834");
        Console.WriteLine("Консольный ToDoList — полнофункциональная версия.\n");
        
        try
        {
            // Initialize data paths using Path.Combine
            dataDir = Path.Combine(Directory.GetCurrentDirectory(), "data");
            ProfileFilePath = Path.Combine(dataDir, "profile.csv");
            
            // Ensure data directory exists and create empty files if needed
            FileManager.EnsureDataDirectory(dataDir);
            
            // Загрузка всех профилей из файла
            AppInfo.Profiles = FileManager.LoadProfiles(ProfileFilePath);

            // Выбор или создание профиля и загрузка его задач
            SelectProfile();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Критическая ошибка при инициализации: {ex.Message}");
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
            return;
        }

        // Основной цикл обработки команд
        while (true)
        {
            Console.Write("\n>>> ");
            string? input = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(input))
                continue;

            // Специальные команды, не требующие парсинга
            string[] parts = input.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            string command = parts.Length > 0 ? parts[0].ToLower() : string.Empty;

            if (command == "exit")
            {
                // Сохраняем данные перед выходом
                try 
                {
                    FileManager.SaveProfiles(AppInfo.Profiles, ProfileFilePath);
                    FileManager.SaveTodos(AppInfo.Todos, TodoFilePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Предупреждение: не удалось сохранить данные при выходе: {ex.Message}");
                }
                Console.WriteLine("До свидания!");
                return;
            }

            if (command == "help")
            {
                PrintHelp();
                continue;
            }

            // Основной поток: парсинг и выполнение команды
            try
            {
                var cmd = CommandParser.Parse(input);
                if (cmd != null)
                {
                    cmd.Execute();
                    
                    // Сохраняем команду в undoStack, если она изменяет данные
                    if (cmd is AddCommand || cmd is DeleteCommand || cmd is UpdateCommand || 
                        cmd is StatusCommand || cmd is ProfileCommand)
                    {
                        AppInfo.UndoStack.Push(cmd);
                        // Очищаем redoStack при новом действии
                        AppInfo.RedoStack.Clear();
                    }
                }
                else
                {
                    Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выполнении команды: {ex.Message}");
            }
        }
    }

    // --- Вспомогательные методы ввода/валидации ---
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
            Console.WriteLine("Неверный ввод. Попробуйте ещё раз.");
        }
    }

    private static void PrintHelp()
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
        Console.WriteLine(" status <idx> <status>        — изменить статус задачи");
        Console.WriteLine("                               Доступные статусы: NotStarted, InProgress, Completed, Postponed, Failed");
        Console.WriteLine(" delete <idx>                 — удалить задачу по индексу");
        Console.WriteLine(" update <idx> \"новый\"         — обновить текст задачи");
        Console.WriteLine(" undo                         — отменить последнее действие");
        Console.WriteLine(" redo                         — повторить последнее отменённое действие");
        Console.WriteLine(" exit                         — выйти");
    }

    /// <summary>
    /// Выбор или создание профиля и загрузка его задач.
    /// </summary>
    public static void SelectProfile()
    {
        while (true)
        {
            Console.Write("Войти в существующий профиль? [y/n]: ");
            string answer = Console.ReadLine()?.Trim().ToLowerInvariant() ?? string.Empty;

            if (answer == "y")
            {
                if (AppInfo.Profiles.Count == 0)
                {
                    Console.WriteLine("Пока нет ни одного профиля. Создадим новый.");
                    CreateNewProfile();
                }
                else
                {
                    if (!TryLogin())
                        continue; // не удалось войти — снова спрашиваем
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

            // К этому моменту выбран текущий профиль
            // Сбрасываем undo/redo для нового профиля
            AppInfo.UndoStack.Clear();
            AppInfo.RedoStack.Clear();

            // Путь к файлу задач текущего профиля
            TodoFilePath = Path.Combine(dataDir, $"todos_{AppInfo.CurrentProfile.Id}.csv");

            // Если файла нет — создаём пустой
            if (!File.Exists(TodoFilePath))
            {
                File.WriteAllText(TodoFilePath, string.Empty);
            }

            // Загрузка списка задач (они автоматически привязываются к текущему профилю через AppInfo.Todos)
            AppInfo.Todos = FileManager.LoadTodos(TodoFilePath);
            return;
        }
    }

    private static bool TryLogin()
    {
        string login = Prompt("Введите логин: ") ?? string.Empty;
        string password = Prompt("Введите пароль: ") ?? string.Empty;

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
        string login = Prompt("Введите логин: ") ?? string.Empty;
        string password = Prompt("Введите пароль: ") ?? string.Empty;
        string firstName = Prompt("Введите имя: ") ?? string.Empty;
        string lastName = Prompt("Введите фамилию: ") ?? string.Empty;
        int birthYear = ReadInt("Введите год рождения: ");

        var newProfile = new Profile(login, password, firstName, lastName, birthYear);
        AppInfo.Profiles.Add(newProfile);
        AppInfo.CurrentProfile = newProfile;

        FileManager.SaveProfiles(AppInfo.Profiles, ProfileFilePath);
        Console.WriteLine($"\nПрофиль создан и сохранён: {AppInfo.CurrentProfile.GetInfo()}\n");
    }
}
