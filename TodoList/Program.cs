using System;
using System.Collections.Generic;

namespace TodoList
{
    class Program
    {
        private static Profile user;
        private static TodoList todoList = new TodoList();

        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Морозов и Прокопенко");
            SetUserData();

            while (true)
            {
                Console.Write("Введите команду: ");
                string fullInput = Console.ReadLine().Trim();

                if (string.IsNullOrEmpty(fullInput))
                    continue;

                string[] partInput = fullInput.Split(' ', 2);
                string command = partInput[0].ToLower();

                switch (command)
                {
                    case "help":
                        ShowHelp();
                        break;

                    case "profile":
                        ShowProfile();
                        break;

                    case "add_user":
                        SetUserData();
                        break;

                    case "add":
                        if (partInput.Length > 1)
                        {
                            AddTask(partInput[1]);
                        }
                        else
                        {
                            Console.WriteLine("Неправильный формат: add \"текст задачи\" или add текст задачи");
                        }
                        break;

                    case "read":
                        if (partInput.Length > 1)
                        {
                            ReadTask(partInput[1]);
                        }
                        else
                        {
                            Console.WriteLine("Неверный формат: read номер_задачи");
                        }
                        break;

                    case "view":
                        string flags = partInput.Length > 1 ? partInput[1] : "";
                        ShowTasks(flags);
                        break;

                    case "done":
                        if (partInput.Length > 1)
                        {
                            MarkTaskAsDone(partInput[1]);
                        }
                        else
                        {
                            Console.WriteLine("Неправильный формат: done номер_задачи");
                        }
                        break;

                    case "delete":
                        if (partInput.Length > 1)
                        {
                            DeleteTask(partInput[1]);
                        }
                        else
                        {
                            Console.WriteLine("Неверный формат: delete номер_задачи");
                        }
                        break;

                    case "update":
                        if (partInput.Length > 1)
                        {
                            UpdateTask(partInput[1]);
                        }
                        else
                        {
                            Console.WriteLine("Неверный формат: update номер_задачи \"новый текст\"");
                        }
                        break;

                    case "exit":
                        return;

                    default:
                        Console.WriteLine($"Неизвестная команда: {command}");
                        Console.WriteLine("Введите 'help' для просмотра доступных команд");
                        break;
                }
            }
        }

        static void MarkTaskAsDone(string taskId)
        {
            if (int.TryParse(taskId, out int taskNumber))
            {
                try
                {
                    var item = todoList.GetItem(taskNumber);
                    item.MarkDone();
                    Console.WriteLine($"Задача №{taskNumber} отмечена как выполненная");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine($"Неверный номер задачи. Должен быть числом от 1 до {todoList.Count}");
            }
        }

        static void DeleteTask(string taskId)
        {
            if (int.TryParse(taskId, out int taskNumber))
            {
                try
                {
                    todoList.Delete(taskNumber);
                    Console.WriteLine($"Задача №{taskNumber} удалена");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine($"Неверный номер задачи. Должен быть числом от 1 до {todoList.Count}");
            }
        }

        static void UpdateTask(string input)
        {
            string[] parts = input.Split(' ', 2);

            if (parts.Length < 2)
            {
                Console.WriteLine("Не указан новый текст задачи");
                return;
            }

            if (int.TryParse(parts[0], out int taskNumber))
            {
                try
                {
                    var item = todoList.GetItem(taskNumber);
                    string newText = parts[1];

                    if (newText.StartsWith("\"") && newText.EndsWith("\""))
                    {
                        newText = newText.Substring(1, newText.Length - 2);
                    }

                    string oldText = item.Text;
                    item.UpdateText(newText);
                    Console.WriteLine($"Задача обновлена: \nБыло: №{taskNumber} \"{oldText}\" \nСтало: №{taskNumber} \"{newText}\"");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine($"Неверный номер задачи. Должен быть числом от 1 до {todoList.Count}");
            }
        }

        static void SetUserData()
        {
            Console.Write("Введите ваше имя: ");
            string name = Console.ReadLine();

            Console.Write("Введите вашу фамилию: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите год вашего рождения: ");
            if (int.TryParse(Console.ReadLine(), out int birthYear))
            {
                user = new Profile(name, lastName, birthYear);
                Console.WriteLine($"Добавлен пользователь: {user.GetInfo()}");
            }
            else
            {
                Console.WriteLine("Ошибка: год рождения должен быть числом");
                SetUserData();
            }
            Console.WriteLine();
        }

        static void ShowHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help — список всех доступных команд");
            Console.WriteLine("profile — данные пользователя");
            Console.WriteLine("add_user — изменить пользователя");
            Console.WriteLine("add — добавить новую задачу");
            Console.WriteLine("read — полный просмотр задачи");
            Console.WriteLine("view — список всех задач");
            Console.WriteLine("done — отметить задачу как выполненную");
            Console.WriteLine("delete — удалить задачу по номеру");
            Console.WriteLine("update — изменение текста задачи");
            Console.WriteLine("exit — завершить программу");
            Console.WriteLine();
            Console.WriteLine("Флаги для команды 'view':");
            Console.WriteLine(" -i, --index — показывать индекс задачи");
            Console.WriteLine(" -s, --status — показывать статус задачи");
            Console.WriteLine(" -d, --update-date — показывать дату изменения");
            Console.WriteLine(" -a, --all — показывать все данные");
        }

        static void ShowProfile()
        {
            if (user != null)
            {
                Console.WriteLine(user.GetInfo());
            }
            else
            {
                Console.WriteLine("Данные пользователя не найдены");
            }
        }

        static void AddTask(string taskText)
        {
            if (taskText == "--multiline" || taskText == "-m")
            {
                AddMultilineTask();
            }
            else
            {
                AddSingleTask(taskText);
            }
        }

        static void AddMultilineTask()
        {
            Console.WriteLine("Многострочный режим. Введите задачи (для завершения введите '!end'):");
            List<string> lines = new List<string>();

            string line;
            while ((line = Console.ReadLine()) != null && line.ToLower() != "!end")
            {
                lines.Add(line);
            }

            if (lines.Count == 0)
            {
                Console.WriteLine("Не было введено ни одной строки");
                return;
            }

            string multilineText = string.Join("\n", lines);
            var item = new TodoItem(multilineText);
            todoList.Add(item);
            Console.WriteLine($"Добавлена многострочная задача №{todoList.Count}:");
            Console.WriteLine(multilineText);
        }

        static void AddSingleTask(string taskText)
        {
            if (taskText.StartsWith("\"") && taskText.EndsWith("\""))
            {
                taskText = taskText.Substring(1, taskText.Length - 2);
            }

            var item = new TodoItem(taskText);
            todoList.Add(item);
            Console.WriteLine($"Добавлена задача №{todoList.Count}: {taskText}");
        }

        static void ReadTask(string taskId)
        {
            if (int.TryParse(taskId, out int taskNumber))
            {
                try
                {
                    var item = todoList.GetItem(taskNumber);
                    Console.WriteLine($"Задача №{taskNumber}:");
                    Console.WriteLine(item.GetFullInfo());
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine($"Неверный номер задачи. Должен быть числом от 1 до {todoList.Count}");
            }
        }

        static void ShowTasks(string flags)
        {
            bool showIndex = flags.Contains("-i") || flags.Contains("--index");
            bool showStatus = flags.Contains("-s") || flags.Contains("--status");
            bool showDate = flags.Contains("-d") || flags.Contains("--update-date");
            bool showAll = flags.Contains("-a") || flags.Contains("--all");

            if (showAll)
            {
                showIndex = showStatus = showDate = true;
            }

            todoList.View(showIndex, showStatus, showDate);
        }
    }
}