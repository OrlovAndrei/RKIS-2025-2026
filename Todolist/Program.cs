using System;
using System.IO;
using Todolist.Commands;
using Todolist.Exceptions;

class Program
{
    private static string dataDir = string.Empty;
    public static string ProfileFilePath { get; private set; } = string.Empty;
    public static string TodoFilePath { get; private set; } = string.Empty;

    private static void Main()
    {
        Console.WriteLine("Р”РѕР±СЂРѕ РїРѕР¶Р°Р»РѕРІР°С‚СЊ РІ РјРµРЅРµРґР¶РµСЂ Р·Р°РґР°С‡. Р”Р»СЏ РІС‹С…РѕРґР° РёСЃРїРѕР»СЊР·СѓР№С‚Рµ РєРѕРјР°РЅРґСѓ exit. Р“СЂСѓРїРїР° 3834");
        Console.WriteLine("РџРµСЂРµРґ РІР°РјРё РєРѕРЅСЃРѕР»СЊРЅРѕРµ РїСЂРёР»РѕР¶РµРЅРёРµ ToDoList. Р”Р»СЏ СЃРїРёСЃРєР° РєРѕРјР°РЅРґ РІРІРµРґРёС‚Рµ help.\n");
        
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
            Console.WriteLine($"РћС€РёР±РєР° РёРЅРёС†РёР°Р»РёР·Р°С†РёРё: {ex.Message}");
            Console.WriteLine("РќР°Р¶РјРёС‚Рµ Р»СЋР±СѓСЋ РєР»Р°РІРёС€Сѓ РґР»СЏ РІС‹С…РѕРґР°...");
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
                    Console.WriteLine($"РћС€РёР±РєР° СЃРѕС…СЂР°РЅРµРЅРёСЏ РґР°РЅРЅС‹С…: {ex.Message}");
                }
                Console.WriteLine("Р”Рѕ РІСЃС‚СЂРµС‡Рё!");
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
                cmd.Execute();

                if (cmd is IUndo undoable)
                {
                    AppInfo.UndoStack.Push(undoable);
                    AppInfo.RedoStack.Clear();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"РћС€РёР±РєР°: {ex.Message}");
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
            Console.WriteLine("РџРѕР¶Р°Р»СѓР№СЃС‚Р°, РІРІРµРґРёС‚Рµ С‡РёСЃР»Рѕ. РџРѕРїСЂРѕР±СѓР№С‚Рµ СЃРЅРѕРІР°.");
        }
    }

    private static void PrintHelp()
    {
        Console.WriteLine("Р”РѕСЃС‚СѓРїРЅС‹Рµ РєРѕРјР°РЅРґС‹:");
        Console.WriteLine(" help                         вЂ” РїРѕРєР°Р·Р°С‚СЊ РїРѕРјРѕС‰СЊ");
        Console.WriteLine(" profile                      вЂ” РїРѕРєР°Р·Р°С‚СЊ/СЃРјРµРЅРёС‚СЊ С‚РµРєСѓС‰РёР№ РїСЂРѕС„РёР»СЊ");
        Console.WriteLine(" add \"С‚РµРєСЃС‚\"                  вЂ” РґРѕР±Р°РІРёС‚СЊ Р·Р°РґР°С‡Сѓ (РѕРґРЅР° СЃС‚СЂРѕРєР°)");
        Console.WriteLine(" add --multiline / -m         вЂ” РґРѕР±Р°РІРёС‚СЊ Р·Р°РґР°С‡Сѓ (РјРЅРѕРіРѕСЃС‚СЂРѕС‡РЅС‹Р№ РІРІРѕРґ, РѕРєРѕРЅС‡Р°РЅРёРµ '!end')");
        Console.WriteLine(" view [flags]                 вЂ” РїРѕРєР°Р·Р°С‚СЊ Р·Р°РґР°С‡Рё (РїРѕ СѓРјРѕР»С‡Р°РЅРёСЋ С‚РѕР»СЊРєРѕ С‚РµРєСЃС‚)");
        Console.WriteLine("    Flags:");
        Console.WriteLine("      --index, -i       вЂ” РІС‹РІРѕРґРёС‚СЊ РёРЅРґРµРєСЃС‹");
        Console.WriteLine("      --status, -s      вЂ” РІС‹РІРѕРґРёС‚СЊ СЃС‚Р°С‚СѓСЃ");
        Console.WriteLine("      --update-date, -d вЂ” РІС‹РІРѕРґРёС‚СЊ РґР°С‚Сѓ РѕР±РЅРѕРІР»РµРЅРёСЏ");
        Console.WriteLine("      --all, -a         вЂ” РІС‹РІРµСЃС‚Рё РІСЃС‘");
        Console.WriteLine(" read <idx>                   вЂ” РїРѕРєР°Р·Р°С‚СЊ РїРѕР»РЅС‹Р№ С‚РµРєСЃС‚ Р·Р°РґР°С‡Рё Рё РµС‘ СЃС‚Р°С‚СѓСЃ");
        Console.WriteLine(" status <idx> <status>        вЂ” СЃРјРµРЅРёС‚СЊ СЃС‚Р°С‚СѓСЃ Р·Р°РґР°С‡Рё");
        Console.WriteLine("                               Р’РѕР·РјРѕР¶РЅС‹Рµ СЃС‚Р°С‚СѓСЃС‹: NotStarted, InProgress, Completed, Postponed, Failed");
        Console.WriteLine(" delete <idx>                 вЂ” СѓРґР°Р»РёС‚СЊ Р·Р°РґР°С‡Сѓ");
        Console.WriteLine(" update <idx> \"С‚РµРєСЃС‚\"         вЂ” РѕР±РЅРѕРІРёС‚СЊ С‚РµРєСЃС‚ Р·Р°РґР°С‡Рё");
        Console.WriteLine(" search [flags]               вЂ” РїРѕРёСЃРє Р·Р°РґР°С‡ (РЅРµ РёР·РјРµРЅСЏРµС‚ РґР°РЅРЅС‹Рµ)");
        Console.WriteLine("    Р¤Р»Р°РіРё С‚РµРєСЃС‚Р°: --contains \"С‚РµРєСЃС‚\", --starts-with \"С‚РµРєСЃС‚\", --ends-with \"С‚РµРєСЃС‚\"");
        Console.WriteLine("    РЎС‚Р°С‚СѓСЃ: --status <status>  Р”Р°С‚С‹ (yyyy-MM-dd): --from <date>, --to <date>");
        Console.WriteLine("    РЎРѕСЂС‚РёСЂРѕРІРєР°: --sort text|date, --desc  РћРіСЂР°РЅРёС‡РµРЅРёРµ: --top <n>");
        Console.WriteLine(" load <count> <size>          — параллельная загрузка с прогресс-барами");
        Console.WriteLine(" undo                         вЂ” РѕС‚РјРµРЅРёС‚СЊ РїРѕСЃР»РµРґРЅСЋСЋ РєРѕРјР°РЅРґСѓ");
        Console.WriteLine(" redo                         вЂ” РїРѕРІС‚РѕСЂРёС‚СЊ РѕС‚РјРµРЅС‘РЅРЅСѓСЋ РєРѕРјР°РЅРґСѓ");
        Console.WriteLine(" exit                         вЂ” РІС‹С…РѕРґ");
    }

    public static void SelectProfile()
    {
        while (true)
        {
            Console.Write("Р’РѕР№С‚Рё РїРѕРґ СЃСѓС‰РµСЃС‚РІСѓСЋС‰РёРј РїСЂРѕС„РёР»РµРј? [y/n]: ");
            string answer = Console.ReadLine()?.Trim().ToLowerInvariant() ?? string.Empty;

            if (answer == "y")
            {
                if (AppInfo.Profiles.Count == 0)
                {
                    Console.WriteLine("РџСЂРѕС„РёР»Рё РЅРµ РЅР°Р№РґРµРЅС‹. РЎРѕР·РґР°Р№С‚Рµ РЅРѕРІС‹Р№.");
                    try { CreateNewProfile(); }
                    catch (Exception ex) { Console.WriteLine($"РћС€РёР±РєР°: {ex.Message}"); continue; }
                }
                else
                {
                    if (!TryLogin())
                        continue;
                }
            }
            else if (answer == "n")
            {
                try { CreateNewProfile(); }
                catch (Exception ex) { Console.WriteLine($"РћС€РёР±РєР°: {ex.Message}"); continue; }
            }
            else
            {
                Console.WriteLine("Р’РІРµРґРёС‚Рµ 'y' РёР»Рё 'n'.\n");
                continue;
            }

            AppInfo.UndoStack.Clear();
            AppInfo.RedoStack.Clear();

            TodoFilePath = Path.Combine(dataDir, $"todos_{AppInfo.CurrentProfile.Id}.csv");

            try
            {
                if (!File.Exists(TodoFilePath))
                {
                    File.WriteAllText(TodoFilePath, string.Empty, System.Text.Encoding.UTF8);
                }
                AppInfo.Todos = FileManager.LoadTodos(TodoFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"РћС€РёР±РєР° РїСЂРё СЂР°Р±РѕС‚Рµ СЃ С„Р°Р№Р»РѕРј Р·Р°РґР°С‡: {ex.Message}");
                continue;
            }

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
        if (string.IsNullOrWhiteSpace(TodoFilePath)) return;
        try
        {
            FileManager.SaveTodos(AppInfo.Todos, TodoFilePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"РќРµ СѓРґР°Р»РѕСЃСЊ СЃРѕС…СЂР°РЅРёС‚СЊ Р·Р°РґР°С‡Рё РЅР° РґРёСЃРє: {ex.Message}");
        }
    }

    private static bool TryLogin()
    {
        string login = Prompt("Р›РѕРіРёРЅ: ") ?? string.Empty;
        string password = Prompt("РџР°СЂРѕР»СЊ: ") ?? string.Empty;

        var profile = AppInfo.Profiles.Find(p =>
            string.Equals(p.Login, login, StringComparison.OrdinalIgnoreCase) &&
            p.Password == password);

        if (profile == null)
        {
            Console.WriteLine("РќРµРІРµСЂРЅС‹Р№ Р»РѕРіРёРЅ РёР»Рё РїР°СЂРѕР»СЊ.\n");
            return false;
        }

        AppInfo.CurrentProfile = profile;
        Console.WriteLine($"\nР’С‹РїРѕР»РЅРµРЅ РІС…РѕРґ: {AppInfo.CurrentProfile.GetInfo()}\n");
        return true;
    }

    private static void CreateNewProfile()
    {
        Console.WriteLine("\nРЎРѕР·РґР°РЅРёРµ РЅРѕРІРѕРіРѕ РїСЂРѕС„РёР»СЏ.");
        string login = (Prompt("Р›РѕРіРёРЅ: ") ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(login))
            throw new InvalidArgumentException("Р›РѕРіРёРЅ РЅРµ РјРѕР¶РµС‚ Р±С‹С‚СЊ РїСѓСЃС‚С‹Рј.");
        if (AppInfo.Profiles.Exists(p => string.Equals(p.Login, login, StringComparison.OrdinalIgnoreCase)))
            throw new DuplicateLoginException("РџРѕР»СЊР·РѕРІР°С‚РµР»СЊ СЃ С‚Р°РєРёРј Р»РѕРіРёРЅРѕРј СѓР¶Рµ Р·Р°СЂРµРіРёСЃС‚СЂРёСЂРѕРІР°РЅ.");
        string password = Prompt("РџР°СЂРѕР»СЊ: ") ?? string.Empty;
        string firstName = Prompt("РРјСЏ: ") ?? string.Empty;
        string lastName = Prompt("Р¤Р°РјРёР»РёСЏ: ") ?? string.Empty;
        int birthYear = ReadInt("Р“РѕРґ СЂРѕР¶РґРµРЅРёСЏ: ");
        int currentYear = DateTime.Now.Year;
        if (birthYear < 1900 || birthYear > currentYear)
            throw new InvalidArgumentException($"Р“РѕРґ СЂРѕР¶РґРµРЅРёСЏ РґРѕР»Р¶РµРЅ Р±С‹С‚СЊ РІ РґРёР°РїР°Р·РѕРЅРµ 1900вЂ“{currentYear}.");
        var newProfile = new Profile(login, password, firstName, lastName, birthYear);
        AppInfo.Profiles.Add(newProfile);
        AppInfo.CurrentProfile = newProfile;
        FileManager.SaveProfiles(AppInfo.Profiles, ProfileFilePath);
        Console.WriteLine($"\nРџСЂРѕС„РёР»СЊ СЃРѕР·РґР°РЅ Рё РІС‹Р±СЂР°РЅ: {AppInfo.CurrentProfile.GetInfo()}\n");
    }
}
