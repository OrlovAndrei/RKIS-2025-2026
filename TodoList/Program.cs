using System;

namespace TodoApp
{
    class Program
    {
        private static Profile userProfile;
        private static TodoList todoList;

        static void Main()
        {
            Console.WriteLine("выполнил работу Турищев Иван");
            InitializeApplication();
            RunCommandLoop();
        }

        static void InitializeApplication()
        {
            Console.WriteLine("=== ПРИЛОЖЕНИЕ ДЛЯ УПРАВЛЕНИЯ ЗАДАЧАМИ ===");
            
            // Создание профиля пользователя
            userProfile = Profile.CreateFromInput();
            todoList = new TodoList();

            Console.WriteLine($"\nДобро пожаловать, {userProfile.FirstName}!");
            Console.WriteLine("Введите 'help' для списка команд.");
        }

        static void RunCommandLoop()
        {
            while (true)
            {
                Console.Write("\n> ");
                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input))
                    continue;

                ProcessCommand(input);
            }
        }

        static void ProcessCommand(string input)
        {
            string[] parts = input.Split(' ', 2);
            string command = parts[0].ToLower();
            string argument = parts.Length > 1 ? parts[1] : "";

            switch (command)
            {
                case "help":
                    ShowHelp();
                    break;
                case "modify":
                    ShowProfile();
                    break;
                case "add":
                    ExecuteAddCommand(argument);
                    break;
                case "done":
                    ExecuteDoneCommand(argument);
                    break;
                case "update":
                    ExecuteUpdateCommand(argument);
                    break;
                case "view":
                    ExecuteViewCommand(argument);
                    break;
                case "read":
                    ExecuteReadCommand(argument);
                    break;
                case "remove":
                    ExecuteRemoveCommand(argument);
                    break;
                case "exit":
                    Console.WriteLine("До свидания!");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
                    break;
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine("\n=== СПРАВКА ПО КОМАНДАМ ===");
            Console.WriteLine("help              - показать все команды");
            Console.WriteLine("modify            - показать профиль пользователя");
            Console.WriteLine("add \"текст\"       - добавить новую задачу");
            Console.WriteLine("view [флаги]      - просмотреть список задач (флаги: -i, -s, -d)");
            Console.WriteLine("done <idx>        - отметить задачу как выполненную");
            Console.WriteLine("update <idx> \"текст\" - обновить текст задачи");
            Console.WriteLine("read <idx>        - показать полную информацию о задаче");
            Console.WriteLine("remove <idx>      - удалить задачу");
            Console.WriteLine("exit              - выход из программы");
            Console.WriteLine("\nФлаги для команды view:");
            Console.WriteLine("-i  - показывать номера задач");
            Console.WriteLine("-s  - показывать статус выполнения");
            Console.WriteLine("-d  - показывать дату изменения");
        }

        static void ShowProfile()
        {
            Console.WriteLine("\n=== ВАШ ПРОФИЛЬ ===");
            Console.WriteLine(userProfile.GetInfo());
        }

        static void ExecuteAddCommand(string argument)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                Console.Write("Введите текст задачи: ");
                argument = Console.ReadLine()?.Trim();
            }

            if (!string.IsNullOrWhiteSpace(argument))
            {
                // Убираем кавычки если они есть
                string taskText = argument.StartsWith("\"") && argument.EndsWith("\"") 
                    ? argument.Substring(1, argument.Length - 2) 
                    : argument;

                // Создаем объект TodoItem и добавляем его в TodoList
                TodoItem newTask = new TodoItem(taskText);
                todoList.Add(newTask);
                Console.WriteLine(" Задача успешно добавлена!");
            }
            else
            {
                Console.WriteLine(" Ошибка: текст задачи не может быть пустым!");
            }
        }

        static void ExecuteDoneCommand(string argument)
        {
            if (todoList.IsEmpty)
            {
                Console.WriteLine(" Список задач пуст!");
                return;
            }

            if (int.TryParse(argument, out int taskIndex))
            {
                try
                {
                    // Получаем задачу через GetItem() и вызываем метод MarkDone()
                    TodoItem task = todoList.GetItem(taskIndex - 1);
                    
                    if (task.IsDone)
                    {
                        Console.WriteLine("ℹ Эта задача уже отмечена как выполненная.");
                    }
                    else
                    {
                        task.MarkDone();
                        Console.WriteLine($" Задача #{taskIndex} отмечена как выполненная!");
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine($" Ошибка: задача с номером {taskIndex} не найдена!");
                }
            }
            else
            {
                Console.WriteLine(" Ошибка: неверный формат команды. Используйте: done <номер_задачи>");
            }
        }

        static void ExecuteUpdateCommand(string argument)
        {
            if (todoList.IsEmpty)
            {
                Console.WriteLine(" Список задач пуст!");
                return;
            }

            // Разбираем аргументы: номер задачи и новый текст
            string[] parts = argument.Split(' ', 2);
            if (parts.Length < 2)
            {
                Console.WriteLine(" Ошибка: неверный формат команды. Используйте: update <номер_задачи> \"новый текст\"");
                return;
            }

            if (int.TryParse(parts[0], out int taskIndex))
            {
                try
                {
                    string newText = parts[1].Trim();
                    // Убираем кавычки если они есть
                    if (newText.StartsWith("\"") && newText.EndsWith("\""))
                    {
                        newText = newText.Substring(1, newText.Length - 2);
                    }

                    if (!string.IsNullOrWhiteSpace(newText))
                    {
                        // Получаем задачу через GetItem() и вызываем метод UpdateText()
                        TodoItem task = todoList.GetItem(taskIndex - 1);
                        task.UpdateText(newText);
                        Console.WriteLine($" Задача #{taskIndex} успешно обновлена!");
                    }
                    else
                    {
                        Console.WriteLine(" Ошибка: текст задачи не может быть пустым!");
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine($" Ошибка: задача с номером {taskIndex} не найдена!");
                }
            }
            else
            {
                Console.WriteLine(" Ошибка: неверный номер задачи!");
            }
        }

        static void ExecuteViewCommand(string argument)
        {
            if (todoList.IsEmpty)
            {
                Console.WriteLine(" Список задач пуст!");
                return;
            }

            // Парсим флаги
            bool showIndex = true;
            bool showStatus = true;
            bool showDate = true;

            if (!string.IsNullOrEmpty(argument))
            {
                string[] flags = argument.Split(' ');
                foreach (string flag in flags)
                {
                    if (flag == "-i") showIndex = true;
                    if (flag == "-s") showStatus = false;
                    if (flag == "-d") showDate = false;
                }
            }

            // Вызываем метод View() для вывода задач в виде таблицы
            Console.WriteLine("\n=== ВАШИ ЗАДАЧИ ===");
            todoList.View(showIndex, showStatus, showDate);
        }

        static void ExecuteReadCommand(string argument)
        {
            if (todoList.IsEmpty)
            {
                Console.WriteLine(" Список задач пуст!");
                return;
            }

            if (int.TryParse(argument, out int taskIndex))
            {
                try
                {
                    // Получаем задачу через GetItem() и вызываем метод GetFullInfo()
                    TodoItem task = todoList.GetItem(taskIndex - 1);
                    Console.WriteLine("\n" + new string('=', 60));
                    Console.WriteLine($"ПОЛНАЯ ИНФОРМАЦИЯ О ЗАДАЧЕ #{taskIndex}");
                    Console.WriteLine(new string('=', 60));
                    Console.WriteLine(task.GetFullInfo());
                    Console.WriteLine(new string('=', 60));
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine($" Ошибка: задача с номером {taskIndex} не найдена!");
                }
            }
            else
            {
                Console.WriteLine(" Ошибка: неверный формат команды. Используйте: read <номер_задачи>");
            }
        }

        static void ExecuteRemoveCommand(string argument)
        {
            if (todoList.IsEmpty)
            {
                Console.WriteLine(" Список задач пуст!");
                return;
            }

            if (int.TryParse(argument, out int taskIndex))
            {
                try
                {
                    TodoItem task = todoList.GetItem(taskIndex - 1);
                    string shortText = GetShortText(task.Text);
                    
                    Console.Write($" Вы уверены, что хотите удалить задачу '{shortText}'? (y/n): ");
                    string confirmation = Console.ReadLine()?.Trim().ToLower();
                    
                    if (confirmation == "y" || confirmation == "yes" || confirmation == "д" || confirmation == "да")
                    {
                        todoList.Delete(taskIndex - 1);
                        Console.WriteLine(" Задача успешно удалена!");
                    }
                    else
                    {
                        Console.WriteLine(" Удаление отменено.");
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine($" Ошибка: задача с номером {taskIndex} не найдена!");
                }
            }
            else
            {
                Console.WriteLine(" Ошибка: неверный формат команды. Используйте: remove <номер_задачи>");
            }
        }

        // Вспомогательный метод для сокращения текста задачи
        private static string GetShortText(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            string shortText = text.Replace("\n", " ").Replace("\r", " ");
            return shortText.Length > 30 ? shortText.Substring(0, 30) + "..." : shortText;
        }
    }
}
