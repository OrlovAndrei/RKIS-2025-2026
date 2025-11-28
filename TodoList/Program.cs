﻿using System;
using System.IO;

namespace Todolist
{
    class Program
    {
        public static string TodoFilePath { get; private set; }
        private static string dataDirPath = Path.Combine(Directory.GetCurrentDirectory(), "data");
        private static string profileFilePath = Path.Combine(dataDirPath, "profile.txt");

        static void Main(string[] args)
        {
            Console.WriteLine("Прокопенко и Морозов");
            InitializeFileSystem();
            LoadData();

            while (true)
            {
                Console.Write("Введите команду: ");
                string input = Console.ReadLine().Trim();

                ICommand command = CommandParser.Parse(input, AppInfo.Todos, AppInfo.CurrentProfile);
                if (command != null)
                {
                    if (!(command is UndoCommand) && !(command is RedoCommand))
                    {
                        AppInfo.UndoStack.Push(command);
                        AppInfo.RedoStack.Clear();
                    }
                    
                    command.Execute();
                    
                    if (!(command is UndoCommand) && !(command is RedoCommand))
                    {
                        FileManager.SaveTodos(AppInfo.Todos, TodoFilePath);
                    }
                }
            }
        }

        static void InitializeFileSystem()
        {
            FileManager.EnsureDataDirectory(dataDirPath);
            CommandParser.SetFilePaths(profileFilePath);
            TodoFilePath = Path.Combine(dataDirPath, "todo.csv");
        }

        static void LoadData()
        {
            AppInfo.CurrentProfile = FileManager.LoadProfile(profileFilePath);
            
            if (AppInfo.CurrentProfile == null)
            {
                CreateProfile();
            }
            else
            {
                Console.WriteLine($"Добро пожаловать, {AppInfo.CurrentProfile.GetInfo()}!");
                Console.WriteLine();
            }

            AppInfo.Todos = FileManager.LoadTodos(TodoFilePath);
        }

        static void CreateProfile()
        {
            Console.WriteLine("Создание профиля пользователя");
            Console.Write("Введите ваше имя: ");
            string firstName = Console.ReadLine();

            Console.Write("Введите вашу фамилию: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите ваш год рождения: ");
            int yearBirth = int.Parse(Console.ReadLine());

            AppInfo.CurrentProfile = new Profile(firstName, lastName, yearBirth);
            FileManager.SaveProfile(AppInfo.CurrentProfile, profileFilePath);

            Console.WriteLine($"Профиль создан: {AppInfo.CurrentProfile.GetInfo()}");
            Console.WriteLine();
        }
    }
}