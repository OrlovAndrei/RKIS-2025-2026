using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    private static IDataStorage? _storage;

    static void Main()
    {
        try
        {
            _storage = new SqliteDataStorage("todo.db");

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
        var profiles = _storage.LoadProfiles();
        AppInfo.Profiles = profiles.ToList();
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

        _storage.SaveProfiles(AppInfo.Profiles);

        Console.WriteLine($"Профиль создан: {newProfile.GetInfo()}");

        SubscribeTodoListEvents(todoList, newId);

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

        var todos = _storage.LoadTodos(profile.Id);
        var todoList = new TodoList(todos.ToList());
        AppInfo.UserTodos[profile.Id] = todoList;

        AppInfo.UndoStack.Clear();
        AppInfo.RedoStack.Clear();

        SubscribeTodoListEvents(todoList, profile.Id);

        Console.WriteLine($"Вход выполнен: {profile.GetInfo()}");
        Console.WriteLine($"Загружено задач: {AppInfo.CurrentTodoList?.Count ?? 0}");

        return true;
    }

    static void RunTodoApplication()
    {
        Console.WriteLine("\nВведите 'help' для списка команд.");

        CommandParser.Initialize(
            AppInfo.CurrentTodoList,
            AppInfo.CurrentProfile,
            _storage!
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
            catch (TaskNotFoundException ex)
            {
                Console.WriteLine($"Ошибка задачи: {ex.Message}");
            }
            catch (InvalidArgumentException ex)
            {
                Console.WriteLine($"Ошибка аргумента: {ex.Message}");
            }
            catch (InvalidCommandException ex)
            {
                Console.WriteLine($"Ошибка команды: {ex.Message}");
            }
            catch (AuthenticationException ex)
            {
                Console.WriteLine($"Ошибка авторизации: {ex.Message}");
            }
            catch (DuplicateLoginException ex)
            {
                Console.WriteLine($"Ошибка регистрации: {ex.Message}");
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine($"Ошибка данных: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка ввода-вывода: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
            }
        }
    }

    static void SubscribeTodoListEvents(TodoList todoList, Guid userId)
    {
        todoList.OnTodoAdded += (item) => _storage.SaveTodos(userId, todoList);
        todoList.OnTodoDeleted += (item) => _storage.SaveTodos(userId, todoList);
        todoList.OnTodoUpdated += (item) => _storage.SaveTodos(userId, todoList);
        todoList.OnStatusChanged += (item) => _storage.SaveTodos(userId, todoList);
    }
}