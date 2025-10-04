﻿using System;

namespace Todolist
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Фоменко и Мартиросьян");

            Console.Write("Введите ваше имя: ");
            string firstName = Console.ReadLine();

            Console.Write("Введите вашу фамилию: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите ваш год рождения: ");
            string yearBirthString = Console.ReadLine();

            int yearBirth = int.Parse(yearBirthString);
            int age = DateTime.Now.Year - yearBirth;

            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст – {age}");
            
            // Инициализация массива задач
            string[] todos = new string[2];
            int todoCount = 0;
            
            Console.WriteLine("Добро пожаловать в систему управления задачами!");
            Console.WriteLine("Введите 'help' для списка команд");

            // Основной цикл программы
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                
                if (string.IsNullOrWhiteSpace(input))
                    continue;
                    
                string[] parts = input.Split(' ');
                string command = parts[0].ToLower();
                
                switch (command)
                {
                    case "help":
                        ShowHelp();
                        break;
                    case "profile":
                        ShowProfile(firstName, lastName, yearBirth);
                        break;
                    case "add":
                        if (parts.Length < 2)
                        {
                            Console.WriteLine("Ошибка: не указана задача");
                        }
                        else
                        {
                            string task = string.Join(" ", parts, 1, parts.Length - 1);
                            AddTodo(ref todos, ref todoCount, task);
                        }
                        break;
                    case "exit":
                        Console.WriteLine("Выход из программы...");
                        return;
                    default:
                        Console.WriteLine($"Неизвестная команда: {command}");
                        break;
                }
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help    - вывести список команд");
            Console.WriteLine("profile - показать данные пользователя");
            Console.WriteLine("add     - добавить задачу");
            Console.WriteLine("exit    - выход из программы");
        }

        static void ShowProfile(string firstName, string lastName, int birthYear)
        {
            Console.WriteLine($"{firstName} {lastName}, {birthYear}");
        }

        static void AddTodo(ref string[] todos, ref int todoCount, string task)
        {
            todos[todoCount] = task;
            todoCount++;
            Console.WriteLine("Задача добавлена!");
        }
    }
}