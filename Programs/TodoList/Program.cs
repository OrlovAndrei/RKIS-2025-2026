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
                        ProcessViewCommand(parts);
                        break;
                    case "read":
                        ProcessReadCommand(parts);
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
            Console.WriteLine("view - показать только текст задачи");
            Console.WriteLine("view --index или -i - показать с индексами");
            Console.WriteLine("view --status или -s - показать со статусами");
            Console.WriteLine("view --update-date или -d - показать дату последнего изменения");
            Console.WriteLine("view --all или -a - показать все данные");
            Console.WriteLine("read <номер> - просмотреть полный текст задачи");
            Console.WriteLine("done <номер> - отметить задачу как выполненную");
            Console.WriteLine("delete <номер> - удалить задачу");
            Console.WriteLine("update <номер> \"новый текст\" - обновить текст задачи");
            Console.WriteLine("exit - выход из программы");
        }
        static void ShowProfile(string? name, string? secondName, int birthYear)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(secondName))
            {
                Console.WriteLine("Данные пользователя не заполнены");
                return;
            }

            Console.WriteLine($"Имя: {name}");
            Console.WriteLine($"Фамилия: {secondName}");
            Console.WriteLine($"Год рождения: {birthYear}");
        }
        static void ProcessAddCommand(string[] parts)
        {
            bool multilineMode = false;
            for (int i = 1; i < parts.Length; i++)
            {
                if (!string.IsNullOrEmpty(parts[i]) &&
                    (parts[i] == "--multiline" || parts[i] == "-m"))
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
                    AddTodo(task);
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
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
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
        static void ProcessViewCommand(string[] parts)
        {
            bool showIndex = false;
            bool showStatus = false;
            bool showDate = false; 
            bool showAll = false;
            for (int i = 1; i < parts.Length; i++)
            {
                string flag = parts[i];
                if (flag == "--all" || flag == "-a")
                {
                    showAll = true;
                }
                else if (flag == "--index" || flag == "-i")
                {
                    showIndex = true;
                }
                else if (flag == "--status" || flag == "-s")
                {
                    showStatus = true;
                }
                else if (flag == "--update-date" || flag == "-d")
                {
                    showDate = true;
                }
                else if (flag.StartsWith("-") && flag.Length > 1 && !flag.StartsWith("--"))
                {
                    foreach (char c in flag.Substring(1))
                    {
                        switch (c)
                        {
                            case 'i': showIndex = true; break;
                            case 's': showStatus = true; break;
                            case 'd': showDate = true; break;
                            case 'a': showAll = true; break;
                        }
                    }
                }
            }
            if (showAll)
            {
                showIndex = true;
                showStatus = true;
                showDate = true;
            }
            ViewTodosWithFlags (showIndex, showStatus, showDate);
        }
        static void ViewTodosWithFlags(bool showIndex, bool showStatus, bool showDate)
        {
            if (todoCount == 0)
            {
                Console.WriteLine("Список пуст");
                return;
            }
            int indexWidth = showIndex ? 8 : 0;
            int statusWidth = showStatus ? 12 : 0;
            int dateWidth = showDate ? 20 : 0;
            int textWidth = 30;

            string header = "";
            if (showIndex) header += "Индекс".PadRight(indexWidth);
            if (showStatus) header += "Статус".PadRight(statusWidth);
            if (showDate) header += "Дата изменения".PadRight(dateWidth);
            header += "Задача";

            Console.WriteLine(header);
            Console.WriteLine(new string('-', header.Length));

            for (int i = 0; i < todoCount; i++)
            {
                string line = "";
                if (showIndex)
                {
                    line += $"{i + 1}".PadRight(indexWidth);
                }
                if (showStatus)
                {
                    string status = statuses[i] ? "Сделано" : "Не сделано";
                    line += status.PadRight(statusWidth);
                }
                if (showDate)
                {
                    string date = dates[i].ToString("dd.MM.yyyy HH:mm");
                    line += date.PadRight(dateWidth);
                }

                string taskText = todos[i]?.Replace("\n", " ") ?? "";
                if (taskText.Length > textWidth)
                {
                    taskText = taskText.Substring(0, textWidth - 5) + ".....";
                }
                line += taskText;

                Console.WriteLine(line);
            }
        }
        static void ProcessReadCommand(string[] parts)
        {
            if (parts.Length < 2)
            {
                Console.WriteLine("Ошибка: не указан номер задачи");
                return;
            }

            string? numberStr = parts[1];
            if (string.IsNullOrEmpty(numberStr))
            {
                Console.WriteLine("Ошибка: номер задачи не может быть пустым");
                return;
            }

            ReadTodo(numberStr);
        }
        static void ReadTodo(string numberStr)
        {
            if (int.TryParse(numberStr, out int number) && number > 0 && number <= todoCount)
            {
                int index = number - 1;

                Console.WriteLine("=======================================");
                Console.WriteLine($"Задача #{number}");
                Console.WriteLine("=======================================");

                // Полный текст задачи
                string? task = todos[index];
                if (!string.IsNullOrEmpty(task))
                {
                    Console.WriteLine("Полный текст:");
                    Console.WriteLine(task);
                }
                else
                {
                    Console.WriteLine("Текст задачи отсутствует");
                }

                Console.WriteLine();

                // Статус
                string status = statuses[index] ? "✓ Выполнена" : "✗ Не выполнена";
                Console.WriteLine($"Статус: {status}");

                // Дата изменения
                Console.WriteLine($"Дата последнего изменения: {dates[index]:dd.MM.yyyy HH:mm}");
                Console.WriteLine("=======================================");
            }
            else
            {
                Console.WriteLine("Неверный номер задачи");
            }
        }

        static void AddTodo(string task)
        {
            if (string.IsNullOrWhiteSpace(task))
            {
                Console.WriteLine("Ошибка: задача не может быть пустой");
                return;
            }

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
            ViewTodosWithFlags(false, false, false);
        }
        static void DoneTodo(string numberStr)
        {
            if (int.TryParse(numberStr, out int number) && number > 0 && number <= todoCount)
            {
                int index = number - 1;
                statuses[index] = true;
                dates[index] = DateTime.Now; // обновляем дату при изменении статуса
                string? task = todos[index];
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
            if (string.IsNullOrWhiteSpace(newText))
            {
                Console.WriteLine("Ошибка: новый текст не может быть пустым");
                return;
            }
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