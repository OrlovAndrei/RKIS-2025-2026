using System;

namespace Todolist
{
    class Program
    {
        private const int InitialCapacity = 2;
        private const int GrowthFactor = 2;
        
        static string[] todos = new string[InitialCapacity];
        static bool[] statuses = new bool[InitialCapacity];
        static DateTime[] dates = new DateTime[InitialCapacity];
        static int todoCount = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Тревога и Назаретьянц");
            
            CreateUserProfile();
            
            Console.WriteLine("Добро пожаловать в систему управления задачами!");
            Console.WriteLine("Введите 'help' для списка команд");

            RunCommandLoop();
        }

        static void CreateUserProfile()
        {
            Console.Write("Введите ваше имя: ");
            string firstName = Console.ReadLine();

            Console.Write("Введите вашу фамилию: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите ваш год рождения: ");
            int birthYear = int.Parse(Console.ReadLine());
            int age = DateTime.Now.Year - birthYear;

            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст – {age}");
        }

        static void RunCommandLoop()
        {
            while (true)
            {
                Console.Write("> ");
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
                    case "add":
                        AddTodo(parts);
                        break;
                    case "view":
                        ViewTodos(parts);
                        break;
                    case "done":
                    case "complete":
                        CompleteTodo(parts);
                        break;
                    case "delete":
                        DeleteTodo(parts);
                        break;
                    case "update":
                        UpdateTodo(parts);
                        break;
                    case "exit":
                        Console.WriteLine("Выход из программы...");
                        return;
                    default:
                        Console.WriteLine($"Неизвестная команда: {command}");
                        break;
                }
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help               - список команд");
            Console.WriteLine("add <задача>       - добавить задачу");
            Console.WriteLine("add -d <задача>    - добавить выполненную задачу");
            Console.WriteLine("view               - показать все задачи");
            Console.WriteLine("view -a            - показать все задачи");
            Console.WriteLine("view -d            - показать выполненные задачи");
            Console.WriteLine("view -u            - показать невыполненные задачи");
            Console.WriteLine("done <idx>         - отметить как выполненную");
            Console.WriteLine("delete <idx>       - удалить задачу");
            Console.WriteLine("update <idx> <текст> - обновить текст");
            Console.WriteLine("exit               - выход");
        }

        static void AddTodo(string[] parts)
        {
            if (parts.Length < 2)
            {
                Console.WriteLine("Ошибка: не указана задача");
                return;
            }

            bool isCompleted = false;
            int taskStartIndex = 1;

            if (parts[1] == "-d" || parts[1] == "--done")
            {
                isCompleted = true;
                taskStartIndex = 2;
                
                if (parts.Length < 3)
                {
                    Console.WriteLine("Ошибка: не указана задача после флага");
                    return;
                }
            }

            string task = string.Join(" ", parts, taskStartIndex, parts.Length - taskStartIndex);
            
            // Расширяем массивы если нужно
            if (todoCount >= todos.Length)
            {
                int newSize = todos.Length * GrowthFactor;
                Array.Resize(ref todos, newSize);
                Array.Resize(ref statuses, newSize);
                Array.Resize(ref dates, newSize);
                Console.WriteLine($"Массивы расширены до {newSize} элементов");
            }

            // Добавляем задачу
            todos[todoCount] = task;
            statuses[todoCount] = isCompleted;
            dates[todoCount] = DateTime.Now;
            todoCount++;

            string status = isCompleted ? "выполненная задача добавлена" : "задача добавлена";
            Console.WriteLine($"{status}!");
        }

        static void ViewTodos(string[] parts)
        {
            string filter = "all"; // all, done, undone
            
            // Обрабатываем флаги
            if (parts.Length > 1)
            {
                switch (parts[1])
                {
                    case "-a":
                    case "--all":
                        filter = "all";
                        break;
                    case "-d":
                    case "--done":
                        filter = "done";
                        break;
                    case "-u":
                    case "--undone":
                        filter = "undone";
                        break;
                    default:
                        Console.WriteLine($"Неизвестный флаг: {parts[1]}");
                        Console.WriteLine("Используйте: -a (все), -d (выполненные), -u (невыполненные)");
                        return;
                }
            }

            int displayedCount = 0;
            
            Console.WriteLine("Список задач:");
            Console.WriteLine(new string('-', 60));
            
            for (int i = 0; i < todoCount; i++)
            {
                // Применяем фильтр
                if (filter == "done" && !statuses[i]) continue;
                if (filter == "undone" && statuses[i]) continue;
                
                string status = statuses[i] ? "Сделано" : "Не сделано";
                string date = dates[i].ToString("dd.MM.yyyy HH:mm");
                Console.WriteLine($"{i + 1}. {todos[i]}");
                Console.WriteLine($"   {status} | {date}");
                displayedCount++;
            }
            
            if (displayedCount == 0)
            {
                string message = filter switch
                {
                    "done" => "Нет выполненных задач",
                    "undone" => "Нет невыполненных задач",
                    _ => "Список задач пуст"
                };
                Console.WriteLine(message);
            }
            else
            {
                string filterText = filter switch
                {
                    "done" => "выполненных",
                    "undone" => "невыполненных",
                    _ => "всех"
                };
                Console.WriteLine($"Показано {displayedCount} {filterText} задач");
            }
            
            Console.WriteLine(new string('-', 60));
        }

        static void CompleteTodo(string[] parts)
        {
            if (parts.Length < 2 || !int.TryParse(parts[1], out int taskNumber))
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }

            int index = taskNumber - 1;
            if (index < 0 || index >= todoCount)
            {
                Console.WriteLine($"Ошибка: задача с номером {taskNumber} не найдена");
                return;
            }

            statuses[index] = true;
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача {taskNumber} отмечена как выполненная!");
        }

        static void DeleteTodo(string[] parts)
        {
            if (parts.Length < 2 || !int.TryParse(parts[1], out int taskNumber))
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }

            int index = taskNumber - 1;
            if (index < 0 || index >= todoCount)
            {
                Console.WriteLine($"Ошибка: задача с номером {taskNumber} не найдена");
                return;
            }

            // Сдвигаем элементы влево
            for (int i = index; i < todoCount - 1; i++)
            {
                todos[i] = todos[i + 1];
                statuses[i] = statuses[i + 1];
                dates[i] = dates[i + 1];
            }

            todoCount--;
            Console.WriteLine($"Задача {taskNumber} удалена!");
        }

        static void UpdateTodo(string[] parts)
        {
            if (parts.Length < 3)
            {
                Console.WriteLine("Ошибка: укажите номер задачи и новый текст");
                return;
            }

            if (!int.TryParse(parts[1], out int taskNumber))
            {
                Console.WriteLine("Ошибка: номер задачи должен быть числом");
                return;
            }

            int index = taskNumber - 1;
            if (index < 0 || index >= todoCount)
            {
                Console.WriteLine($"Ошибка: задача с номером {taskNumber} не найдена");
                return;
            }

            string newTask = string.Join(" ", parts, 2, parts.Length - 2);
            todos[index] = newTask;
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача {taskNumber} обновлена!");
        }
    }
}