using TodoList.commands;
using TodoList.Exceptions;
using System.Security.Cryptography;
using System.Text;

namespace TodoList;

internal class Program
{
    private static IDataStorage _dataStorage = null!;

    private static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("=== Todo List Application ===");
            Console.WriteLine("Практическую работу 4 (РКИС, 2 семестр) сделали: Фоменко и Мартиросьян");

            // Ключ и IV для AES (в реальном проекте должны храниться безопасно)
            byte[] key = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
            byte[] iv = Encoding.UTF8.GetBytes("1234567890123456");

            _dataStorage = new ApiDataStorage("http://localhost:5000", key, iv);
            AppInfo.DataStorage = _dataStorage; // сохраняем ссылку для команд

            AppInfo.Profiles = _dataStorage.LoadProfiles().ToList();

            if (!LoginUser())
                return;

            Console.WriteLine("Введите 'help' для списка команд");

            while (true)
            {
                Console.Write("> ");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    continue;

                try
                {
                    var command = CommandParser.Parse(input);
                    command.Execute();
                }
                catch (TaskNotFoundException ex)
                {
                    Console.WriteLine($"Ошибка задачи: {ex.Message}");
                }
                catch (AuthenticationException ex)
                {
                    Console.WriteLine($"Ошибка авторизации: {ex.Message}");
                }
                catch (InvalidCommandException ex)
                {
                    Console.WriteLine($"Ошибка команды: {ex.Message}");
                }
                catch (InvalidArgumentException ex)
                {
                    Console.WriteLine($"Ошибка аргументов: {ex.Message}");
                }
                catch (ProfileNotFoundException ex)
                {
                    Console.WriteLine($"Ошибка профиля: {ex.Message}");
                }
                catch (DuplicateLoginException ex)
                {
                    Console.WriteLine($"Ошибка регистрации: {ex.Message}");
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
                try
                {
                    return LoginExistingUser();
                }
                catch (AuthenticationException ex)
                {
                    Console.WriteLine($"Ошибка входа: {ex.Message}");
                }
            }
            else if (choice == "n")
            {
                try
                {
                    return CreateNewUser();
                }
                catch (DuplicateLoginException ex)
                {
                    Console.WriteLine($"Ошибка регистрации: {ex.Message}");
                }
                catch (InvalidArgumentException ex)
                {
                    Console.WriteLine($"Ошибка ввода: {ex.Message}");
                }
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
        if (string.IsNullOrWhiteSpace(login))
            throw new InvalidArgumentException("Логин не может быть пустым.");

        Console.Write("Пароль: ");
        var password = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(password))
            throw new InvalidArgumentException("Пароль не может быть пустым.");

        var profile = AppInfo.Profiles.FirstOrDefault(p => p.Login == login && p.CheckPassword(password));

        if (profile == null)
            throw new AuthenticationException("Неверный логин или пароль.");

        AppInfo.CurrentProfileId = profile.Id;
        var loadedTodos = _dataStorage.LoadTodos(profile.Id).ToList();
        var todoList = new TodoList();
        todoList.items = loadedTodos; // прямое присвоение (items - публичное поле)
        SubscribeToTodoListEvents(todoList);
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

        if (AppInfo.Profiles.Any(p => p.Login == login))
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
        var birthYearInput = Console.ReadLine();
        if (!int.TryParse(birthYearInput, out var birthYear) || birthYear < 1900 || birthYear > DateTime.Now.Year)
            throw new InvalidArgumentException("Некорректный год рождения. Введите число от 1900 до текущего года.");

        var profile = new Profile(login, password, firstName, lastName, birthYear);
        AppInfo.Profiles.Add(profile);
        _dataStorage.SaveProfiles(AppInfo.Profiles);

        AppInfo.CurrentProfileId = profile.Id;
        var todoList = new TodoList();
        SubscribeToTodoListEvents(todoList);
        AppInfo.TodosByUser[profile.Id] = todoList;
        AppInfo.UndoStack.Clear();
        AppInfo.RedoStack.Clear();

        Console.WriteLine($"Профиль создан! Добро пожаловать, {firstName} {lastName}!");
        return true;
    }

    private static void SubscribeToTodoListEvents(TodoList todoList)
    {
        todoList.OnTodoAdded += (item) => _dataStorage.SaveTodos(AppInfo.CurrentProfileId!.Value, todoList.items);
        todoList.OnTodoDeleted += (item) => _dataStorage.SaveTodos(AppInfo.CurrentProfileId!.Value, todoList.items);
        todoList.OnTodoUpdated += (item) => _dataStorage.SaveTodos(AppInfo.CurrentProfileId!.Value, todoList.items);
        todoList.OnStatusChanged += (item) => _dataStorage.SaveTodos(AppInfo.CurrentProfileId!.Value, todoList.items);
    }
}