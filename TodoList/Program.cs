﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using TodoList.Exceptions;

namespace TodoList
{
    public class Program
    {
        private static FileManager? _fileManager;
        private static Dictionary<Guid, (TodoList todoList, Action<TodoItem> saveHandler)> _todoListSubscriptions = new();

        public static void Main(string[] args)
        {
            string dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
            _fileManager = new FileManager(dataDir);

            while (true)
            {
                AppInfo.Profiles = _fileManager.LoadProfiles().ToList();

                Console.WriteLine("Добро пожаловать в TodoList!");

                if (!ChooseProfile())
                {
                    Console.WriteLine("Выход из программы.");
                    break;
                }

                StartMainLoop();

                AppInfo.ShouldLogout = false;
            }
        }

        private static bool ChooseProfile()
        {
            if (AppInfo.Profiles.Count > 0)
            {
                Console.Write("Войти в существующий профиль? [y/n]: ");
                string response = Console.ReadLine()?.Trim().ToLower();

                if (response == "y" || response == "yes" || response == "да")
                {
                    try
                    {
                        if (LoginToExistingProfile())
                            return true;
                    }
                    catch (AuthenticationException ex)
                    {
                        Console.WriteLine($"Ошибка входа: {ex.Message}");
                        Console.Write("Создать новый профиль? [y/n]: ");
                        response = Console.ReadLine()?.Trim().ToLower();
                        if (response == "y" || response == "yes" || response == "да")
                        {
                            try
                            {
                                CreateNewProfile();
                                return true;
                            }
                            catch (DuplicateLoginException ex2)
                            {
                                Console.WriteLine($"Ошибка: {ex2.Message}");
                                return false;
                            }
                            catch (InvalidArgumentException ex2)
                            {
                                Console.WriteLine($"Ошибка ввода: {ex2.Message}");
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    Console.Write("Создать новый профиль? [y/n]: ");
                    string createResponse = Console.ReadLine()?.Trim().ToLower();
                    if (createResponse == "y" || createResponse == "yes" || createResponse == "да")
                    {
                        try
                        {
                            CreateNewProfile();
                            return true;
                        }
                        catch (DuplicateLoginException ex)
                        {
                            Console.WriteLine($"Ошибка: {ex.Message}");
                            return false;
                        }
                        catch (InvalidArgumentException ex)
                        {
                            Console.WriteLine($"Ошибка ввода: {ex.Message}");
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                Console.WriteLine("Профили не найдены.");
                Console.Write("Создать новый профиль? [y/n]: ");
                string response = Console.ReadLine()?.Trim().ToLower();
                if (response == "y" || response == "yes" || response == "да")
                {
                    try
                    {
                        CreateNewProfile();
                        return true;
                    }
                    catch (DuplicateLoginException ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                        return false;
                    }
                    catch (InvalidArgumentException ex)
                    {
                        Console.WriteLine($"Ошибка ввода: {ex.Message}");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private static bool LoginToExistingProfile()
        {
            Console.Write("Логин: ");
            string login = Console.ReadLine();

            Console.Write("Пароль: ");
            string password = Console.ReadLine();

            var profile = AppInfo.Profiles.FirstOrDefault(p => p.Login == login && p.CheckPassword(password));
            if (profile == null)
                throw new AuthenticationException("Неверный логин или пароль.");

            AppInfo.CurrentProfileId = profile.Id;

            if (!AppInfo.TodosByUser.ContainsKey(profile.Id))
            {
                var todoList = new TodoList(_fileManager!.LoadTodos(profile.Id).ToList());
                SubscribeToTodoListEvents(todoList, profile.Id);
                AppInfo.TodosByUser[profile.Id] = todoList;
            }
            else
            {
                var todoList = AppInfo.TodosByUser[profile.Id];
                SubscribeToTodoListEvents(todoList, profile.Id);
            }

            Console.WriteLine($"Вход выполнен. Профиль: {profile.GetInfo()}");
            return true;
        }

        private static void CreateNewProfile()
        {
            Console.WriteLine("Создание нового профиля:");

            Console.Write("Логин: ");
            string login = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(login))
                throw new InvalidArgumentException("Логин не может быть пустым.");

            if (AppInfo.Profiles.Any(p => p.Login.Equals(login, StringComparison.OrdinalIgnoreCase)))
                throw new DuplicateLoginException($"Пользователь с логином '{login}' уже существует.");

            Console.Write("Пароль: ");
            string password = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(password))
                throw new InvalidArgumentException("Пароль не может быть пустым.");

            Console.Write("Имя: ");
            string firstName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(firstName))
                throw new InvalidArgumentException("Имя не может быть пустым.");

            Console.Write("Фамилия: ");
            string lastName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(lastName))
                throw new InvalidArgumentException("Фамилия не может быть пустой.");

            Console.Write("Год рождения: ");
            if (!int.TryParse(Console.ReadLine(), out int birthYear))
                throw new InvalidArgumentException("Неверный формат года.");

            var profile = new Profile(login, password, firstName, lastName, birthYear);
            AppInfo.Profiles.Add(profile);
            AppInfo.CurrentProfileId = profile.Id;

            var todoList = new TodoList(new List<TodoItem>());
            SubscribeToTodoListEvents(todoList, profile.Id);
            AppInfo.TodosByUser[profile.Id] = todoList;

            _fileManager!.SaveProfiles(AppInfo.Profiles);

            Console.WriteLine($"Профиль создан: {profile.GetInfo()}");
        }

        private static void StartMainLoop()
        {
            while (true)
            {
                Console.Write($"[{AppInfo.CurrentProfile?.Login}]> ");
                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Пусто.");
                    continue;
                }

                try
                {
                    ICommand command = CommandParser.Parse(input);
                    command.Execute();

                    if (AppInfo.ShouldLogout)
                    {
                        if (AppInfo.CurrentProfileId.HasValue)
                            UnsubscribeFromTodoListEvents(AppInfo.CurrentProfileId.Value);
                        return;
                    }

                    _fileManager!.SaveProfiles(AppInfo.Profiles);
                }
                catch (InvalidCommandException ex)
                {
                    Console.WriteLine($"Ошибка команды: {ex.Message}");
                }
                catch (InvalidArgumentException ex)
                {
                    Console.WriteLine($"Ошибка в аргументах: {ex.Message}");
                }
                catch (TaskNotFoundException ex)
                {
                    Console.WriteLine($"Ошибка задачи: {ex.Message}");
                }
                catch (AuthenticationException ex)
                {
                    Console.WriteLine($"Ошибка авторизации: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
                }
            }
        }

        private static void SubscribeToTodoListEvents(TodoList todoList, Guid userId)
        {
            UnsubscribeFromTodoListEvents(userId);

            void SaveHandler(TodoItem item)
            {
                _fileManager!.SaveTodos(userId, todoList);
            }

            todoList.OnTodoAdded += SaveHandler;
            todoList.OnTodoDeleted += SaveHandler;
            todoList.OnTodoUpdated += SaveHandler;
            todoList.OnStatusChanged += SaveHandler;

            _todoListSubscriptions[userId] = (todoList, SaveHandler);
        }

        private static void UnsubscribeFromTodoListEvents(Guid userId)
        {
            if (_todoListSubscriptions.TryGetValue(userId, out var subscription))
            {
                var (todoList, handler) = subscription;

                todoList.OnTodoAdded -= handler;
                todoList.OnTodoDeleted -= handler;
                todoList.OnTodoUpdated -= handler;
                todoList.OnStatusChanged -= handler;

                _todoListSubscriptions.Remove(userId);
            }
        }
    }
}