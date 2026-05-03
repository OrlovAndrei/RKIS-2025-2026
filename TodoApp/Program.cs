using System;
using System.IO;
using System.Linq;
using TodoApp.Commands;
using TodoApp.Exceptions;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Clear();

            try
            {
                AppInfo.Storage = new FileManager();
                AppInfo.Profiles = AppInfo.Storage.LoadProfiles().ToList();
            }
            catch (DataStorageException ex)
            {
                Console.WriteLine($"Ошибка хранилища: {ex.Message}");
                return;
            }

            MainLoop();
        }

        private static bool SelectOrCreateProfile()
        {
            while (true)
            {
                Console.WriteLine("Войти в существующий профиль? [y/n]");
                Console.Write("> ");

                string choice = Console.ReadLine()?.Trim().ToLower() ?? "";

                if (choice == "y")
                {
                    LoginProfile();
                    return true;
                }

                if (choice == "n")
                {
                    CreateProfile();
                    return true;
                }

                Console.WriteLine("Пожалуйста, введите 'y' или 'n'.");
            }
        }

        private static void LoginProfile()
        {
            if (AppInfo.Profiles.Count == 0)
            {
                throw new ProfileNotFoundException("Нет сохранённых профилей. Создайте новый профиль.");
            }

            Console.Write("Логин: ");
            string login = Console.ReadLine()?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(login))
            {
                throw new InvalidArgumentException("Логин не может быть пустым.");
            }

            Console.Write("Пароль: ");
            string password = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidArgumentException("Пароль не может быть пустым.");
            }

            var profile = AppInfo.Profiles.FirstOrDefault(p =>
                p.Login.Equals(login, StringComparison.OrdinalIgnoreCase));

            if (profile == null)
            {
                throw new ProfileNotFoundException("Профиль с таким логином не найден.");
            }

            if (profile.Password != password)
            {
                throw new AuthenticationException("Неверный пароль.");
            }

            AppInfo.CurrentProfile = profile;
            AppInfo.UserTodos[profile.Id] = CreateTodoList(AppInfo.Storage.LoadTodos(profile.Id));

            SubscribeToTodoEvents(profile.Id, AppInfo.UserTodos[profile.Id]);
            AppInfo.ClearUndoRedo();
        }

        private static void CreateProfile()
        {
            Console.Write("Логин: ");
            string login = Console.ReadLine()?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(login))
            {
                throw new InvalidArgumentException("Логин не может быть пустым.");
            }

            if (AppInfo.Profiles.Any(p => p.Login.Equals(login, StringComparison.OrdinalIgnoreCase)))
            {
                throw new DuplicateLoginException("Этот логин уже занят.");
            }

            Console.Write("Пароль: ");
            string password = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidArgumentException("Пароль не может быть пустым.");
            }

            Console.Write("Имя: ");
            string firstName = Console.ReadLine()?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new InvalidArgumentException("Имя не может быть пустым.");
            }

            Console.Write("Фамилия: ");
            string lastName = Console.ReadLine()?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new InvalidArgumentException("Фамилия не может быть пустой.");
            }

            Console.Write("Год рождения: ");
            if (!int.TryParse(Console.ReadLine(), out int birthYear))
            {
                throw new InvalidArgumentException("Год рождения должен быть числом.");
            }

            int currentYear = DateTime.Now.Year;
            if (birthYear < 1900 || birthYear > currentYear)
            {
                throw new InvalidArgumentException($"Год рождения должен быть в диапазоне 1900-{currentYear}.");
            }

            var profile = new Profile(login, password, firstName, lastName, birthYear);
            AppInfo.Profiles.Add(profile);
            AppInfo.Storage.SaveProfiles(AppInfo.Profiles);

            AppInfo.CurrentProfile = profile;
            AppInfo.UserTodos[profile.Id] = new TodoList();
            AppInfo.Storage.SaveTodos(profile.Id, AppInfo.UserTodos[profile.Id].GetAll());

            SubscribeToTodoEvents(profile.Id, AppInfo.UserTodos[profile.Id]);
            AppInfo.ClearUndoRedo();
        }

        private static TodoList CreateTodoList(System.Collections.Generic.IEnumerable<TodoItem> items)
        {
            var todoList = new TodoList();
            foreach (var item in items)
            {
                todoList.Add(item);
            }

            return todoList;
        }

        private static void SubscribeToTodoEvents(Guid userId, TodoList todoList)
        {
            void Save(TodoItem item)
            {
                AppInfo.Storage.SaveTodos(userId, todoList.GetAll());
            }

            todoList.OnTodoAdded += Save;
            todoList.OnTodoDeleted += Save;
            todoList.OnTodoUpdated += Save;
            todoList.OnStatusChanged += Save;
        }

        private static void MainLoop()
        {
            while (true)
            {
                if (AppInfo.CurrentProfile is null)
                {
                    try
                    {
                        if (!SelectOrCreateProfile())
                        {
                            return;
                        }

                        Console.WriteLine($"\nДобро пожаловать, {AppInfo.CurrentProfile?.FirstName}!\n");
                    }
                    catch (Exception ex)
                    {
                        HandleException(ex);
                        continue;
                    }
                }

                Console.Write("> ");
                string input = Console.ReadLine() ?? "";

                if (input.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("До свидания!");
                    break;
                }

                try
                {
                    var command = CommandParser.Parse(input);
                    command.Execute();

                    if (command is IUndoableCommand undoableCmd)
                    {
                        AppInfo.UndoStack.Push(undoableCmd);
                        AppInfo.RedoStack.Clear();
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
                catch (ProfileNotFoundException ex)
                {
                    Console.WriteLine($"Ошибка профиля: {ex.Message}");
                }
                catch (DuplicateLoginException ex)
                {
                    Console.WriteLine($"Ошибка регистрации: {ex.Message}");
                }
                catch (InvalidCommandException ex)
                {
                    Console.WriteLine($"Ошибка команды: {ex.Message}");
                }
                catch (InvalidArgumentException ex)
                {
                    Console.WriteLine($"Ошибка аргумента: {ex.Message}");
                }
                catch (DataStorageException ex)
                {
                    Console.WriteLine($"Ошибка хранилища: {ex.Message}");
                }
                catch (Exception ex)
                {
                    _ = ex;
                    Console.WriteLine("Неожиданная ошибка.");
                }
            }
        }

        private static void HandleException(Exception ex)
        {
            switch (ex)
            {
                case TaskNotFoundException taskEx:
                    Console.WriteLine($"Ошибка задачи: {taskEx.Message}");
                    break;
                case AuthenticationException authEx:
                    Console.WriteLine($"Ошибка авторизации: {authEx.Message}");
                    break;
                case ProfileNotFoundException profileEx:
                    Console.WriteLine($"Ошибка профиля: {profileEx.Message}");
                    break;
                case DuplicateLoginException duplicateEx:
                    Console.WriteLine($"Ошибка регистрации: {duplicateEx.Message}");
                    break;
                case InvalidCommandException commandEx:
                    Console.WriteLine($"Ошибка команды: {commandEx.Message}");
                    break;
                case InvalidArgumentException argumentEx:
                    Console.WriteLine($"Ошибка аргумента: {argumentEx.Message}");
                    break;
                case DataStorageException storageEx:
                    Console.WriteLine($"Ошибка хранилища: {storageEx.Message}");
                    break;
                default:
                    Console.WriteLine("Неожиданная ошибка.");
                    break;
            }
        }
    }
}
