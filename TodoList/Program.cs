﻿using System;

class Program
{
    const int InitialCapacity = 2;
    static void Main(string[] args)
    {
        Console.Write("Введите имя: ");
        string firstName = Console.ReadLine();

        Console.Write("Введите фамилию: ");
        string lastName = Console.ReadLine();

        Console.Write("Введите год рождения: ");
        string birthYearInput = Console.ReadLine();

        int birthYear = int.Parse(birthYearString);
        int currentYear = DateTime.Now.Year;
        int age = currentYear - birthYear;

        Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");

        string[] tasks = new string[InitialCapacity];
        bool[] statuses = new bool[InitialCapacity];
        DateTime[] dates = new DateTime[InitialCapacity];
        int taskCount = 0;

        Console.WriteLine("Введите команду (help - список команд):");

        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine();

            if (input == "help")
            {
                ProcessHelp();
                continue;
            }

            if (input == "profile")
            {
                ProcessProfile(firstName, lastName, age);
                continue;
            }

            if (input.StartsWith("add "))
            {
                ProcessAdd(input, ref tasks, ref statuses, ref dates, ref taskCount);
                continue;
            }

            if (input == "view")
            {
                ProcessView(tasks, statuses, dates, taskCount);
                continue;
            }

            if (input.StartsWith("done "))
            {
                ProcessDone(input, ref statuses, ref dates, taskCount);
                continue;
            }
           
            if (input.StartsWith("delete "))
            {
                ProcessDelete(input, ref tasks, ref statuses, ref dates, ref taskCount);
                continue;
            }

            if (input.StartsWith("update "))
            {
                ProcessUpdate(input, ref tasks, ref dates, taskCount);
                continue;
            }

            if (input == "exit")
            {
                ProcessExit();
                break;
            }

            Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
        }
    }

    static void ProcessHelp()
    {
        Console.WriteLine("Доступные команды:");
        Console.WriteLine("help - показать список команд");
        Console.WriteLine("profile - показать данные пользователя");
        Console.WriteLine("add \"текст задачи\" - добавить задачу");
        Console.WriteLine("view - показать все задачи");
        Console.WriteLine("exit - выйти из программы");
    }

    static void ProcessProfile(string firstName, string lastName, int age)
    {
        Console.WriteLine($"{firstName} {lastName}, возраст - {age}");
    }

    static void ProcessAdd(string input, ref string[] tasks, ref bool[] statuses, ref DateTime[] dates, ref int taskCount)
    {
        string[] parts = input.Split(' ', 2);
        if (parts.Length == 2 && parts[0] == "add")
        {
            string taskPart = parts[1].Trim();
            if (taskPart.StartsWith("\"") && taskPart.EndsWith("\"") && taskPart.Length > 2)
            {
                string task = taskPart.Substring(1, taskPart.Length - 2);

                if (taskCount == tasks.Length)
                {
                    ResizeArrays(ref tasks, ref statuses, ref dates);
                }

                tasks[taskCount] = task;
                statuses[taskCount] = false;
                dates[taskCount] = DateTime.Now;
                taskCount++;

                Console.WriteLine("Задача добавлена.");
            }
            else
            {
                Console.WriteLine("Неверный формат команды. Используйте: add \"текст задачи\"");
            }
        }
        else
        {
            Console.WriteLine("Неверный формат команды. Используйте: add \"текст задачи\"");
        }
    }

    static void ProcessView(string[] tasks, bool[] statuses, DateTime[] dates, int taskCount)
    {
        if (taskCount == 0)
        {
            Console.WriteLine("Список задач пуст.");
        }
        else
        {
            Console.WriteLine("Список задач:");
            for (int i = 0; i < taskCount; i++)
            {
                string statusText = statuses[i] ? "сделано" : "не сделано";
                Console.WriteLine($"{i + 1}. {tasks[i]} {statusText} {dates[i]}");
            }
        }
    }

    static void ProcessExit()
    {
        Console.WriteLine("Выход из программы.");
    }
    static void ResizeArrays(ref string[] tasks, ref bool[] statuses, ref DateTime[] dates)
    {
        int newSize = tasks.Length * 2;
        string[] newTasks = new string[newSize];
        bool[] newStatuses = new bool[newSize];
        DateTime[] newDates = new DateTime[newSize];

        for (int i = 0; i < tasks.Length; i++)
        {
            newTasks[i] = tasks[i];
            newStatuses[i] = statuses[i];
            newDates[i] = dates[i];
        }

        tasks = newTasks;
        statuses = newStatuses;
        dates = newDates;
    }
}
