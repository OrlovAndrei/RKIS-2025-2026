using System;
using System.Linq;
using TodoList.Exceptions;

namespace TodoList;

class Program
{
    public static void Main()
    {
        Console.WriteLine("Работу выполнили Турчин Крошняк");
        FileManager.EnsureDataDirectory();

        AppInfo.Profiles = FileManager.LoadAllProfiles();

        while (AppInfo.IsRunning)
        {
            try
            {
                if (AppInfo.CurrentProfileId == null)
                {
                    Console.Write("\nВойти в существующий профиль? [y/n]: ");
                    string choice = Console.ReadLine()?.ToLower();
                    if (choice == "y")
                        Login();
                    else if (choice == "n")
                        Register();
                    else
                    {
                        Console.WriteLine("Некорректный ввод. Завершение программы.");
                        break;
                    }
                }

                if (AppInfo.CurrentProfileId == null)
                    continue;

                var userId = AppInfo.CurrentProfileId.Value;
                if (!AppInfo.TodosDictionary.ContainsKey(userId))
                {
                    var todos = FileManager.LoadTodosForUser(userId);
                    AppInfo.TodosDictionary[userId] = todos;
                }

                var todoList = AppInfo.CurrentTodoList;
                todoList.OnTodoAdded += _ => SaveCurrentUserTodos();
                todoList.OnTodoDeleted += _ => SaveCurrentUserTodos();
                todoList.OnTodoUpdated += _ => SaveCurrentUserTodos();
                todoList.OnStatusChanged += _ => SaveCurrentUserTodos();

                bool profileActive = true;
                while (profileActive && AppInfo.CurrentProfileId != null && AppInfo.IsRunning)
                {
                    Console.Write("\nВведите команду: ");
                    string input = Console.ReadLine();

                    ICommand command = CommandParser.Parse(input);
                    command?.Execute();

                    if (AppInfo.CurrentProfileId == null)
                        profileActive = false;
                }
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
            catch (DuplicateLoginException ex)
            {
                Console.WriteLine($"Ошибка регистрации: {ex.Message}");
            }
            catch (ProfileNotFoundException ex)
            {
                Console.WriteLine($"Ошибка профиля: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Внутренняя ошибка: {ex.InnerException.Message}");
            }
        }
    }

    private static void SaveCurrentUserTodos()
    {
        if (AppInfo.CurrentProfileId.HasValue && AppInfo.CurrentTodoList != null)
        {
            FileManager.SaveTodosForUser(AppInfo.CurrentProfileId.Value, AppInfo.CurrentTodoList);
        }
    }

    private static void Login()
    {
        Console.Write("Логин: ");
        string login = Console.ReadLine();
        Console.Write("Пароль: ");
        string password = Console.ReadLine();

        var profile = AppInfo.Profiles.FirstOrDefault(p => p.Login == login && p.VerifyPassword(password));
        if (profile != null)
        {
            AppInfo.SetCurrentProfile(profile.Id);
            Console.WriteLine($"Добро пожаловать, {profile.FirstName}!");
        }
        else
        {
            throw new AuthenticationException("Неверный логин или пароль.");
        }
    }

    private static void Register()
    {
        Console.Write("Логин: ");
        string login = Console.ReadLine();
        if (AppInfo.Profiles.Any(p => p.Login == login))
            throw new DuplicateLoginException($"Пользователь с логином '{login}' уже существует.");

        Console.Write("Пароль: ");
        string password = Console.ReadLine();
        Console.Write("Имя: ");
        string firstName = Console.ReadLine();
        Console.Write("Фамилия: ");
        string lastName = Console.ReadLine();
        Console.Write("Год рождения: ");
        if (!int.TryParse(Console.ReadLine(), out int birthYear))
            throw new InvalidArgumentException("Год рождения должен быть числом.");

        var profile = new Profile(login, password, firstName, lastName, birthYear);
        AppInfo.Profiles.Add(profile);
        FileManager.SaveProfile(profile);
        AppInfo.SetCurrentProfile(profile.Id);
        Console.WriteLine($"Профиль создан. Добро пожаловать, {firstName}!");
    }
}