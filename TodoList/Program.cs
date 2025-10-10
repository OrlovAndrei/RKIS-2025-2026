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
        private const string CommandDone = "done";
        private const string CommandDelete = "delete";
        private const string CommandUpdate = "update";
        private const string CommandExit = "exit";

        // Статусы задач
        private const string StatusCompleted = "Сделано";
        private const string StatusPending = "Не сделано";

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
            private string[] tasks;
            private bool[] statuses;
            private DateTime[] dates;
            private int taskCount;

            public int TaskCount => taskCount;

            public TaskManager()
            {
                tasks = new string[InitialTasksCapacity];
                statuses = new bool[InitialTasksCapacity];
                dates = new DateTime[InitialTasksCapacity];
                taskCount = 0;
            }

            public void AddTask(string taskDescription)
            {
                EnsureCapacity();
                
                // Синхронное добавление во все три массива
                tasks[taskCount] = taskDescription;
                statuses[taskCount] = false; // задача не выполнена по умолчанию
                dates[taskCount] = DateTime.Now;
                taskCount++;
                
                Console.WriteLine("Задача добавлена!");
                SaveTasksToFile();
            }

            public void MarkTaskAsDone(int taskIndex)
            {
                if (IsValidTaskIndex(taskIndex))
                {
                    statuses[taskIndex] = true;
                    dates[taskIndex] = DateTime.Now; // обновляем дату при изменении статуса
                    Console.WriteLine($"Задача '{tasks[taskIndex]}' отмечена как выполненная");
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

                string deletedTask = tasks[taskIndex];

                // Синхронное удаление из всех трех массивов
                for (int i = taskIndex; i < taskCount - 1; i++)
                {
                    tasks[i] = tasks[i + 1];
                    statuses[i] = statuses[i + 1];
                    dates[i] = dates[i + 1];
                }

                // Синхронная очистка последних элементов
                tasks[taskCount - 1] = null;
                statuses[taskCount - 1] = false;
                dates[taskCount - 1] = DateTime.MinValue;
                taskCount--;

                Console.WriteLine($"Задача '{deletedTask}' удалена!");
                SaveTasksToFile();
            }

            public void UpdateTask(int taskIndex, string newTaskDescription)
            {
                if (!IsValidTaskIndex(taskIndex))
                {
                    Console.WriteLine("Ошибка: неверный номер задачи");
                    return;
                }

                string oldTask = tasks[taskIndex];
                tasks[taskIndex] = newTaskDescription;
                dates[taskIndex] = DateTime.Now; // обновляем дату при изменении задачи

                Console.WriteLine($"Задача обновлена: '{oldTask}' -> '{newTaskDescription}'");
                SaveTasksToFile();
            }

            public void DisplayAllTasks()
            {
                if (taskCount == 0)
                {
                    Console.WriteLine("Список задач пуст");
                    return;
                }
                
                Console.WriteLine("Список задач:");
                Console.WriteLine("┌─────┬──────────────────────────────────┬────────────┬─────────────────────┐");
                Console.WriteLine("│ №   │ Задача                           │ Статус     │ Дата изменения      │");
                Console.WriteLine("├─────┼──────────────────────────────────┼────────────┼─────────────────────┤");
                
                for (int i = 0; i < taskCount; i++)
                {
                    string status = statuses[i] ? StatusCompleted : StatusPending;
                    string date = dates[i].ToString("dd.MM.yyyy HH:mm");
                    string taskText = tasks[i].Length > 30 ? tasks[i].Substring(0, 27) + "..." : tasks[i].PadRight(30);
                    
                    Console.WriteLine($"│ {i + 1,-3} │ {taskText} │ {status,-10} │ {date,-19} │");
                }
                
                Console.WriteLine("└─────┴──────────────────────────────────┴────────────┴─────────────────────┘");
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

            public DateTime GetTaskDate(int taskIndex)
            {
                return IsValidTaskIndex(taskIndex) ? dates[taskIndex] : DateTime.MinValue;
            }

            private void EnsureCapacity()
            {
                if (taskCount >= tasks.Length)
                {
                    ResizeAllArrays();
                }
            }

            private void ResizeAllArrays()
            {
                int newCapacity = tasks.Length * 2;
                
                Console.WriteLine($"Расширение массивов с {tasks.Length} до {newCapacity} элементов...");

                // Синхронное создание новых массивов
                string[] newTasks = new string[newCapacity];
                bool[] newStatuses = new bool[newCapacity];
                DateTime[] newDates = new DateTime[newCapacity];
                
                // Синхронное копирование данных
                for (int i = 0; i < taskCount; i++)
                {
                    newTasks[i] = tasks[i];
                    newStatuses[i] = statuses[i];
                    newDates[i] = dates[i];
                }
                
                // Синхронная замена массивов
                tasks = newTasks;
                statuses = newStatuses;
                dates = newDates;
                
                Console.WriteLine($"Массивы успешно расширены до {tasks.Length} элементов");
            }

            public void LoadTasksFromFile()
            {
                if (!System.IO.File.Exists(TasksFileName))
                    return;

                try
                {
                    string[] lines = System.IO.File.ReadAllLines(TasksFileName);
                    int loadedCount = 0;
                    
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
                                loadedCount++;
                            }
                        }
                    }
                    
                    if (loadedCount > 0)
                    {
                        Console.WriteLine($"Загружено {loadedCount} задач из файла");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при загрузке задач: {ex.Message}");
                }
            }

            private void AddTaskFromFile(string taskDescription, bool status, DateTime date)
            {
                EnsureCapacity();
                
                // Синхронное добавление из файла
                tasks[taskCount] = taskDescription;
                statuses[taskCount] = status;
                dates[taskCount] = date;
                taskCount++;
            }

            private void SaveTasksToFile()
            {
                try
                {
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(TasksFileName))
                    {
                        for (int i = 0; i < taskCount; i++)
                        {
                            // Сохраняем все три атрибута задачи
                            writer.WriteLine($"{tasks[i]}|{statuses[i]}|{dates[i]:O}");
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
                case CommandDone:
                    HandleDoneCommand(commandParts, taskManager);
                    break;
                case CommandDelete:
                    HandleDeleteCommand(commandParts, taskManager);
                    break;
                case CommandUpdate:
                    HandleUpdateCommand(commandParts, taskManager);
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
            Console.WriteLine("done     - отметить задачу как выполненную");
            Console.WriteLine("delete   - удалить задачу");
            Console.WriteLine("update   - обновить текст задачи");
            Console.WriteLine("exit     - выход из программы");
            Console.WriteLine();
            Console.WriteLine("Примеры использования:");
            Console.WriteLine("  add Новая задача");
            Console.WriteLine("  done 1");
            Console.WriteLine("  delete 2");
            Console.WriteLine("  update 3 \"Новый текст задачи\"");
        }

        private static void ShowUserProfile(User user)
        {
            Console.WriteLine($"{user.FirstName} {user.LastName}, {user.BirthYear} (возраст: {user.Age})");
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

        private static void HandleDoneCommand(string[] commandParts, TaskManager taskManager)
        {
            if (commandParts.Length < 2 || !int.TryParse(commandParts[1], out int taskNumber) || taskNumber < 1)
            {
                Console.WriteLine("Ошибка: укажите корректный номер задачи");
                return;
            }

            int taskIndex = taskNumber - 1;
            taskManager.MarkTaskAsDone(taskIndex);
        }

        private static void HandleDeleteCommand(string[] commandParts, TaskManager taskManager)
        {
            if (commandParts.Length < 2 || !int.TryParse(commandParts[1], out int taskNumber) || taskNumber < 1)
            {
                Console.WriteLine("Ошибка: укажите корректный номер задачи");
                return;
            }

            int taskIndex = taskNumber - 1;
            taskManager.DeleteTask(taskIndex);
        }

        private static void HandleUpdateCommand(string[] commandParts, TaskManager taskManager)
        {
            if (commandParts.Length < 3)
            {
                Console.WriteLine("Ошибка: укажите номер задачи и новый текст в кавычках");
                Console.WriteLine("Пример: update 1 \"Новый текст задачи\"");
                return;
            }

            if (!int.TryParse(commandParts[1], out int taskNumber) || taskNumber < 1)
            {
                Console.WriteLine("Ошибка: укажите корректный номер задачи");
                return;
            }

            // Объединяем все части после номера задачи для получения текста
            string newTaskDescription = string.Join(" ", commandParts, 2, commandParts.Length - 2);
            
            // Убираем кавычки если они есть
            if (newTaskDescription.StartsWith("\"") && newTaskDescription.EndsWith("\""))
            {
                newTaskDescription = newTaskDescription.Substring(1, newTaskDescription.Length - 2);
            }

            int taskIndex = taskNumber - 1;
            taskManager.UpdateTask(taskIndex, newTaskDescription);
        }

        private static void ExitProgram()
        {
            Console.WriteLine("Выход из программы...");
            Environment.Exit(0);
        }
    }
}