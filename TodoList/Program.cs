using System;
using System.IO;

class Program
{
    private static string _dataDirectory = "Data";
    private static string _profilesFilePath;

    static void Main()
    {
        try
        {
            _profilesFilePath = Path.Combine(_dataDirectory, "profiles.csv");

            FileManager.EnsureDataDirectory(_dataDirectory);
            LoadProfiles();

            if (!SelectOrCreateProfile())
            {
                Console.WriteLine("Выход из программы.");
                return;
            }

            RunTodoApplication();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Критическая ошибка: {ex.Message}");
        }
    }

    static void LoadProfiles()
    {
        AppInfo.Profiles = FileManager.LoadProfiles(_profilesFilePath);
        Console.WriteLine($"Загружено профилей: {AppInfo.Profiles.Count}");
    }

    static bool SelectOrCreateProfile()
    {
        Console.WriteLine("Добро пожаловать в TodoList!");

        if (AppInfo.Profiles.Count > 0)
        {
            Console.Write("Войти в существующий профиль? [y/n]: ");
            string choice = Console.ReadLine()?.ToLower();

            if (choice == "y")
            {
                return LoginProfile();
            }
        }

        return CreateNewProfile();
    }

    static bool CreateNewProfile()
    {
        Console.WriteLine("Создание нового профиля:");

        Console.Write("Логин: ");
        string login = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(login))
        {
            Console.WriteLine("Ошибка: логин не может быть пустым.");
            return false;
        }

        if (AppInfo.Profiles.Exists(p => p.Login == login))
        {
            Console.WriteLine("Ошибка: пользователь с таким логином уже существует.");
            return false;
        }

        Console.Write("Пароль: ");
        string password = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Ошибка: пароль не может быть пустым.");
            return false;
        }

        Console.Write("Имя: ");
        string firstName = Console.ReadLine();

        Console.Write("Фамилия: ");
        string lastName = Console.ReadLine();

        Console.Write("Год рождения: ");
        string yearInput = Console.ReadLine();

        int birthYear;
        if (!int.TryParse(yearInput, out birthYear))
        {
            Console.WriteLine("Неверный формат года. Установлен 2000 год по умолчанию.");
            birthYear = 2000;
        }
        else if (birthYear < 1900 || birthYear > DateTime.Now.Year)
        {
            Console.WriteLine($"Год рождения должен быть от 1900 до {DateTime.Now.Year}. Установлен 2000.");
            birthYear = 2000;
        }

        Guid newId = Guid.NewGuid();
        var newProfile = new Profile(newId, login, password, firstName, lastName, birthYear);

        AppInfo.Profiles.Add(newProfile);
        AppInfo.CurrentProfileId = newId;

        var todoList = new TodoList();
        AppInfo.UserTodos[newId] = todoList;

        FileManager.SaveProfiles(AppInfo.Profiles, _profilesFilePath);

        Console.WriteLine($"Профиль создан: {newProfile.GetInfo()}");

        string todoFilePath = FileManager.GetUserTodoFilePath(newId, _dataDirectory);
        SubscribeTodoListEvents(todoList, todoFilePath);

        return true;
    }

    static bool LoginProfile()
    {
        Console.Write("Логин: ");
        string login = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(login))
        {
            Console.WriteLine("Ошибка: логин не может быть пустым.");
            return false;
        }

        Console.Write("Пароль: ");
        string password = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Ошибка: пароль не может быть пустым.");
            return false;
        }

        var profile = AppInfo.Profiles.Find(p => p.Login == login && p.CheckPassword(password));

        if (profile == null)
        {
            Console.WriteLine("Неверный логин или пароль.");
            return false;
        }

        AppInfo.CurrentProfileId = profile.Id;

        string todoFilePath = FileManager.GetUserTodoFilePath(profile.Id, _dataDirectory);
        var todoList = FileManager.LoadTodos(todoFilePath);
        AppInfo.UserTodos[profile.Id] = todoList;

        AppInfo.UndoStack.Clear();
        AppInfo.RedoStack.Clear();

        SubscribeTodoListEvents(todoList, todoFilePath);

        Console.WriteLine($"Вход выполнен: {profile.GetInfo()}");
        Console.WriteLine($"Загружено задач: {AppInfo.CurrentTodoList?.Count ?? 0}");

        return true;
    }

    static void RunTodoApplication()
    {
        Console.WriteLine("\nВведите 'help' для списка команд.");

        string todoFilePath = FileManager.GetUserTodoFilePath(AppInfo.CurrentProfileId.Value, _dataDirectory);
        CommandParser.Initialize(
            AppInfo.CurrentTodoList,
            AppInfo.CurrentProfile,
            todoFilePath,
            _profilesFilePath
        );

        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                continue;

            if (!AppInfo.CurrentProfileId.HasValue)
            {
                Console.WriteLine("Ошибка: нет активного профиля.");
                continue;
            }

            try
            {
                ICommand command = CommandParser.Parse(input);

                if (command is ProfileCommand profileCmd && profileCmd.ShouldLogout)
                {
                    command.Execute();

                    if (!AppInfo.CurrentProfileId.HasValue)
                    {
                        Console.WriteLine("\nВозврат к выбору профиля...");
                        Console.WriteLine("==============================");
                        return;
                    }
                }
                else
                {
                    command.Execute();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }

    static void SubscribeTodoListEvents(TodoList todoList, string filePath)
    {
        todoList.OnTodoAdded -= FileManager.SaveTodoList;
        todoList.OnTodoDeleted -= FileManager.SaveTodoList;
        todoList.OnTodoUpdated -= FileManager.SaveTodoList;
        todoList.OnStatusChanged -= FileManager.SaveTodoList;

        todoList.OnTodoAdded += FileManager.SaveTodoList;
        todoList.OnTodoDeleted += FileManager.SaveTodoList;
        todoList.OnTodoUpdated += FileManager.SaveTodoList;
        todoList.OnStatusChanged += FileManager.SaveTodoList;
    }
}