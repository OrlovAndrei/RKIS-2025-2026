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

        Profile profile;
        TodoList todoList;
        
        try
        {
            // Initialize data paths using Path.Combine
            dataDir = Path.Combine(Directory.GetCurrentDirectory(), "data");
            ProfileFilePath = Path.Combine(dataDir, "profile.txt");
            TodoFilePath = Path.Combine(dataDir, "todo.csv");
            
            // Ensure data directory exists and create empty files if needed
            FileManager.EnsureDataDirectory(dataDir);
            
            // Загрузка профиля и создание нового при необходимости
            var loadedProfile = FileManager.LoadProfile(ProfileFilePath);
            if (loadedProfile != null)
            {
                profile = loadedProfile;
                Console.WriteLine($"Загружен профиль: {profile.GetInfo()}\n");
            }
            else
            {
                string firstName = Prompt("Введите имя: ") ?? string.Empty;
                string lastName = Prompt("Введите фамилию: ") ?? string.Empty;
                int birthYear = ReadInt("Введите год рождения: ");
                
                profile = new Profile(firstName, lastName, birthYear);
                FileManager.SaveProfile(profile, ProfileFilePath);
                Console.WriteLine($"\nПрофиль создан и сохранён: {profile.GetInfo()}\n");
            }

            // Загрузка списка задач или создание пустого списка
            todoList = FileManager.LoadTodos(TodoFilePath);
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
                    FileManager.SaveProfile(profile, ProfileFilePath);
                    FileManager.SaveTodos(todoList, TodoFilePath);
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
                var cmd = CommandParser.Parse(input, todoList, profile);
                if (cmd != null)
                {
                    cmd.Execute();
                    // Обновляем profile после выполнения команды (если команда могла его изменить)
                    if (cmd is ProfileCommand pc)
                    {
                        profile = pc.Profile;
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
        Console.WriteLine(" done <idx>                   — отметить задачу выполненной (idx — номер задачи)");
        Console.WriteLine(" delete <idx>                 — удалить задачу по индексу");
        Console.WriteLine(" update <idx> \"новый\"         — обновить текст задачи");
        Console.WriteLine(" exit                         — выйти");
    }
}
