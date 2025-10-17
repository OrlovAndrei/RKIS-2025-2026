using System;
using System.Collections.Generic;

namespace TodoList
{

    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Работу выполнили Шелепов и Кузьменко");

            Console.WriteLine("Введите имя");
            string name = Console.ReadLine();

            Console.WriteLine("Введите фамилию");
            string surname = Console.ReadLine();

            Console.WriteLine("Введите год рождения");
            string birthYearInput = Console.ReadLine();

            if (!int.TryParse(birthYearInput, out int birthYear))
            {
                Console.WriteLine("Ошибка: введите корректный год рождения");
                return;
            }

            int currentYear = DateTime.Now.Year;
            
            int age = currentYear - birthYear;

            Console.WriteLine($"Добавлен пользователь {name} {surname} Возраст - {age}");

            string[] todos = new string[2];
            bool[] statuses = new bool[2];
            DateTime[] dates = new DateTime[2];
            int count = 0;

            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (line == null || line.Trim() == "exit") break;

                var (command, flags, argsLine) = ParseCommand(line);

                switch (command)
                {
                    case "add":
                        AddTask(todos, statuses, dates, ref count, argsLine, flags);
                        break;
                    case "done":
                        MarkTaskDone(statuses, dates, count, argsLine);
                        break;
                    case "delete":
                        DeleteTask(todos, statuses, dates, ref count, argsLine);
                        break;
                    case "update":
                        UpdateTask(todos, dates, count, argsLine);
                        break;
                    case "view":
                        ViewTasks(todos, statuses, dates, count, flags);
                        break;
                    case "help":
                        ShowHelp();
                        break;
                    case "profile":
                        ShowProfile(name, surname, birthYear);
                        break;
                    default:
                        Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
                        break;
                }
            }
        }

        // --- FLAG PARSING ---
        static (string command, HashSet<string> flags, string argsLine) ParseCommand(string line)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                return ("", new HashSet<string>(), "");

            string command = parts[0];
            var flags = new HashSet<string>();
            int i = 1;

            for (; i < parts.Length; i++)
            {
                string token = parts[i];
                if (token.StartsWith("--"))
                {
                    flags.Add(token.Substring(2));
                }
                else if (token.StartsWith("-"))
                {
                    foreach (char c in token.Substring(1))
                        flags.Add(c.ToString());
                }
                else break;
            }

            string argsLine = string.Join(' ', parts, i, parts.Length - i);
            return (command, flags, argsLine);
        }

        static void ShowHelp()
        {
            Console.WriteLine("Команды:");
            Console.WriteLine("profile - выводит данные о пользователе");
            Console.WriteLine("add 'текст задачи' - добавляет задачу");
            Console.WriteLine("add --multiline / -m - многострочный режим добавления");
            Console.WriteLine("done (номер) - пометить задачу выполненной");
            Console.WriteLine("update (номер) 'новый текст' - обновить задачу");
            Console.WriteLine("delete (номер) - удалить задачу");
            Console.WriteLine("view - вывести все задачи");
            Console.WriteLine("exit - выход");
        }

        static void ShowProfile(string name, string surname, int birthYear)
        {
            Console.WriteLine($"{name} {surname}, {birthYear}");
        }

        // --- ADD TASK WITH MULTILINE SUPPORT ---
        static void AddTask(string[] todos, bool[] statuses, DateTime[] dates, ref int count, string line, HashSet<string> flags)
        {
            if (flags.Contains("multiline") || flags.Contains("m"))
            {
                Console.WriteLine("Введите строки задачи (введите !end для завершения):");
                var lines = new List<string>();
                while (true)
                {
                    Console.Write("> ");
                    string? input = Console.ReadLine();
                    if (input == null || input.Trim() == "!end") break;
                    lines.Add(input);
                }

                string taskText = string.Join("\n", lines);
                AddToArrays(todos, statuses, dates, ref count, taskText);
                Console.WriteLine("Многострочная задача добавлена!");
                return;
            }

            if (string.IsNullOrWhiteSpace(line))
            {
                Console.WriteLine("Ошибка: не введён текст задачи");
                return;
            }

            AddToArrays(todos, statuses, dates, ref count, line.Trim());
            Console.WriteLine("Задача добавлена!");
        }

        static void AddToArrays(string[] todos, bool[] statuses, DateTime[] dates, ref int count, string text)
        {
            if (count >= todos.Length)
                ExpandArrays(ref todos, ref statuses, ref dates);

            todos[count] = text;
            statuses[count] = false;
            dates[count] = DateTime.Now;
            count++;
        }

        static void ExpandArrays(ref string[] todos, ref bool[] statuses, ref DateTime[] dates)
        {
            string[] newTodos = new string[todos.Length * 2];
            bool[] newStatuses = new bool[statuses.Length * 2];
            DateTime[] newDates = new DateTime[dates.Length * 2];

            for (int i = 0; i < todos.Length; i++)
            {
                newTodos[i] = todos[i];
                newStatuses[i] = statuses[i];
                newDates[i] = dates[i];
            }

            todos = newTodos;
            statuses = newStatuses;
            dates = newDates;
        }

        static void ViewTasks(string[] todos, bool[] statuses, DateTime[] dates, int count, HashSet<string> flags)
        {
            if (count == 0)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }

            Console.WriteLine("Ваши задачи: ");
            for (int i = 0; i < count; i++)
            {
                string statusText = statuses[i] ? "сделано" : "не сделано";
                Console.WriteLine($"{i + 1}. {todos[i]} - {statusText} - {dates[i]}");
            }
        }

        static void MarkTaskDone(bool[] statuses, DateTime[] dates, int count, string line)
        {
            if (!int.TryParse(line, out int idx))
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }

            idx--;
            if (idx >= 0 && idx < count)
            {
                statuses[idx] = true;
                dates[idx] = DateTime.Now;
                Console.WriteLine("Задача выполнена");
            }
            else Console.WriteLine("Ошибка: некорректный номер задачи");
        }

        static void DeleteTask(string[] todos, bool[] statuses, DateTime[] dates, ref int count, string line)
        {
            if (!int.TryParse(line, out int idx))
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }

            
            idx--;
            if (idx >= 0 && idx < count)
            {
                for (int i = idx; i < count - 1; i++)
                {
                    todos[i] = todos[i + 1];
                    statuses[i] = statuses[i + 1];
                    dates[i] = dates[i + 1];
                }
                count--;
                Console.WriteLine("Задача удалена");
            }
            else Console.WriteLine("Ошибка: некорректный номер задачи");
        }

        static void UpdateTask(string[] todos, DateTime[] dates, int count, string line)
        {
            var parts = line.Split(' ', 2);
            if (parts.Length < 2 || !int.TryParse(parts[0], out int idx))
            {
                Console.WriteLine("Ошибка: укажите номер задачи и текст");
                return;
            }

            idx--;
            if (idx >= 0 && idx < count)
            {
                todos[idx] = parts[1].Trim('"');
                dates[idx] = DateTime.Now;
                Console.WriteLine("Задача обновлена");
            }
            else Console.WriteLine("Ошибка: некорректный номер задачи");
        }
    }
}
