using System;

namespace Todolist
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Шегрикян и Агулов");
            
            // Создаем пользователя
            Console.Write("Введите ваше имя: ");
            string firstName = Console.ReadLine();
            
            Console.Write("Введите вашу фамилию: ");
            string lastName = Console.ReadLine();
            
            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}");
            
            // Создаем менеджер задач
            TaskManager taskManager = new TaskManager();
            
            Console.WriteLine("Добро пожаловать в систему управления задачами!");
            Console.WriteLine("Введите 'help' для списка команд");
            
            // Основной цикл
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                
                if (string.IsNullOrWhiteSpace(input))
                    continue;
                    
                ProcessCommand(input, taskManager);
            }
        }

        private class TaskManager
        {
            private string[] tasks;
            private bool[] statuses;
            private int taskCount;

            public TaskManager()
            {
                tasks = new string[10]; // Фиксированный размер
                statuses = new bool[10];
                taskCount = 0;
            }

            public void AddTask(string taskDescription)
            {
                if (taskCount >= tasks.Length)
                {
                    Console.WriteLine("Достигнут лимит задач!");
                    return;
                }
                
                tasks[taskCount] = taskDescription;
                statuses[taskCount] = false;
                taskCount++;
                
                Console.WriteLine("Задача добавлена!");
            }

            public void MarkTaskAsDone(int taskIndex)
            {
                if (taskIndex >= 0 && taskIndex < taskCount)
                {
                    statuses[taskIndex] = true;
                    Console.WriteLine($"Задача '{tasks[taskIndex]}' отмечена как выполненная");
                }
                else
                {
                    Console.WriteLine("Ошибка: неверный номер задачи");
                }
            }

            public void DeleteTask(int taskIndex)
            {
                if (taskIndex < 0 || taskIndex >= taskCount)
                {
                    Console.WriteLine("Ошибка: неверный номер задачи");
                    return;
                }

                string deletedTask = tasks[taskIndex];

                // Сдвигаем задачи
                for (int i = taskIndex; i < taskCount - 1; i++)
                {
                    tasks[i] = tasks[i + 1];
                    statuses[i] = statuses[i + 1];
                }

                taskCount--;
                Console.WriteLine($"Задача '{deletedTask}' удалена!");
            }

            public void DisplayAllTasks()
            {
                if (taskCount == 0)
                {
                    Console.WriteLine("Список задач пуст");
                    return;
                }
                
                Console.WriteLine("Список задач:");
                for (int i = 0; i < taskCount; i++)
                {
                    string status = statuses[i] ? "✓" : " ";
                    Console.WriteLine($"{i + 1}. [{status}] {tasks[i]}");
                }
            }

            public void DisplayCompletedTasks()
            {
                Console.WriteLine("Выполненные задачи:");
                bool found = false;
                
                for (int i = 0; i < taskCount; i++)
                {
                    if (statuses[i])
                    {
                        Console.WriteLine($"{i + 1}. ✓ {tasks[i]}");
                        found = true;
                    }
                }
                
                if (!found)
                {
                    Console.WriteLine("Нет выполненных задач");
                }
            }

            public void DisplayPendingTasks()
            {
                Console.WriteLine("Невыполненные задачи:");
                bool found = false;
                
                for (int i = 0; i < taskCount; i++)
                {
                    if (!statuses[i])
                    {
                        Console.WriteLine($"{i + 1}. □ {tasks[i]}");
                        found = true;
                    }
                }
                
                if (!found)
                {
                    Console.WriteLine("Нет невыполненных задач");
                }
            }
        }

        private static void ProcessCommand(string input, TaskManager taskManager)
        {
            string[] parts = input.Split(' ');
            string command = parts[0].ToLower();
            
            switch (command)
            {
                case "help":
                    ShowHelp();
                    break;
                case "add":
                    HandleAddCommand(parts, taskManager);
                    break;
                case "view":
                    HandleViewCommand(parts, taskManager);
                    break;
                case "done":
                    HandleDoneCommand(parts, taskManager);
                    break;
                case "delete":
                    HandleDeleteCommand(parts, taskManager);
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine($"Неизвестная команда: {command}");
                    break;
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help          - показать это сообщение");
            Console.WriteLine("add           - добавить задачу");
            Console.WriteLine("view          - показать все задачи");
            Console.WriteLine("view -c       - показать выполненные задачи");
            Console.WriteLine("view -p       - показать невыполненные задачи");
            Console.WriteLine("done <номер>  - отметить задачу как выполненную");
            Console.WriteLine("delete <номер> - удалить задачу");
            Console.WriteLine("exit          - выйти из программы");
            Console.WriteLine("Пример: add Купить молоко");
            Console.WriteLine("Пример: view -c");
            Console.WriteLine("Пример: done 1");
        }

        private static void HandleAddCommand(string[] parts, TaskManager taskManager)
        {
            if (parts.Length < 2)
            {
                Console.WriteLine("Ошибка: не указана задача");
                return;
            }

            string task = string.Join(" ", parts, 1, parts.Length - 1);
            taskManager.AddTask(task);
        }

        private static void HandleViewCommand(string[] parts, TaskManager taskManager)
        {
            if (parts.Length == 1)
            {
                // Простой view без флагов
                taskManager.DisplayAllTasks();
            }
            else
            {
                string flag = parts[1].ToLower();
                
                switch (flag)
                {
                    case "-c":
                    case "--completed":
                        taskManager.DisplayCompletedTasks();
                        break;
                    case "-p":
                    case "--pending":
                        taskManager.DisplayPendingTasks();
                        break;
                    case "-h":
                    case "--help":
                        ShowViewHelp();
                        break;
                    default:
                        Console.WriteLine($"Неизвестный флаг: {flag}");
                        Console.WriteLine("Используйте 'view -h' для справки");
                        break;
                }
            }
        }

        private static void ShowViewHelp()
        {
            Console.WriteLine("Флаги команды view:");
            Console.WriteLine("  (без флагов) - показать все задачи");
            Console.WriteLine("  -c, --completed - показать выполненные задачи");
            Console.WriteLine("  -p, --pending   - показать невыполненные задачи");
            Console.WriteLine("  -h, --help      - показать эту справку");
        }

        private static void HandleDoneCommand(string[] parts, TaskManager taskManager)
        {
            if (parts.Length < 2 || !int.TryParse(parts[1], out int taskNumber) || taskNumber < 1)
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }

            taskManager.MarkTaskAsDone(taskNumber - 1);
        }

        private static void HandleDeleteCommand(string[] parts, TaskManager taskManager)
        {
            if (parts.Length < 2 || !int.TryParse(parts[1], out int taskNumber) || taskNumber < 1)
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }

            taskManager.DeleteTask(taskNumber - 1);
        }
    }
}