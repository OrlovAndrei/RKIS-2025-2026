﻿using System;
using System.IO;

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
                    Login();
                    break;
                case "2":
                    CreateProfile();
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

            Console.Write("Введите пароль: ");
            string password = Console.ReadLine();

            Profile foundProfile = AppInfo.Profiles.Find(p => p.Login == login && p.CheckPassword(password));

            if (foundProfile != null)
            {
                AppInfo.CurrentProfileId = foundProfile.Id;
                LoadUserTodos();
                AppInfo.ClearUndoRedoStacks();
                Console.WriteLine($"\nДобро пожаловать, {foundProfile.FirstName}!");
            }
            else
            {
                Console.WriteLine("Неверный логин или пароль.");
            }
        }

        static void CreateProfile()
        {
            Console.WriteLine("\n=== Создание нового профиля ===");
            
            Console.Write("Введите логин: ");
            string login = Console.ReadLine();

            if (AppInfo.Profiles.Exists(p => p.Login == login))
            {
                Console.WriteLine("Этот логин уже занят.");
                return;
            }

            Console.Write("Введите пароль: ");
            string password = Console.ReadLine();

            Console.Write("Введите ваше имя: ");
            string firstName = Console.ReadLine();

            Console.Write("Введите вашу фамилию: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите ваш год рождения: ");
            if (!int.TryParse(Console.ReadLine(), out int birthYear))
            {
                Console.WriteLine("Неверный год рождения.");
                return;
            }

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

                ICommand command = CommandParser.Parse(input);
                if (command != null)
                {
                    if (command is AddCommand || command is DeleteCommand || 
                        command is UpdateCommand || command is StatusCommand)
                    {
                        AppInfo.UndoStack.Push(command);
                        AppInfo.RedoStack.Clear();
                    }

                    command.Execute();
                }
            }
        }
    }
}