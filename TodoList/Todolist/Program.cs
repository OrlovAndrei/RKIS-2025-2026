using System;
using System.Collections.Generic;

namespace Todolist
{
    class Program
    {
        private static TodoList todoList;
        private static Profile userProfile;

        static void Main()
        {
            InitializeUser();
            InitializeTodoList();
            RunTodoList();
        }

        static void InitializeUser()
        {
            Console.WriteLine("Работу выполнили Дайнеко и Гергель 3832");
            Console.WriteLine();

            Console.Write("Введите имя: ");
            string firstName = Console.ReadLine();

            Console.Write("Введите фамилию: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите год рождения: ");
            int birthYear = int.Parse(Console.ReadLine());

            userProfile = new Profile(firstName, lastName, birthYear);
            Console.WriteLine($"Добавлен пользователь: {userProfile.GetInfo()}");
            Console.WriteLine();
        }

        static void InitializeTodoList()
        {
            todoList = new TodoList();
        }

        static void RunTodoList()
        {
            while (true)
            {
                Console.Write("Введите команду (help - список команд): ");
                string input = Console.ReadLine();

                if (input == "help")
                {
                    ShowHelp();
                }
                else if (input == "profile")
                {
                    ShowProfile();
                }
                else if (input.StartsWith("add"))
                {
                    AddTask(input);
                }
                else if (input.StartsWith("view"))
                {
                    ViewTasks(input);
                }
                else if (input.StartsWith("done "))
                {
                    MarkTaskDone(input);
                }
                else if (input.StartsWith("delete "))
                {
                    DeleteTask(input);
                }
                else if (input.StartsWith("update "))
                {
                    UpdateTask(input);
                }
                else if (input.StartsWith("read "))
                {
                    ReadTask(input);
                }
                else if (input == "exit")
                {
                    Console.WriteLine("Выход из программы...");
                    break;
                }
                else
                {
                    Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
                }

                Console.WriteLine();
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help - вывести список команд");
            Console.WriteLine("profile - показать данные пользователя");
            Console.WriteLine("add \"текст задачи\" - добавить задачу (однострочный режим)");
            Console.WriteLine("add --multiline (-m) - добавить задачу (многострочный режим)");
            Console.WriteLine("view [флаги] - показать задачи");
            Console.WriteLine("  Флаги: --index (-i), --status (-s), --update-date (-d), --all (-a)");
            Console.WriteLine("done <номер> - отметить задачу выполненной");
            Console.WriteLine("delete <номер> - удалить задачу");
            Console.WriteLine("update <номер> \"новый текст\" - изменить задачу");
            Console.WriteLine("read <номер> - посмотреть полный текст задачи");
            Console.WriteLine("exit - выйти из программы");
        }

        static void ShowProfile()
        {
            Console.WriteLine(userProfile.GetInfo());
        }

        static void AddTask(string input)
        {
            if (input.Contains("--multiline") || input.Contains("-m"))
            {
                AddMultilineTask();
            }
            else
            {
                AddSingleLineTask(input);
            }
        }

        static void AddSingleLineTask(string input)
        {
            string[] parts = input.Split('"');
            if (parts.Length >= 2)
            {
                string task = parts[1];
                TodoItem newItem = new TodoItem(task);
                todoList.Add(newItem);
                Console.WriteLine($"Задача добавлена: {task}");
            }
            else
            {
                Console.WriteLine("Ошибка: используйте формат add \"текст задачи\" или add --multiline");
            }
        }

        static void AddMultilineTask()
        {
            Console.WriteLine("Многострочный режим. Вводите задачи (для завершения введите '!end'):");

            List<string> lines = new List<string>();
            while (true)
            {
                string line = Console.ReadLine();
                if (line?.ToLower() == "!end")
                    break;

                if (!string.IsNullOrEmpty(line))
                    lines.Add(line);
            }

            if (lines.Count > 0)
            {
                string task = string.Join("\n", lines);
                TodoItem newItem = new TodoItem(task);
                todoList.Add(newItem);
                Console.WriteLine($"Добавлена многострочная задача ({lines.Count} строк)");
            }
            else
            {
                Console.WriteLine("Не добавлено ни одной строки");
            }
        }

        static void ViewTasks(string input)
        {
            bool showIndex = input.Contains("--index") || input.Contains("-i");
            bool showStatus = input.Contains("--status") || input.Contains("-s");
            bool showDate = input.Contains("--update-date") || input.Contains("-d");
            bool showAll = input.Contains("--all") || input.Contains("-a");

            if (showAll)
            {
                showIndex = true;
                showStatus = true;
                showDate = true;
            }

            todoList.View(showIndex, showStatus, showDate);
        }

        static void MarkTaskDone(string input)
        {
            if (int.TryParse(input.Substring(5), out int index))
            {
                int taskIndex = index - 1;
                TodoItem item = todoList.GetItem(taskIndex);
                if (item != null)
                {
                    item.MarkDone();
                    Console.WriteLine($"Задача {index} отмечена как выполненная");
                }
                else
                {
                    Console.WriteLine("Ошибка: неверный номер задачи");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: используйте формат done <номер>");
            }
        }

        static void DeleteTask(string input)
        {
            if (int.TryParse(input.Substring(7), out int index))
            {
                int taskIndex = index - 1;
                TodoItem item = todoList.GetItem(taskIndex);
                if (item != null)
                {
                    string deletedText = item.Text;
                    todoList.Delete(taskIndex);
                    Console.WriteLine($"Задача удалена: {deletedText}");
                }
                else
                {
                    Console.WriteLine("Ошибка: неверный номер задачи");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: используйте формат delete <номер>");
            }
        }

        static void UpdateTask(string input)
        {
            string[] parts = input.Split('"');
            if (parts.Length >= 2)
            {
                string numberPart = parts[0].Substring(7).Trim();
                if (int.TryParse(numberPart, out int index))
                {
                    int taskIndex = index - 1;
                    TodoItem item = todoList.GetItem(taskIndex);
                    if (item != null)
                    {
                        string newText = parts[1];
                        string oldText = item.Text;
                        item.UpdateText(newText);
                        Console.WriteLine($"Задача обновлена: '{oldText}' -> '{newText}'");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: неверный номер задачи");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка: используйте формат update <номер> \"новый текст\"");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: используйте формат update <номер> \"новый текст\"");
            }
        }

        static void ReadTask(string input)
        {
            if (int.TryParse(input.Substring(5).Trim(), out int index))
            {
                int taskIndex = index - 1;
                TodoItem item = todoList.GetItem(taskIndex);
                if (item != null)
                {
                    Console.WriteLine($"=== Задача {index} ===");
                    Console.WriteLine(item.GetFullInfo());
                }
                else
                {
                    Console.WriteLine("Ошибка: неверный номер задачи");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: используйте формат read <номер>");
            }
        }
    }
}

