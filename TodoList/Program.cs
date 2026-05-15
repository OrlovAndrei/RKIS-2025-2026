using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

class Program
{
    private static IProfileRepository _profileRepo = new ProfileRepository();
    private static ITodoRepository _todoRepo = new TodoRepository();

    static async Task Main(string[] args)
    {
        try
        {
            using (var db = new AppDbContext())
            {
                await db.Database.MigrateAsync();
            }

            var profiles = await _profileRepo.GetAllAsync();
            AppInfo.Profiles = profiles;
            Console.WriteLine($"Система готова. Загружено профилей: {AppInfo.Profiles.Count}");

            if (!await SelectOrCreateProfile())
            {
                Console.WriteLine("Выход из программы.");
                return;
            }

            RunTodoApplication();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Критическая ошибка при запуске: {ex.Message}");
        }
    }

    static async Task<bool> SelectOrCreateProfile()
    {
        Console.WriteLine("\n--- Меню входа ---");

        if (AppInfo.Profiles.Count > 0)
        {
            Console.Write("Войти в существующий профиль? [y/n]: ");
            string choice = Console.ReadLine()?.ToLower();
            if (choice == "y") return await LoginProfile();
        }

        return await CreateNewProfile();
    }

    static async Task<bool> CreateNewProfile()
    {
        Console.WriteLine("\nРегистрация нового пользователя:");

        Console.Write("Логин: ");
        string login = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(login)) return false;

        var existing = await _profileRepo.GetByLoginAsync(login);
        if (existing != null)
        {
            Console.WriteLine("Ошибка: такой логин уже занят.");
            return false;
        }

        Console.Write("Пароль: ");
        string password = Console.ReadLine();
        Console.Write("Имя: ");
        string firstName = Console.ReadLine();
        Console.Write("Фамилия: ");
        string lastName = Console.ReadLine();
        Console.Write("Год рождения: ");
        int.TryParse(Console.ReadLine(), out int birthYear);

        var id = Guid.NewGuid();
        var profile = new Profile(id, login, password, firstName, lastName, birthYear);

        await _profileRepo.AddAsync(profile);
        AppInfo.Profiles.Add(profile);
        AppInfo.CurrentProfileId = id;
        AppInfo.UserTodos[id] = new TodoList();

        Console.WriteLine($"Профиль успешно создан: {profile.GetInfo()}");
        return true;
    }

    static async Task<bool> LoginProfile()
    {
        Console.Write("Логин: ");
        string login = Console.ReadLine();
        Console.Write("Пароль: ");
        string password = Console.ReadLine();

        var profile = AppInfo.Profiles.FirstOrDefault(p => p.Login == login && p.CheckPassword(password));
        if (profile == null)
        {
            Console.WriteLine("Неверный логин или пароль.");
            return false;
        }

        AppInfo.CurrentProfileId = profile.Id;

        var todos = await _todoRepo.GetAllByProfileAsync(profile.Id);
        var todoList = new TodoList();
        foreach (var t in todos) todoList.Add(t);
        AppInfo.UserTodos[profile.Id] = todoList;

        AppInfo.UndoStack.Clear();
        AppInfo.RedoStack.Clear();

        Console.WriteLine($"Добро пожаловать, {profile.FirstName}!");
        Console.WriteLine($"Загружено задач: {todoList.Count}");
        return true;
    }

    static void RunTodoApplication()
    {
        Console.WriteLine("\nВведите 'help' для списка команд.");

        CommandParser.Initialize(
            AppInfo.CurrentTodoList!,
            AppInfo.CurrentProfile!,
            _todoRepo,
            _profileRepo
        );

        while (true)
        {
            Console.Write($"{AppInfo.CurrentProfile?.Login} > ");
            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) continue;

            try
            {
                var command = CommandParser.Parse(input);
                command.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выполнении команды: {ex.Message}");
            }
        }
    }
}