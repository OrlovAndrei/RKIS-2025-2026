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
        private const string CommandExit = "exit";

        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Шегрикян и Агулов");
            
            User user = InitializeUser();
            string[] tasks = new string[InitialTasksCapacity];
            int taskCount = 0;
            
            LoadTasksFromFile(ref tasks, ref taskCount);
            ShowWelcomeMessage();
            RunMainLoop(user, ref tasks, ref taskCount);
        }

        private struct User
        {
            public string FirstName;
            public string LastName;
            public int BirthYear;
            public int Age;
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

        private static void RunMainLoop(User user, ref string[] tasks, ref int taskCount)
        {
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                
                if (string.IsNullOrWhiteSpace(input))
                    continue;
                    
                ProcessCommand(input, user, ref tasks, ref taskCount);
            }
        }

        private static void ProcessCommand(string input, User user, ref string[] tasks, ref int taskCount)
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
                    HandleAddCommand(commandParts, ref tasks, ref taskCount);
                    break;
                case CommandView:
                    ShowAllTasks(tasks, taskCount);
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
            Console.WriteLine("help    - вывести список команд");
            Console.WriteLine("profile - показать данные пользователя");
            Console.WriteLine("add     - добавить задачу");
            Console.WriteLine("view    - показать все задачи");
            Console.WriteLine("exit    - выход из программы");
        }

        private static void ShowUserProfile(User user)
        {
            Console.WriteLine($"{user.FirstName} {user.LastName}, {user.BirthYear}");
        }

        private static void HandleAddCommand(string[] commandParts, ref string[] tasks, ref int taskCount)
        {
            if (commandParts.Length < 2)
            {
                Console.WriteLine("Ошибка: не указана задача");
                return;
            }

            string taskDescription = string.Join(" ", commandParts, 1, commandParts.Length - 1);
            AddTask(ref tasks, ref taskCount, taskDescription);
        }

        private static void AddTask(ref string[] tasks, ref int taskCount, string taskDescription)
        {
            if (taskCount >= tasks.Length)
            {
                ResizeTasksArray(ref tasks);
            }
            
            tasks[taskCount] = taskDescription;
            taskCount++;
            Console.WriteLine("Задача добавлена!");
            
            SaveTasksToFile(tasks, taskCount);
        }

        private static void ResizeTasksArray(ref string[] tasks)
        {
            int newCapacity = tasks.Length * 2;
            string[] resizedTasks = new string[newCapacity];
            
            for (int i = 0; i < tasks.Length; i++)
            {
                resizedTasks[i] = tasks[i];
            }
            
            tasks = resizedTasks;
            Console.WriteLine($"Массив задач расширен до {tasks.Length} элементов");
        }

        private static void ShowAllTasks(string[] tasks, int taskCount)
        {
            if (taskCount == 0)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }
            
            Console.WriteLine("Список задач:");
            for (int i = 0; i < taskCount; i++)
            {
                Console.WriteLine($"{i + 1}. {tasks[i]}");
            }
        }

        private static void ExitProgram()
        {
            Console.WriteLine("Выход из программы...");
            Environment.Exit(0);
        }

        private static void SaveTasksToFile(string[] tasks, int taskCount)
        {
            try
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(TasksFileName))
                {
                    for (int i = 0; i < taskCount; i++)
                    {
                        writer.WriteLine(tasks[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении задач: {ex.Message}");
            }
        }

        private static void LoadTasksFromFile(ref string[] tasks, ref int taskCount)
        {
            if (!System.IO.File.Exists(TasksFileName))
                return;

            try
            {
                string[] savedTasks = System.IO.File.ReadAllLines(TasksFileName);
                foreach (string task in savedTasks)
                {
                    if (!string.IsNullOrWhiteSpace(task))
                    {
                        AddTask(ref tasks, ref taskCount, task);
                    }
                }
                
                if (savedTasks.Length > 0)
                {
                    Console.WriteLine($"Загружено {savedTasks.Length} задач из файла");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке задач: {ex.Message}");
            }
        }
    }
}
