using System;
using System.Text.RegularExpressions;

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
                tasks = new string[10];
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

                for (int i = taskIndex; i < taskCount - 1; i++)
                {
                    tasks[i] = tasks[i + 1];
                    statuses[i] = statuses[i + 1];
                }

                taskCount--;
                Console.WriteLine($"Задача '{deletedTask}' удалена!");
            }

            public void DisplayTaskDetails(int taskIndex)
            {
                if (taskIndex < 0 || taskIndex >= taskCount)
                {
                    Console.WriteLine("Ошибка: неверный номер задачи");
                    return;
                }

                string status = statuses[taskIndex] ? "Выполнена ✓" : "Не выполнена □";
                
                Console.WriteLine("┌────────────────────────────────────────┐");
                Console.WriteLine("│             ДЕТАЛИ ЗАДАЧИ             │");
                Console.WriteLine("├────────────────────────────────────────┤");
                Console.WriteLine($"│ Номер: {taskIndex + 1,-30} │");
                Console.WriteLine($"│ Статус: {status,-28} │");
                Console.WriteLine("├────────────────────────────────────────┤");
                Console.WriteLine("│ Описание:                             │");
                Console.WriteLine($"│ {tasks[taskIndex],-38} │");
                Console.WriteLine("└────────────────────────────────────────┘");
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

            public bool IsValidTaskIndex(int taskIndex)
            {
                return taskIndex >= 0 && taskIndex < taskCount;
            }

            public string GetTaskDescription(int taskIndex)
            {
                return IsValidTaskIndex(taskIndex) ? tasks[taskIndex] : null;
            }

            public bool GetTaskStatus(int taskIndex)
            {
                return IsValidTaskIndex(taskIndex) ? statuses[taskIndex] : false;
            }
        }

        private static void ProcessCommand(string input, TaskManager taskManager)
        {
            string trimmedInput = input.Trim();
            string baseCommand = trimmedInput.Split(' ')[0].ToLower();
            
            switch (baseCommand)
            {
                case "help":
                    ShowHelp();
                    break;
                case "add":
                    HandleAddCommand(trimmedInput, taskManager);
                    break;
                case "view":
                    HandleViewCommand(trimmedInput, taskManager);
                    break;
                case "read":
                    HandleReadCommand(trimmedInput, taskManager);
                    break;
                case "done":
                    HandleDoneCommand(trimmedInput, taskManager);
                    break;
                case "delete":
                    HandleDeleteCommand(trimmedInput, taskManager);
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine($"Неизвестная команда: {baseCommand}");
                    Console.WriteLine("Введите 'help' для списка команд");
                    break;
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                     СИСТЕМА УПРАВЛЕНИЯ ЗАДАЧАМИ              ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ ОСНОВНЫЕ КОМАНДЫ:                                            ║");
            Console.WriteLine("║                                                              ║");
            Console.WriteLine("║  help                   - показать это сообщение             ║");
            Console.WriteLine("║  exit                   - выйти из программы                 ║");
            Console.WriteLine("║                                                              ║");
            Console.WriteLine("║ РАБОТА С ЗАДАЧАМИ:                                           ║");
            Console.WriteLine("║                                                              ║");
            Console.WriteLine("║  add <текст>            - добавить новую задачу              ║");
            Console.WriteLine("║  view                   - показать все задачи                ║");
            Console.WriteLine("║  view -c                - только выполненные задачи          ║");
            Console.WriteLine("║  view -p                - только невыполненные задачи        ║");
            Console.WriteLine("║  read <номер>           - детали задачи                      ║");
            Console.WriteLine("║  read <номер> -d        - подробные детали                   ║");
            Console.WriteLine("║  read <номер> -s        - только статус задачи               ║");
            Console.WriteLine("║  done <номер>           - отметить как выполненную           ║");
            Console.WriteLine("║  delete <номер>         - удалить задачу                     ║");
            Console.WriteLine("║                                                              ║");
            Console.WriteLine("║ ПРИМЕРЫ ИСПОЛЬЗОВАНИЯ:                                       ║");
            Console.WriteLine("║                                                              ║");
            Console.WriteLine("║  add Купить молоко и хлеб                                    ║");
            Console.WriteLine("║  view                     # все задачи                       ║");
            Console.WriteLine("║  view -c                  # только выполненные               ║");
            Console.WriteLine("║  read 1                   # детали задачи 1                  ║");
            Console.WriteLine("║  done 1                   # отметить задачу 1 выполненной    ║");
            Console.WriteLine("║  delete 2                 # удалить задачу 2                 ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
        }

        private static void HandleAddCommand(string input, TaskManager taskManager)
        {
            if (input.ToLower().StartsWith("add "))
            {
                string taskDescription = input.Substring(4).Trim();
                if (!string.IsNullOrWhiteSpace(taskDescription))
                {
                    taskManager.AddTask(taskDescription);
                }
                else
                {
                    Console.WriteLine("Ошибка: не указан текст задачи");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: неверный формат команды add");
                Console.WriteLine("Пример: add Купить молоко");
            }
        }

        private static void HandleViewCommand(string input, TaskManager taskManager)
        {
            string lowerInput = input.ToLower();
            
            if (lowerInput == "view")
            {
                taskManager.DisplayAllTasks();
            }
            else if (lowerInput.Contains(" -c") || lowerInput.Contains(" --completed"))
            {
                taskManager.DisplayCompletedTasks();
            }
            else if (lowerInput.Contains(" -p") || lowerInput.Contains(" --pending"))
            {
                taskManager.DisplayPendingTasks();
            }
            else if (lowerInput.Contains(" -h") || lowerInput.Contains(" --help"))
            {
                ShowViewHelp();
            }
            else
            {
                Console.WriteLine("Неизвестный флаг для команды view");
                ShowViewHelp();
            }
        }

        private static void HandleReadCommand(string input, TaskManager taskManager)
        {
            // Используем Regex для парсинга команды read
            Regex readRegex = new Regex(@"^read\s+(\d+)(?:\s+(-[a-z]|--[a-z]+))?$", RegexOptions.IgnoreCase);
            Match match = readRegex.Match(input);
            
            if (!match.Success)
            {
                Console.WriteLine("Ошибка: неверный формат команды read");
                Console.WriteLine("Пример: read 1");
                Console.WriteLine("Пример: read 2 -d");
                return;
            }

            // Используем TryParse для преобразования номера задачи
            string numberString = match.Groups[1].Value;
            if (!int.TryParse(numberString, out int taskNumber))
            {
                Console.WriteLine("Ошибка: неверный номер задачи");
                return;
            }

            int taskIndex = taskNumber - 1;
            
            if (!taskManager.IsValidTaskIndex(taskIndex))
            {
                Console.WriteLine($"Ошибка: задача с номером {taskNumber} не существует");
                return;
            }

            // Извлекаем флаг (если есть)
            string flag = match.Groups[2].Success ? match.Groups[2].Value.ToLower() : null;

            if (string.IsNullOrEmpty(flag))
            {
                taskManager.DisplayTaskDetails(taskIndex);
            }
            else
            {
                switch (flag)
                {
                    case "-d":
                    case "--details":
                        ShowDetailedTaskInfo(taskManager, taskIndex);
                        break;
                    case "-s":
                    case "--status":
                        ShowTaskStatus(taskManager, taskIndex);
                        break;
                    default:
                        Console.WriteLine($"Неизвестный флаг: {flag}");
                        ShowReadHelp();
                        break;
                }
            }
        }

        private static void HandleDoneCommand(string input, TaskManager taskManager)
        {
            string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            
            if (parts.Length != 2)
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                Console.WriteLine("Пример: done 1");
                return;
            }

            // Используем TryParse для безопасного преобразования
            if (int.TryParse(parts[1], out int taskNumber))
            {
                if (taskNumber > 0)
                {
                    taskManager.MarkTaskAsDone(taskNumber - 1);
                }
                else
                {
                    Console.WriteLine("Ошибка: номер задачи должен быть положительным числом");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: укажите корректный номер задачи (целое число)");
            }
        }

        private static void HandleDeleteCommand(string input, TaskManager taskManager)
        {
            int spaceIndex = input.IndexOf(' ');
            if (spaceIndex == -1)
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                Console.WriteLine("Пример: delete 1");
                return;
            }

            string numberPart = input.Substring(spaceIndex + 1).Trim();
            
            // Используем TryParse для безопасного преобразования
            if (int.TryParse(numberPart, out int taskNumber))
            {
                if (taskNumber > 0)
                {
                    taskManager.DeleteTask(taskNumber - 1);
                }
                else
                {
                    Console.WriteLine("Ошибка: номер задачи должен быть положительным числом");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: укажите корректный номер задачи (целое число)");
            }
        }

        private static void ShowDetailedTaskInfo(TaskManager taskManager, int taskIndex)
        {
            string description = taskManager.GetTaskDescription(taskIndex);
            bool status = taskManager.GetTaskStatus(taskIndex);
            
            Console.WriteLine("┌────────────────────────────────────────┐");
            Console.WriteLine("│          ПОДРОБНЫЕ ДЕТАЛИ              │");
            Console.WriteLine("├────────────────────────────────────────┤");
            Console.WriteLine($"│ Номер задачи: {taskIndex + 1,-24}     │");
            Console.WriteLine($"│ Статус: {(status ? "ВЫПОЛНЕНА" : "НЕ ВЫПОЛНЕНА"),-28} │");
            Console.WriteLine($"│ Индекс в массиве: {taskIndex,-19}     │");
            Console.WriteLine("├────────────────────────────────────────┤");
            Console.WriteLine("│              ОПИСАНИЕ:                 │");
            
            int maxLength = 36;
            for (int i = 0; i < description.Length; i += maxLength)
            {
                int length = Math.Min(maxLength, description.Length - i);
                string line = description.Substring(i, length);
                Console.WriteLine($"│ {line.PadRight(maxLength)} │");
            }
            
            Console.WriteLine("└────────────────────────────────────────┘");
        }

        private static void ShowTaskStatus(TaskManager taskManager, int taskIndex)
        {
            string status = taskManager.GetTaskStatus(taskIndex) ? "✓ Выполнена" : "□ Не выполнена";
            Console.WriteLine($"Статус задачи {taskIndex + 1}: {status}");
        }

        private static void ShowViewHelp()
        {
            Console.WriteLine("Флаги команды view:");
            Console.WriteLine("  (без флагов) - показать все задачи");
            Console.WriteLine("  -c, --completed - показать выполненные задачи");
            Console.WriteLine("  -p, --pending   - показать невыполненные задачи");
            Console.WriteLine("  -h, --help      - показать эту справку");
        }

        private static void ShowReadHelp()
        {
            Console.WriteLine("Флаги команды read:");
            Console.WriteLine("  (без флагов) - основные детали задачи");
            Console.WriteLine("  -d, --details - подробная информация о задаче");
            Console.WriteLine("  -s, --status  - показать только статус задачи");
            Console.WriteLine("  -h, --help    - показать эту справку");
        }
    }
}