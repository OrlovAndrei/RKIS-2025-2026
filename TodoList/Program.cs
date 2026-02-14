using TodoList.commands;

namespace TodoList;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("=== Todo List Application ===");
        Console.WriteLine("Практическую работу 11 сделали: Фоменко и Мартиросьян");
        
        FileManager.EnsureAllData();
        AppInfo.Profiles = FileManager.LoadProfiles();
        
        // Шаг авторизации
        if (!LoginUser())
            return;
        
        Console.WriteLine("Введите 'help' для списка команд");
        
        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                continue;
            
            var command = CommandParser.Parse(input);
            command.Execute();
            
            // Сохраняем задачи текущего пользователя
            if (AppInfo.CurrentProfileId.HasValue)
                FileManager.SaveTodos(AppInfo.CurrentProfileId.Value, AppInfo.GetCurrentTodoList());
        }
    }
    
    private static bool LoginUser()
    {
        while (true)
        {
            Console.WriteLine("Войти в существующий профиль? [y/n]");
            Console.Write("> ");
            var choice = Console.ReadLine()?.ToLower();
            
            if (choice == "y")
            {
                return LoginExistingUser();
            }
            else if (choice == "n")
            {
                return CreateNewUser();
            }
            else
            {
                Console.WriteLine("Некорректный выбор. Попробуйте снова.");
            }
        }
    }
    
    private static bool LoginExistingUser()
    {
        Console.Write("Логин: ");
        var login = Console.ReadLine();
        Console.Write("Пароль: ");
        var password = Console.ReadLine();
        
        var profile = AppInfo.Profiles.FirstOrDefault(p => p.Login == login && p.CheckPassword(password));
        
        if (profile == null)
        {
            Console.WriteLine("Неверный логин или пароль.");
            return false;
        }
        
        AppInfo.CurrentProfileId = profile.Id;
        AppInfo.TodosByUser[profile.Id] = FileManager.LoadTodos(profile.Id);
        AppInfo.UndoStack.Clear();
        AppInfo.RedoStack.Clear();
        
        Console.WriteLine($"Добро пожаловать, {profile.FirstName} {profile.LastName}!");
        return true;
    }
    
    private static bool CreateNewUser()
    {
        Console.WriteLine("Создание нового профиля:");
        
        Console.Write("Логин: ");
        var login = Console.ReadLine();
        
        if (AppInfo.Profiles.Any(p => p.Login == login))
        {
            Console.WriteLine("Пользователь с таким логином уже существует.");
            return false;
        }
        
        Console.Write("Пароль: ");
        var password = Console.ReadLine();
        Console.Write("Имя: ");
        var firstName = Console.ReadLine();
        Console.Write("Фамилия: ");
        var lastName = Console.ReadLine();
        Console.Write("Год рождения: ");
        
        if (!int.TryParse(Console.ReadLine(), out var birthYear))
        {
            Console.WriteLine("Некорректный год рождения.");
            return false;
        }
        
        var profile = new Profile(login, password, firstName, lastName, birthYear);
        AppInfo.Profiles.Add(profile);
        FileManager.SaveProfiles(AppInfo.Profiles);
        
        AppInfo.CurrentProfileId = profile.Id;
        AppInfo.TodosByUser[profile.Id] = new TodoList();
        AppInfo.UndoStack.Clear();
        AppInfo.RedoStack.Clear();
        
        Console.WriteLine($"Профиль создан! Добро пожаловать, {firstName} {lastName}!");
        return true;
    }
}