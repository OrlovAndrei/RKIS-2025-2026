﻿using System;
using System.IO;
using Todolist.Exceptions;

namespace Todolist
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Todolist - Прокопенко и Морозов");
            Console.WriteLine("================================\n");

            FileManager.EnsureDataDirectory();
            InitializeProfiles();

            while (true)
            {
                if (!AppInfo.CurrentProfileId.HasValue)
                {
                    ShowLoginMenu();
                }
                else
                {
                    ShowMainMenu();
                }
            }
        }

        static void InitializeProfiles()
        {
            AppInfo.Profiles = FileManager.LoadProfiles();
            Console.WriteLine($"Загружено профилей: {AppInfo.Profiles.Count}");
        }

        static void ShowLoginMenu()
        {
            Console.WriteLine("\n=== Меню входа ===");
            Console.WriteLine("1. Войти в существующий профиль");
            Console.WriteLine("2. Создать новый профиль");
            Console.WriteLine("3. Выйти из программы");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    try
                    {
                        Login();
                    }
                    catch (ProfileNotFoundException ex)
                    {
                        Console.WriteLine($"Ошибка входа: {ex.Message}");
                    }
                    catch (InvalidArgumentException ex)
                    {
                        Console.WriteLine($"Ошибка ввода: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
                    }
                    break;
                case "2":
                    try
                    {
                        CreateProfile();
                    }
                    catch (DuplicateLoginException ex)
                    {
                        Console.WriteLine($"Ошибка регистрации: {ex.Message}");
                    }
                    catch (InvalidArgumentException ex)
                    {
                        Console.WriteLine($"Ошибка ввода: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
                    }
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }

        static void Login()
        {
            Console.Write("Введите логин: ");
            string login = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(login))
                throw new InvalidArgumentException("Логин не может быть пустым");

            Console.Write("Введите пароль: ");
            string password = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(password))
                throw new InvalidArgumentException("Пароль не может быть пустым");

            Profile foundProfile = AppInfo.Profiles.Find(p => p.Login == login && p.CheckPassword(password));

            if (foundProfile == null)
                throw new ProfileNotFoundException("Неверный логин или пароль");

            AppInfo.CurrentProfileId = foundProfile.Id;
            LoadUserTodos();
            AppInfo.ClearUndoRedoStacks();
            Console.WriteLine($"\nДобро пожаловать, {foundProfile.FirstName}!");
        }

        static void CreateProfile()
        {
            Console.WriteLine("\n=== Создание нового профиля ===");

            Console.Write("Введите логин: ");
            string login = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(login))
                throw new InvalidArgumentException("Логин не может быть пустым");

            if (AppInfo.Profiles.Exists(p => p.Login == login))
                throw new DuplicateLoginException($"Логин '{login}' уже занят");

            Console.Write("Введите пароль: ");
            string password = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(password))
                throw new InvalidArgumentException("Пароль не может быть пустым");

            Console.Write("Введите ваше имя: ");
            string firstName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(firstName))
                throw new InvalidArgumentException("Имя не может быть пустым");

            Console.Write("Введите вашу фамилию: ");
            string lastName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(lastName))
                throw new InvalidArgumentException("Фамилия не может быть пустой");

            Console.Write("Введите ваш год рождения: ");
            if (!int.TryParse(Console.ReadLine(), out int birthYear) || birthYear < 1900 || birthYear > DateTime.Now.Year)
                throw new InvalidArgumentException("Неверный год рождения. Введите число от 1900 до текущего года.");

            Profile newProfile = new Profile(login, password, firstName, lastName, birthYear);
            AppInfo.Profiles.Add(newProfile);
            FileManager.SaveProfiles(AppInfo.Profiles);

            AppInfo.CurrentProfileId = newProfile.Id;
            LoadUserTodos();
            AppInfo.ClearUndoRedoStacks();

            Console.WriteLine($"\nПрофиль создан успешно! Добро пожаловать, {firstName}!");
        }

        static void LoadUserTodos()
        {
            if (AppInfo.CurrentProfileId.HasValue)
            {
                Guid userId = AppInfo.CurrentProfileId.Value;
                if (!AppInfo.UserTodos.ContainsKey(userId))
                {
                    AppInfo.UserTodos[userId] = FileManager.LoadTodos(userId);
                }
            }
        }

        static void ShowMainMenu()
        {
            Console.WriteLine("\n" + new string('=', 50));
            Console.WriteLine("Главное меню (введите 'help' для списка команд)");
            Console.WriteLine(new string('=', 50));

            while (AppInfo.CurrentProfileId.HasValue)
            {
                Console.Write("\n> ");
                string input = Console.ReadLine().Trim();

                if (string.IsNullOrEmpty(input))
                    continue;

                try
                {
                    ICommand command = CommandParser.Parse(input);
                    if (command == null)
                        throw new InvalidCommandException("Неизвестная команда");

                    command.Execute();

                    if (command is AddCommand || command is DeleteCommand || command is UpdateCommand || command is StatusCommand)
                    {
                        AppInfo.UndoStack.Push(command);
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
                catch (InvalidCommandException ex)
                {
                    Console.WriteLine($"Ошибка команды: {ex.Message}");
                }
                catch (InvalidArgumentException ex)
                {
                    Console.WriteLine($"Ошибка в аргументах: {ex.Message}");
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
    }
}