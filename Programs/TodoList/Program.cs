using System;

namespace Todolist
{
    class Program
    {
        // Массив в 2 элемента
        static string[] todos = new string[2]; // задачи
        static bool[] statuses = new bool[2]; // true - выполнено, false - не выполнено
        static DateTime[] dates = new DateTime[2]; // даты
        static string firstName = "";
        static string lastName = "";
        static int birthYear = 0;
        static int todoCount = 0;

        static void Main()
        {
            bool isValid = true;
            int currentYear = DateTime.Now.Year;

            Console.Write("Работу сделали Приходько и Бочкарёв\n");
            Console.Write("Введите свое имя: ");
            string? name = Console.ReadLine();
            Console.Write("Введите свою фамилию: ");
            string? secondName = Console.ReadLine();
            Console.Write("Введите свой год рождения: ");
            int birthYear = 0;

            try
            {
                birthYear = int.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                isValid = false;
            }


            if ((isValid == true) && (birthYear <= currentYear))
            {
                int age = currentYear - birthYear;
                Console.WriteLine($"Добавлен пользователь:{name},{secondName},возраст - {age}");
            }
            else
            {
                Console.WriteLine("Неверно введен год рождения"); // это ошибка, которая выводится при неправильном введении года рождения, например если введут "арбуз" или введут год больше 2025
            }
            //ниже код с третьей практической работы

            // инициализация массива задач
            string[] todos = new string[2];
            int todoCount = 0;

            Console.WriteLine("Добро пожаловать в программу");
            Console.WriteLine("Введите 'help' для списка команд");
            while (true)
            {
                Console.WriteLine("=-=-=-=-=-=-=-=");
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
                        ShowProfile(name, secondName, birthYear);
                        break;
                    case "add":
                        ProcessAddCommand(parts);
                        break;
                    case "view":
                        ViewTodos();
                        break;
                    case "done":
                        if (parts.Length < 2)
                        {
                            Console.WriteLine("Ошибка: не указан номер задачи");
                        }
                        else
                        {
                            DoneTodo(parts[1]);
                        }
                        break;
                    case "delete":
                        if (parts.Length < 2)
                        {
                            Console.WriteLine("Ошибка: не указан номер задачи");
                        }
                        else
                        {
                            DeleteTodo(parts[1]);
                        }
                        break;
                    case "update":
                        if (parts.Length < 3)
                        {
                            Console.WriteLine("Ошибка: не указан номер задачи");
                        }
                        else
                        {
                            string newText = string.Join(" ", parts, 2, parts.Length - 2);
                            UpdateTodo(parts[1], newText);
                        }
                        break;
                    case "exit":
                        Console.WriteLine("Выход из программы");
                        return;
                    default:
                        Console.WriteLine($"Неизвестная команда: {command}");
                        break;
                }
            }
        }
        static void ShowHelp()
        {
            Console.WriteLine("Доступные команды");
            Console.WriteLine("help - вывести список команд");
            Console.WriteLine("profile - показать данные пользователя");
            Console.WriteLine("add - добавить задачу (однострочный режим)");
            Console.WriteLine("add --multiline или add -m - добавить задачу (многострочный режим)");
            Console.WriteLine("view - показать задачи");
            Console.WriteLine("done <номер> - отметить задачу как выполненную");
            Console.WriteLine("delete <номер> - удалить задачу");
            Console.WriteLine("update <номер> \"новый текст\" - обновить текст задачи");
            Console.WriteLine("exit - выход из программы");
        }
        static void ShowProfile(string name, string secondName, int birthYear)
        {
            Console.WriteLine($"{name} {secondName} {birthYear}");
        }
        static void ProcessAddCommand(string[] parts)
        {
            bool multilineMode = false;
            for (int i = 1; i < parts.Length; i++)
            {
                if (parts[i] == "--multiline" || parts[i] == "-m")
                {
                    multilineMode = true;
                    break;
                }
            }
            if (multilineMode)
            {
                AddTodoMultiline();
            }
            else
            {
                if (parts.Length < 2)
                {
                    Console.WriteLine("Ошибка: не указана задача");
                }
                else
                {
                    string task = string.Join(" ", parts, 1, parts.Length - 1);
                }
            }
        }
        static void AddTodoMultiline()
        {
            if (todoCount >= todos.Length)
            {
                ExpandArrays();
                Console.WriteLine($"Массив расширен до {todos.Length} элементов");
            }
            Console.WriteLine("Введите задачу построчно. Для завершения введите '!end':");
            List<string> lines = new List<string>();
            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();
                if (line == "!end")
                {
                    break;
                }
                lines.Add(line);
            }
            if (lines.Count == 0)
            {
                Console.WriteLine("Задача не была добавлена - пустой ввод");
                return;
            }
            string task = string.Join("\n", lines);
            todos[todoCount] = task;
            statuses[todoCount] = false;
            dates[todoCount] = DateTime.Now;
            todoCount++;
            Console.WriteLine("Многострочная задача добавлена");
        }
        static void AddTodo(string task)
        {
            //проверка, нужно ли расширять массив
            if (todoCount >= todos.Length)
            {
            ExpandArrays();
            Console.WriteLine($"Массив расширен до {todos.Length} элементов");
            }

            //здесь добавляется задача и вся информация по ней
            todos[todoCount] = task;
            statuses[todoCount] = false; // по умолчанию задача не выполнена
            dates[todoCount] = DateTime.Now;
            todoCount++;

            Console.WriteLine("Задача добавлена");
            }
        static void ViewTodos()
        {
            if (todoCount == 0)
            {
                Console.WriteLine("Список пуст");
                return;
            }
            Console.WriteLine("Список задач:");
            for (int i = 0; i < todoCount; i++)
            {
                string status = statuses[i] ? "сделано" : "не сделано";
                string date = dates[i].ToString("dd.MM.yyyy HH:mm");
                Console.WriteLine($"{i + 1}. {todos[i]} [{status}] {date}");
            }
        }
        static void DoneTodo(string numberStr)
        {
            if (int.TryParse(numberStr, out int number) && number > 0 && number <= todoCount)
            {
                int index = number - 1;
                statuses[index] = true;
                dates[index] = DateTime.Now; // обновляем дату при изменении статуса
                Console.WriteLine($"Задача '{todos[index]}' выполненна");
            }
            else
            {
                Console.WriteLine("Неверный номер задачи");
            }
        }
        static void DeleteTodo(string numberStr)
        {
            if (int.TryParse(numberStr, out int number) && number > 0 && number <= todoCount)
            {
                int index = number - 1;
                string deletedTask = todos[index];

                // Сдвигаем все элементы после удаляемого влево на одну позицию
                for (int i = index; i < todoCount - 1; i++)
                {
                    todos[i] = todos[i + 1];
                    statuses[i] = statuses[i + 1];
                    dates[i] = dates[i + 1];
                }

                // Очищаем последний элемент
                todos[todoCount - 1] = "";
                statuses[todoCount - 1] = false;
                dates[todoCount - 1] = DateTime.MinValue;

                todoCount--;
                Console.WriteLine($"Задача '{deletedTask}' удалена");
            }
            else
            {
                Console.WriteLine("Неверный номер задачи");
            }
        }
        static void UpdateTodo(string numberStr, string newText)
        {
            if (int.TryParse(numberStr, out int number) && number > 0 && number <= todoCount)
            {
                int index = number - 1;
                string oldTask = todos[index];
                todos[index] = newText;
                dates[index] = DateTime.Now; // обновляем дату при изменении задачи
                Console.WriteLine($"Задача '{oldTask}' обновлена на '{newText}'");
            }
            else
            {
                Console.WriteLine("Неверный номер задачи");
            }
        }

        static void ExpandArrays()
        {
            int newSize = todos.Length * 2;

            // Расширяем массив todos
            string[] newTodos = new string[newSize];
            Array.Copy(todos, newTodos, todos.Length);
            todos = newTodos;

            // Расширяем массив statuses
            bool[] newStatuses = new bool[newSize];
            Array.Copy(statuses, newStatuses, statuses.Length);
            statuses = newStatuses;

            // Расширяем массив dates
            DateTime[] newDates = new DateTime[newSize];
            Array.Copy(dates, newDates, dates.Length);
            dates = newDates;

            Console.WriteLine($"Массивы расширены до {newSize} элементов");
        }
    }
}