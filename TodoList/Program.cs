﻿using System;
using System.IO;

namespace Todolist
{
    class Program
    {
        private static Profile user;
        private static Todolist todoList = new Todolist();
        private static string dataDirPath = Path.Combine(Directory.GetCurrentDirectory(), "data");
        private static string profileFilePath = Path.Combine(dataDirPath, "profile.txt");
        private static string todoFilePath = Path.Combine(dataDirPath, "todo.csv");

        static void Main(string[] args)
        {
            Console.WriteLine("Прокопенко и Морозов");
            InitializeFileSystem();
            LoadData();

            while (true)
            {
                Console.Write("Введите команду: ");
                string input = Console.ReadLine().Trim();

                ICommand command = CommandParser.Parse(input, todoList, user);
                if (command != null)
                {
                    command.Execute();
                }
            }
        }

        static void InitializeFileSystem()
        {
            FileManager.EnsureDataDirectory(dataDirPath);
            CommandParser.SetFilePaths(todoFilePath, profileFilePath);
        }

        static void LoadData()
        {
            user = FileManager.LoadProfile(profileFilePath);
            
            if (user == null)
            {
                CreateProfile();
            }
            else
            {
                Console.WriteLine($"Добро пожаловать, {user.GetInfo()}!");
                Console.WriteLine();
            }

            todoList = FileManager.LoadTodos(todoFilePath);
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

            user = new Profile(firstName, lastName, yearBirth);
            FileManager.SaveProfile(user, profileFilePath);

            Console.WriteLine($"Профиль создан: {user.GetInfo()}");
            Console.WriteLine();
        }
    }
}