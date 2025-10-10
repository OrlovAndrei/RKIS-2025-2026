using System;

namespace Todolist
{
    class Program
    {
        // Константы для конфигурации
        private const int InitialTasksCapacity = 2;
        private const int MinimumBirthYear = 1900;
        private const string TasksFileName = "tasks.txt";
        
        // Команды
        private const string CommandHelp = "help";
        private const string CommandProfile = "profile";
        private const string CommandAdd = "add";
        private const string CommandView = "view";
        private const string CommandComplete = "complete";
        private const string CommandDelete = "delete";
        private const string CommandExit = "exit";

        // Статусы задач
        private const string StatusCompleted = "[✓]";
        private const string StatusPending = "[ ]";

        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Шегрикян и Агулов");
            
            User user = InitializeUser();
            TaskManager taskManager = new TaskManager();
            
            ShowWelcomeMessage();
            RunMainLoop(user, taskManager);
        }

        private struct User
        {
            public string FirstName;
            public string LastName;
            public int BirthYear;
            public int Age;
        }

        private class TaskManager
        {
            public string[] Tasks { get; private set; }
            public bool[] Statuses { get; private set; }
            public DateTime[] Dates { get; private set; }
            public int TaskCount { get; private set; }

            public TaskManager()
            {
                Tasks = new string[InitialTasksCapacity];
                Statuses = new bool[InitialTasksCapacity];
                Dates = new DateTime[InitialTasksCapacity];
                TaskCount = 0;
            }

            public void AddTask(string taskDescription)
            {
                if (TaskCount >= Tasks.Length)
                {
                    ResizeArrays();
                }
                
                Tasks[TaskCount] = taskDescription;
                Statuses[TaskCount] = false; // задача не выполнена по умолчанию
                Dates[TaskCount] = DateTime.Now;
                TaskCount++;
                
                Console.WriteLine("Задача добавлена!");
                SaveTasksToFile();
            }

            public void MarkTaskAsCompleted(int taskIndex)
            {
                if (IsValidTaskIndex(taskIndex))
                {
                    Statuses[taskIndex] = true;
                    Dates[taskIndex] = DateTime.Now; // обновляем дату при изменении
                    Console.WriteLine($"Задача '{Tasks[taskIndex]}' отмечена как выполненная");
                    SaveTasksToFile();
                }
                else
                {
                    Console.WriteLine("Ошибка: неверный номер задачи");
                }
            }

            public void DeleteTask(int taskIndex)
            {
                if (!IsValidTaskIndex(taskIndex))
                {
                    Console.WriteLine("Ошибка: неверный номер задачи");
                    return;
                }

                for (int i = taskIndex; i < TaskCount - 1; i++)
                {
                    Tasks[i] = Tasks[i + 1];
                    Statuses[i] = Statuses[i + 1];
                    Dates[i] = Dates[i + 1];
                }

                Tasks[TaskCount - 1] = null;
                Statuses[TaskCount - 1] = false;
                Dates[TaskCount - 1] = DateTime.MinValue;
                TaskCount--;

                Console.WriteLine("Задача удалена!");
                SaveTasksToFile();
            }

            public void DisplayAllTasks()
            {
                if (TaskCount == 0)
                {
                    Console.WriteLine("Список задач пуст");
                    return;
                }
                
                Console.WriteLine("Список задач:");
                for (int i = 0; i < TaskCount; i++)
                {
                    string status = Statuses[i] ? StatusCompleted : StatusPending;
                    string creationDate = Dates[i].ToString("dd.MM.yyyy HH:mm");
                    Console.WriteLine($"{i + 1}. {status} {Tasks[i]} (создана: {creationDate})");
                }
            }

            public bool IsValidTaskIndex(int taskIndex)
            {
                return taskIndex >= 0 && taskIndex < TaskCount;
            }

            private void ResizeArrays()
            {
                int newCapacity = Tasks.Length * 2;
                
                string[] newTasks = new string[newCapacity];
                bool[] newStatuses = new bool[newCapacity];
                DateTime[] newDates = new DateTime[newCapacity];
                
                for (int i = 0; i < Tasks.Length; i++)
                {
                    newTasks[i] = Tasks[i];
                    newStatuses[i] = Statuses[i];
                    newDates[i] = Dates[i];
                }
                
                Tasks = newTasks;
                Statuses = newStatuses;
                Dates = newDates;
                
                Console.WriteLine($"Массивы расширены до {Tasks.Length} элементов");
            }

            public void LoadTasksFromFile()
            {
                if (!System.IO.File.Exists(TasksFileName))
                    return;

                try
                {
                    string[] lines = System.IO.File.ReadAllLines(TasksFileName);
                    foreach (string line in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            // Формат: task|status|date
                            string[] parts = line.Split('|');
                            if (parts.Length >= 1)
                            {
                                string taskDescription = parts[0];
                                bool status = parts.Length > 1 && bool.Parse(parts[1]);
                                DateTime date = parts.Length > 2 ? DateTime.Parse(parts[2]) : DateTime.Now;
                                
                                AddTaskFromFile(taskDescription, status, date);
                            }
                        }
                    }
                    
                    if (lines.Length > 0)
                    {
                        Console.WriteLine($"Загружено {lines.Length} задач из файла");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при загрузке задач: {ex.Message}");
                }
            }

            private void AddTaskFromFile(string taskDescription, bool status, DateTime date)
            {
                if (TaskCount >= Tasks.Length)
                {
                    ResizeArrays();
                }
                
                Tasks[TaskCount] = taskDescription;
                Statuses[TaskCount] = status;
                Dates[TaskCount] = date;
                TaskCount++;
            }

            private void SaveTasksToFile()
            {
                try
                {
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(TasksFileName))
                    {
                        for (int i = 0; i < TaskCount; i++)
                        {
                            writer.WriteLine($"{Tasks[i]}|{Statuses[i]}|{Dates[i]:O}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при сохранении задач: {ex.Message}");
                }
            }
        }

        private static User InitializeUser()
        {
            User user = new User();
            
            Console.Write("Введите ваше имя: ");
            user.FirstName = Console.ReadLine();

            Console.Write("Введите вашу фамилию: ");
            user.LastName = Console.ReadLine();

            user.BirthYear = GetValidBirthYear();
            user.Age = DateTime.Now.Year - user.BirthYear;

            Console.WriteLine($"Добавлен пользователь {user.FirstName} {user.LastName}, возраст – {user.Age}");
            
            return user;
        }

        private static int GetValidBirthYear()
        {
            while (true)
            {
                Console.Write("Введите ваш год рождения: ");
                string input = Console.ReadLine();
                
                if (int.TryParse(input, out int birthYear) && 
                    birthYear >= MinimumBirthYear && 
                    birthYear <= DateTime.Now.Year)
                {
                    return birthYear;
                }
                Console.WriteLine($"Ошибка: введите корректный год рождения ({MinimumBirthYear}-{DateTime.Now.Year})");
            }
        }

        private static void ShowWelcomeMessage()
        {
            Console.WriteLine("Добро пожаловать в систему управления задачами!");
            Console.WriteLine("Введите 'help' для списка команд");
        }

        private static void RunMainLoop(User user, TaskManager taskManager)
        {
            taskManager.LoadTasksFromFile();
            
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                
                if (string.IsNullOrWhiteSpace(input))
                    continue;
                    
                ProcessCommand(input, user, taskManager);
            }
        }

        private static void ProcessCommand(string input, User user, TaskManager taskManager)
        {
            string[] commandParts = input.Split(' ');
            string command = commandParts[0].ToLower();
            
            switch (command)
            {
                case CommandHelp:
                    ShowHelp();
                    break;
                case CommandProfile:
                    ShowUserProfile(user);
                    break;
                case CommandAdd:
                    HandleAddCommand(commandParts, taskManager);
                    break;
                case CommandView:
                    taskManager.DisplayAllTasks();
                    break;
                case CommandComplete:
                    HandleCompleteCommand(commandParts, taskManager);
                    break;
                case CommandDelete:
                    HandleDeleteCommand(commandParts, taskManager);
                    break;
                case CommandExit:
                    ExitProgram();
                    break;
                default:
                    Console.WriteLine($"Неизвестная команда: {command}");
                    break;
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help     - вывести список команд");
            Console.WriteLine("profile  - показать данные пользователя");
            Console.WriteLine("add      - добавить задачу");
            Console.WriteLine("view     - показать все задачи");
            Console.WriteLine("complete - отметить задачу как выполненную");
            Console.WriteLine("delete   - удалить задачу");
            Console.WriteLine("exit     - выход из программы");
        }

        private static void ShowUserProfile(User user)
        {
            Console.WriteLine($"{user.FirstName} {user.LastName}, {user.BirthYear}");
        }

        private static void HandleAddCommand(string[] commandParts, TaskManager taskManager)
        {
            if (commandParts.Length < 2)
            {
                Console.WriteLine("Ошибка: не указана задача");
                return;
            }

            string taskDescription = string.Join(" ", commandParts, 1, commandParts.Length - 1);
            taskManager.AddTask(taskDescription);
        }

        private static void HandleCompleteCommand(string[] commandParts, TaskManager taskManager)
        {
            if (commandParts.Length < 2 || !int.TryParse(commandParts[1], out int taskNumber))
            {
                Console.WriteLine("Ошибка: укажите корректный номер задачи");
                return;
            }

            int taskIndex = taskNumber - 1;
            taskManager.MarkTaskAsCompleted(taskIndex);
        }

        private static void HandleDeleteCommand(string[] commandParts, TaskManager taskManager)
        {
            if (commandParts.Length < 2 || !int.TryParse(commandParts[1], out int taskNumber))
            {
                Console.WriteLine("Ошибка: укажите корректный номер задачи");
                return;
            }

            int taskIndex = taskNumber - 1;
            taskManager.DeleteTask(taskIndex);
        }

        private static void ExitProgram()
        {
            Console.WriteLine("Выход из программы...");
            Environment.Exit(0);
        }
    }
}