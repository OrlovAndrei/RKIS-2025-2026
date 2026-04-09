﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Data;
using TodoList.Exceptions;
using TodoList.Models;
using TodoList.Services;
using Microsoft.EntityFrameworkCore;

namespace TodoList
{
    public class Program
    {
        private static TodoRepository? _todoRepository;
        private static ProfileRepository? _profileRepository;
        private static Dictionary<Guid, (TodoList todoList, System.Action<TodoItem> saveHandler)> _todoListSubscriptions = new();

        public static async Task Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                await context.Database.MigrateAsync();
            }

            _todoRepository = new TodoRepository();
            _profileRepository = new ProfileRepository();

            while (true)
            {
                AppInfo.Profiles = await _profileRepository.GetAllAsync();

                Console.WriteLine("Добро пожаловать в TodoList!");

                if (!await ChooseProfileAsync())
                {
                    Console.WriteLine("Выход из программы.");
                    break;
                }

                await StartMainLoopAsync();

                AppInfo.ShouldLogout = false;
            }
        }

        private static async Task<bool> ChooseProfileAsync()
        {
            if (AppInfo.Profiles.Count > 0)
            {
                Console.Write("Войти в существующий профиль? [y/n]: ");
                string? response = Console.ReadLine()?.Trim().ToLower();

                if (response == "y" || response == "yes" || response == "да")
                {
                    try
                    {
                        if (await LoginToExistingProfileAsync())
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
                                await CreateNewProfileAsync();
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
                    string? createResponse = Console.ReadLine()?.Trim().ToLower();
                    if (createResponse == "y" || createResponse == "yes" || createResponse == "да")
                    {
                        try
                        {
                            await CreateNewProfileAsync();
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
                string? response = Console.ReadLine()?.Trim().ToLower();
                if (response == "y" || response == "yes" || response == "да")
                {
                    try
                    {
                        await CreateNewProfileAsync();
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

        private static async Task<bool> LoginToExistingProfileAsync()
        {
            Console.Write("Логин: ");
            string? login = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(login))
                throw new InvalidArgumentException("Логин не может быть пустым.");

            Console.Write("Пароль: ");
            string? password = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(password))
                throw new InvalidArgumentException("Пароль не может быть пустым.");

            var profile = await _profileRepository!.GetByLoginAsync(login);
            if (profile == null || !profile.CheckPassword(password))
                throw new AuthenticationException("Неверный логин или пароль.");

            AppInfo.CurrentProfileId = profile.Id;

            if (!AppInfo.TodosByUser.ContainsKey(profile.Id))
            {
                var todosFromDb = await _todoRepository!.GetAllForProfileAsync(profile.Id);
                var todoList = new TodoList(todosFromDb.ToList());
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

        private static async Task CreateNewProfileAsync()
        {
            Console.WriteLine("Создание нового профиля:");

            Console.Write("Логин: ");
            string? login = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(login))
                throw new InvalidArgumentException("Логин не может быть пустым.");

            if (AppInfo.Profiles.Any(p => p.Login.Equals(login, StringComparison.OrdinalIgnoreCase)))
                throw new DuplicateLoginException($"Пользователь с логином '{login}' уже существует.");

            Console.Write("Пароль: ");
            string? password = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(password))
                throw new InvalidArgumentException("Пароль не может быть пустым.");

            Console.Write("Имя: ");
            string? firstName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(firstName))
                throw new InvalidArgumentException("Имя не может быть пустым.");

            Console.Write("Фамилия: ");
            string? lastName = Console.ReadLine();
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

            await _profileRepository!.AddAsync(profile);

            Console.WriteLine($"Профиль создан: {profile.GetInfo()}");
        }

        private static async Task StartMainLoopAsync()
        {
            while (true)
            {
                Console.Write($"[{AppInfo.CurrentProfile?.Login}]> ");
                string? input = Console.ReadLine()?.Trim();

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

            async void SaveHandler(TodoItem item)
            {
                var items = todoList.ToList();
                await _todoRepository!.ReplaceAllForProfileAsync(userId, items);
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