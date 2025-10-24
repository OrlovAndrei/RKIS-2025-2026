using System;
using System.Collections.Generic;

namespace Todolist
{
    class Program
    {
        private static TodoList taskManager = new TodoList();
        private static Profile userProfile = new Profile();

        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Шегрикян и Агулов");
            InitializeUser();
            Console.WriteLine("Добро пожаловать в системе управления задачами!");
            Console.WriteLine("Введите 'help' для списка команд");
            RunMainLoop();
        }

        static void InitializeUser()
        {
            Console.Write("Введите ваше имя: ");
            string firstName = ReadNotEmptyInput("имя");
            
            Console.Write("Введите вашу фамилию: ");
            string lastName = ReadNotEmptyInput("фамилию");

            Console.Write("Введите ваш год рождения: ");
            int birthYear = ReadValidBirthYear();
            
            userProfile = new Profile(firstName, lastName, birthYear);
            Console.WriteLine($"Добавлен пользователь: {userProfile.GetInfo()}");
        }

        static string ReadNotEmptyInput(string fieldName)
        {
            while (true)
            {
                string input = Console.ReadLine() ?? "";
                if (!string.IsNullOrWhiteSpace(input)) return input.Trim();
                Console.Write($"{fieldName} не может быть пустым. Введите еще раз: ");
            }
        }

        static int ReadValidBirthYear()
        {
            while (true)
            {
                string input = Console.ReadLine() ?? "";
                if (int.TryParse(input, out int year) && year > 1900 && year <= DateTime.Now.Year) 
                    return year;
                Console.Write($"Год рождения должен быть числом от 1900 до {DateTime.Now.Year}. Введите еще раз: ");
            }
        }

        static void RunMainLoop()
        {
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(input)) continue;
                ExecuteCommand(input.Trim());
            }
        }

        static void ExecuteCommand(string input)
        {
            string command = input.Split(' ')[0].ToLower();
            
            switch (command)
            {
                case "help": ShowHelp(); break;
                case "profile": ShowProfile(); break;
                case "add": AddTask(input); break;
                case "view": ViewTasks(input); break;
                case "read": ReadTask(input); break;
                case "done": MarkTaskDone(input); break;
                case "update": UpdateTask(input); break;
                case "delete": DeleteTask(input); break;
                case "exit": ExitProgram(); break;
                default: ShowUnknownCommand(command); break;
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                       КОМАНДЫ УПРАВЛЕНИЯ                    ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ help                    - показать справку                  ║");
            Console.WriteLine("║ profile                 - данные пользователя               ║");
            Console.WriteLine("║ add <текст>            - добавить задачу                   ║");
            Console.WriteLine("║ add --multiline         - многострочный ввод задачи         ║");
            Console.WriteLine("║ view                    - текст задач (по умолчанию)        ║");
            Console.WriteLine("║ view --index            - с индексами                       ║");
            Console.WriteLine("║ view --status           - со статусами                      ║");
            Console.WriteLine("║ view --update-date      - с датами изменений                ║");
            Console.WriteLine("║ view -a                 - все данные                        ║");
            Console.WriteLine("║ read <номер>            - детали задачи                     ║");
            Console.WriteLine("║ done <номер>            - отметить выполненной              ║");
            Console.WriteLine("║ update <номер> <текст> - обновить текст задачи              ║");
            Console.WriteLine("║ delete <номер>          - удалить задачу                    ║");
            Console.WriteLine("║ exit                    - выход                             ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
        }

        static void ShowProfile()
        {
            Console.WriteLine("╔════════════════════════════════════════════════╗");
            Console.WriteLine("║                ПРОФИЛЬ ПОЛЬЗОВАТЕЛЯ           ║");
            Console.WriteLine("╠════════════════════════════════════════════════╣");
            Console.WriteLine($"║ {userProfile.GetInfo(),-44} ║");
            Console.WriteLine("╠════════════════════════════════════════════════╣");
            Console.WriteLine($"║ Всего задач: {taskManager.Count,-28} ║");
            Console.WriteLine($"║ Выполнено: {taskManager.CompletedCount,-29} ║");
            Console.WriteLine($"║ Осталось: {taskManager.PendingCount,-30} ║");
            Console.WriteLine("╚════════════════════════════════════════════════╝");
        }

        static void AddTask(string input)
        {
            if (input.Contains("--multiline") || input.Contains("-m"))
            {
                AddMultilineTask();
            }
            else
            {
                string taskText = GetTaskText(input);
                if (!string.IsNullOrWhiteSpace(taskText))
                {
                    // Создаем объект TodoItem и добавляем в TodoList
                    TodoItem newTask = new TodoItem(taskText);
                    taskManager.Add(newTask);
                    Console.WriteLine("Задача добавлена!");
                }
            }
        }

        static void AddMultilineTask()
        {
            Console.WriteLine("Многострочный ввод задачи. Вводите строки, для завершения введите !end");
            Console.WriteLine("Введите текст задачи:");

            List<string> lines = new List<string>();
            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine() ?? "";
                if (line.Trim() == "!end") break;
                if (!string.IsNullOrWhiteSpace(line)) lines.Add(line);
            }

            if (lines.Count == 0)
            {
                Console.WriteLine("Задача не добавлена: пустой текст");
                return;
            }

            // Создаем объект TodoItem с многострочным текстом
            TodoItem newTask = new TodoItem(string.Join("\n", lines));
            taskManager.Add(newTask);
            Console.WriteLine("Многострочная задача добавлена!");
        }

        static void ViewTasks(string input)
        {
            bool showIndex = input.Contains("--index") || input.Contains("-i");
            bool showStatus = input.Contains("--status") || input.Contains("-s");
            bool showDate = input.Contains("--update-date") || input.Contains("-d");
            bool showAll = input.Contains("-a");

            if (showAll)
            {
                showIndex = showStatus = showDate = true;
            }

            // Вызываем метод View TodoList с нужными флагами
            taskManager.View(showIndex, showStatus, showDate);
        }

        static void ReadTask(string input)
        {
            int taskNumber = ParseTaskNumber(input);
            if (taskNumber > 0)
            {
                // Получаем задачу через GetItem и вызываем GetFullInfo
                TodoItem task = taskManager.GetItem(taskNumber - 1);
                if (task != null)
                {
                    Console.WriteLine(task.GetFullInfo());
                }
                else
                {
                    Console.WriteLine("Задача не найдена");
                }
            }
        }

        static void MarkTaskDone(string input)
        {
            int taskNumber = ParseTaskNumber(input);
            if (taskNumber > 0)
            {
                // Получаем задачу через GetItem и вызываем MarkDone
                TodoItem task = taskManager.GetItem(taskNumber - 1);
                if (task != null)
                {
                    task.MarkDone();
                    Console.WriteLine($"Задача '{GetShortText(task.Text, 50)}' отмечена как выполненная");
                }
                else
                {
                    Console.WriteLine("Задача не найдена");
                }
            }
        }

        static void UpdateTask(string input)
        {
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            if (parts.Length < 3)
            {
                Console.WriteLine("Ошибка: укажите номер задачи и новый текст");
                Console.WriteLine("Пример: update 1 \"Новый текст задачи\"");
                return;
            }

            if (int.TryParse(parts[1], out int taskNumber) && taskNumber > 0)
            {
                // Получаем задачу через GetItem
                TodoItem task = taskManager.GetItem(taskNumber - 1);
                if (task != null)
                {
                    // Извлекаем новый текст (объединяем все части после номера)
                    string newText = string.Join(" ", parts, 2, parts.Length - 2);
                    
                    // Убираем кавычки если они есть
                    if (newText.StartsWith("\"") && newText.EndsWith("\""))
                    {
                        newText = newText.Substring(1, newText.Length - 2);
                    }

                    if (!string.IsNullOrWhiteSpace(newText))
                    {
                        string oldText = task.Text;
                        task.UpdateText(newText);
                        Console.WriteLine($"Задача обновлена: '{GetShortText(oldText, 30)}' -> '{GetShortText(newText, 30)}'");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: новый текст не может быть пустым");
                    }
                }
                else
                {
                    Console.WriteLine("Задача не найдена");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: укажите корректный номер задачи");
            }
        }

        static void DeleteTask(string input)
        {
            int taskNumber = ParseTaskNumber(input);
            if (taskNumber > 0)
            {
                // Получаем задачу для отображения перед удалением
                TodoItem task = taskManager.GetItem(taskNumber - 1);
                if (task != null)
                {
                    string taskText = GetShortText(task.Text, 50);
                    taskManager.Delete(taskNumber - 1);
                    Console.WriteLine($"Задача '{taskText}' удалена!");
                }
                else
                {
                    Console.WriteLine("Задача не найдена");
                }
            }
        }

        static void ExitProgram()
        {
            Console.WriteLine("Выход из программы...");
            Environment.Exit(0);
        }

        static void ShowUnknownCommand(string command)
        {
            Console.WriteLine($"Неизвестная команда: {command}");
            Console.WriteLine("Введите 'help' для списка команд");
        }

        static string GetTaskText(string input)
        {
            if (!input.ToLower().StartsWith("add "))
            {
                Console.WriteLine("Ошибка: используйте 'add <текст задачи>'");
                return "";
            }

            string taskText = input.Substring(4).Trim();
            if (string.IsNullOrWhiteSpace(taskText))
            {
                Console.WriteLine("Ошибка: текст задачи не может быть пустым");
                return "";
            }

            return taskText;
        }

        static int ParseTaskNumber(string input)
        {
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            if (parts.Length < 2)
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return 0;
            }

            if (int.TryParse(parts[1], out int number) && number > 0) return number;

            Console.WriteLine("Ошибка: номер задачи должен быть положительным числом");
            return 0;
        }

        static string GetShortText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return text.Length <= maxLength ? text : text.Substring(0, maxLength - 3) + "...";
        }
    }
}