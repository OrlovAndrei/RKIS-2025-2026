using TodoList.commands;
using TodoList.Exceptions;
using TodoList.Services;

namespace TodoList;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("=== Todo List Application ===");
            Console.WriteLine("Практическую работу 7 (РКИС, 2 семестр) сделали: Фоменко и Мартиросьян");

            using (var ctx = new Data.AppDbContext())
            {
                ctx.Database.EnsureCreated();
            }

            AppInfo.Profiles = AppInfo.ProfileRepo.GetAll();

            if (!LoginUser())
                return;

            Console.WriteLine("Введите 'help' для списка команд");

            while (true)
            {
                Console.Write("> ");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;

                try
                {
                    var command = CommandParser.Parse(input);
                    command.Execute();
                }
                catch (Exception ex) when (ex is TaskNotFoundException || ex is AuthenticationException ||
                                          ex is InvalidCommandException || ex is InvalidArgumentException ||
                                          ex is ProfileNotFoundException || ex is DuplicateLoginException)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Критическая ошибка при запуске: {ex.Message}");
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
                try { return LoginExistingUser(); }
                catch (AuthenticationException ex) { Console.WriteLine($"Ошибка входа: {ex.Message}"); }
            }
            else if (choice == "n")
            {
                try { return CreateNewUser(); }
                catch (Exception ex) when (ex is DuplicateLoginException || ex is InvalidArgumentException)
                {
                    Console.WriteLine($"Ошибка регистрации: {ex.Message}");
                }
            }
            else Console.WriteLine("Некорректный выбор.");
        }
    }

    private static bool LoginExistingUser()
    {
        Console.Write("Логин: ");
        var login = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(login))
            throw new InvalidArgumentException("Логин не может быть пустым.");

        Console.Write("Пароль: ");
        var password = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(password))
            throw new InvalidArgumentException("Пароль не может быть пустым.");

        var profile = AppInfo.ProfileRepo.GetByLogin(login);
        if (profile == null || !profile.CheckPassword(password))
            throw new AuthenticationException("Неверный логин или пароль.");

        AppInfo.CurrentProfileId = profile.Id;
        var todos = AppInfo.TodoRepo.GetAllForUser(profile.Id);
        var todoList = new TodoList();
        todoList.items = todos.ToList();
        AppInfo.TodosByUser[profile.Id] = todoList;
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
        if (string.IsNullOrWhiteSpace(login))
            throw new InvalidArgumentException("Логин не может быть пустым.");
        if (AppInfo.ProfileRepo.GetByLogin(login) != null)
            throw new DuplicateLoginException("Пользователь с таким логином уже существует.");

        Console.Write("Пароль: ");
        var password = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(password))
            throw new InvalidArgumentException("Пароль не может быть пустым.");

        Console.Write("Имя: ");
        var firstName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(firstName))
            throw new InvalidArgumentException("Имя не может быть пустым.");

        Console.Write("Фамилия: ");
        var lastName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(lastName))
            throw new InvalidArgumentException("Фамилия не может быть пустой.");

        Console.Write("Год рождения: ");
        if (!int.TryParse(Console.ReadLine(), out var birthYear) || birthYear < 1900 || birthYear > DateTime.Now.Year)
            throw new InvalidArgumentException("Некорректный год рождения.");

        var profile = new Profile(login, password, firstName, lastName, birthYear);
        AppInfo.ProfileRepo.Add(profile);
        AppInfo.CurrentProfileId = profile.Id;
        AppInfo.TodosByUser[profile.Id] = new TodoList();
        AppInfo.UndoStack.Clear();
        AppInfo.RedoStack.Clear();

        Console.WriteLine($"Профиль создан! Добро пожаловать, {firstName} {lastName}!");
        return true;
    }
}