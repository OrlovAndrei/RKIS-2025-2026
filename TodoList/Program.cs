using System;

namespace Todolist
{
    class Program
    {
        // Константы для "магических" значений
        private const int InitialTodoCapacity = 2;
        private const int ArrayGrowthFactor = 2;
        private const int CurrentYear = 2024; // Можно заменить на DateTime.Now.Year если нужна динамичность

        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Тревога и Назаретьянц");
            
            UserProfile user = InitializeUserProfile();
            TodoManager todoManager = new TodoManager(InitialTodoCapacity, ArrayGrowthFactor);
            
            DisplayWelcomeMessage(user);
            RunCommandLoop(user, todoManager);
        }

        private static UserProfile InitializeUserProfile()
        {
            Console.Write("Введите ваше имя: ");
            string firstName = Console.ReadLine();

            Console.Write("Введите вашу фамилию: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите ваш год рождения: ");
            string birthYearInput = Console.ReadLine();

            int birthYear = int.Parse(birthYearInput);
            int age = CurrentYear - birthYear;

            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст – {age}");
            
            return new UserProfile(firstName, lastName, birthYear, age);
        }

        private static void DisplayWelcomeMessage(UserProfile user)
        {
            Console.WriteLine("Добро пожаловать в систему управления задачами!");
            Console.WriteLine("Введите 'help' для списка команд");
        }

        private static void RunCommandLoop(UserProfile user, TodoManager todoManager)
        {
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                
                if (string.IsNullOrWhiteSpace(input))
                    continue;

                CommandResult result = ProcessCommand(input, user, todoManager);
                
                if (result.ShouldExit)
                    break;
            }
        }

        private static CommandResult ProcessCommand(string input, UserProfile user, TodoManager todoManager)
        {
            string[] commandParts = input.Split(' ');
            string command = commandParts[0].ToLower();
            
            switch (command)
            {
                case "help":
                    CommandHandlers.ShowHelp();
                    break;
                case "profile":
                    CommandHandlers.ShowProfile(user);
                    break;
                case "add":
                    CommandHandlers.AddTodo(commandParts, todoManager);
                    break;
                case "view":
                    CommandHandlers.ViewTodos(todoManager);
                    break;
                case "complete":
                    CommandHandlers.CompleteTodo(commandParts, todoManager);
                    break;
                case "delete":
                    CommandHandlers.DeleteTodo(commandParts, todoManager);
                    break;
                case "exit":
                    CommandHandlers.ExitProgram();
                    return new CommandResult(true);
                default:
                    Console.WriteLine($"Неизвестная команда: {command}");
                    break;
            }
            
            return new CommandResult(false);
        }
    }

    // Класс для управления задачами
    public class TodoManager
    {
        private string[] _todos;
        private bool[] _statuses;
        private DateTime[] _dates;
        private int _todoCount;
        private readonly int _arrayGrowthFactor;

        public TodoManager(int initialCapacity, int growthFactor)
        {
            _todos = new string[initialCapacity];
            _statuses = new bool[initialCapacity];
            _dates = new DateTime[initialCapacity];
            _todoCount = 0;
            _arrayGrowthFactor = growthFactor;
        }

        public void AddTodo(string task)
        {
            if (_todoCount >= _todos.Length)
            {
                ResizeArrays();
            }

            _todos[_todoCount] = task;
            _statuses[_todoCount] = false; // По умолчанию задача не выполнена
            _dates[_todoCount] = DateTime.Now;
            _todoCount++;
        }

        public bool CompleteTodo(int taskIndex)
        {
            if (taskIndex < 0 || taskIndex >= _todoCount)
            {
                return false;
            }

            _statuses[taskIndex] = true;
            _dates[taskIndex] = DateTime.Now; // Обновляем дату при изменении статуса
            return true;
        }

        public bool DeleteTodo(int taskIndex)
        {
            if (taskIndex < 0 || taskIndex >= _todoCount)
            {
                return false;
            }

            // Сдвигаем все элементы после удаляемого
            for (int i = taskIndex; i < _todoCount - 1; i++)
            {
                _todos[i] = _todos[i + 1];
                _statuses[i] = _statuses[i + 1];
                _dates[i] = _dates[i + 1];
            }

            _todoCount--;
            return true;
        }

        public string[] GetTodos()
        {
            return _todos;
        }

        public bool[] GetStatuses()
        {
            return _statuses;
        }

        public DateTime[] GetDates()
        {
            return _dates;
        }

        public int GetTodoCount()
        {
            return _todoCount;
        }

        private void ResizeArrays()
        {
            int newSize = _todos.Length * _arrayGrowthFactor;
            
            string[] newTodos = new string[newSize];
            bool[] newStatuses = new bool[newSize];
            DateTime[] newDates = new DateTime[newSize];

            for (int i = 0; i < _todos.Length; i++)
            {
                newTodos[i] = _todos[i];
                newStatuses[i] = _statuses[i];
                newDates[i] = _dates[i];
            }

            _todos = newTodos;
            _statuses = newStatuses;
            _dates = newDates;
            
            Console.WriteLine($"Массивы расширены до {_todos.Length} элементов");
        }
    }

    // Класс для хранения профиля пользователя
    public class UserProfile
    {
        public string FirstName { get; }
        public string LastName { get; }
        public int BirthYear { get; }
        public int Age { get; }

        public UserProfile(string firstName, string lastName, int birthYear, int age)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
            Age = age;
        }

        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }
    }

    // Класс для обработки команд
    public static class CommandHandlers
    {
        public static void ShowHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help     - вывести список команд");
            Console.WriteLine("profile  - показать данные пользователя");
            Console.WriteLine("add      - добавить задачу");
            Console.WriteLine("view     - показать все задачи");
            Console.WriteLine("complete - отметить задачу как выполненную");
            Console.WriteLine("delete   - удалить задачу");
            Console.WriteLine("exit     - выход из программы");
            Console.WriteLine("\nПримеры:");
            Console.WriteLine("add Сходить в магазин");
            Console.WriteLine("complete 1");
            Console.WriteLine("delete 2");
        }

        public static void ShowProfile(UserProfile user)
        {
            Console.WriteLine($"{user.FirstName} {user.LastName}, {user.BirthYear}");
        }

        public static void AddTodo(string[] commandParts, TodoManager todoManager)
        {
            if (commandParts.Length < 2)
            {
                Console.WriteLine("Ошибка: не указана задача");
                return;
            }

            string task = string.Join(" ", commandParts, 1, commandParts.Length - 1);
            todoManager.AddTodo(task);
            Console.WriteLine("Задача добавлена!");
        }

        public static void ViewTodos(TodoManager todoManager)
        {
            int todoCount = todoManager.GetTodoCount();
            string[] todos = todoManager.GetTodos();
            bool[] statuses = todoManager.GetStatuses();
            DateTime[] dates = todoManager.GetDates();

            if (todoCount == 0)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }
            
            Console.WriteLine("Список задач:");
            for (int i = 0; i < todoCount; i++)
            {
                string status = statuses[i] ? "✓ ВЫПОЛНЕНО" : "✗ НЕ ВЫПОЛНЕНО";
                string date = dates[i].ToString("dd.MM.yyyy HH:mm");
                Console.WriteLine($"{i + 1}. {todos[i]}");
                Console.WriteLine($"   Статус: {status}");
                Console.WriteLine($"   Дата: {date}");
                Console.WriteLine();
            }
        }

        public static void CompleteTodo(string[] commandParts, TodoManager todoManager)
        {
            if (commandParts.Length < 2 || !int.TryParse(commandParts[1], out int taskNumber))
            {
                Console.WriteLine("Ошибка: укажите номер задачи для выполнения");
                return;
            }

            int taskIndex = taskNumber - 1; // Переводим в 0-based индекс
            bool success = todoManager.CompleteTodo(taskIndex);
            
            if (success)
            {
                Console.WriteLine($"Задача {taskNumber} отмечена как выполненная!");
            }
            else
            {
                Console.WriteLine($"Ошибка: задача с номером {taskNumber} не найдена");
            }
        }

        public static void DeleteTodo(string[] commandParts, TodoManager todoManager)
        {
            if (commandParts.Length < 2 || !int.TryParse(commandParts[1], out int taskNumber))
            {
                Console.WriteLine("Ошибка: укажите номер задачи для удаления");
                return;
            }

            int taskIndex = taskNumber - 1; // Переводим в 0-based индекс
            bool success = todoManager.DeleteTodo(taskIndex);
            
            if (success)
            {
                Console.WriteLine($"Задача {taskNumber} удалена!");
            }
            else
            {
                Console.WriteLine($"Ошибка: задача с номером {taskNumber} не найдена");
            }
        }

        public static void ExitProgram()
        {
            Console.WriteLine("Выход из программы...");
        }
    }

    // Вспомогательный класс для возврата результата выполнения команды
    public class CommandResult
    {
        public bool ShouldExit { get; }

        public CommandResult(bool shouldExit)
        {
            ShouldExit = shouldExit;
        }
    }
}