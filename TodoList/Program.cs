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
        private int _todoCount;
        private readonly int _arrayGrowthFactor;

        public TodoManager(int initialCapacity, int growthFactor)
        {
            _todos = new string[initialCapacity];
            _todoCount = 0;
            _arrayGrowthFactor = growthFactor;
        }

        public void AddTodo(string task)
        {
            if (_todoCount >= _todos.Length)
            {
                ResizeTodoArray();
            }

            _todos[_todoCount] = task;
            _todoCount++;
        }

        public string[] GetTodos()
        {
            return _todos;
        }

        public int GetTodoCount()
        {
            return _todoCount;
        }

        private void ResizeTodoArray()
        {
            int newSize = _todos.Length * _arrayGrowthFactor;
            string[] newTodos = new string[newSize];

            for (int i = 0; i < _todos.Length; i++)
            {
                newTodos[i] = _todos[i];
            }

            _todos = newTodos;
            Console.WriteLine($"Массив расширен до {_todos.Length} элементов");
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
            Console.WriteLine("help    - вывести список команд");
            Console.WriteLine("profile - показать данные пользователя");
            Console.WriteLine("add     - добавить задачу");
            Console.WriteLine("view    - показать все задачи");
            Console.WriteLine("exit    - выход из программы");
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

            if (todoCount == 0)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }
            
            Console.WriteLine("Список задач:");
            for (int i = 0; i < todoCount; i++)
            {
                Console.WriteLine($"{i + 1}. {todos[i]}");
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