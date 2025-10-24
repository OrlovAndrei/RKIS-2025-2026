using System;

namespace Todolist
{
    class Program
    {
        // Константы для "магических" значений
        private const int InitialTodoCapacity = 2;
        private const int ArrayGrowthFactor = 2;
        private const int CurrentYear = 2024;

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

    // Класс для представления одной задачи
    public class TodoItem
    {
        public string Task { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public TodoItem(string task)
        {
            Task = task;
            IsCompleted = false;
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }

        public void MarkAsCompleted()
        {
            IsCompleted = true;
            ModifiedDate = DateTime.Now;
        }

        public void UpdateTask(string newTask)
        {
            Task = newTask;
            ModifiedDate = DateTime.Now;
        }
    }

    // Класс для управления задачами
    public class TodoManager
    {
        private TodoItem[] _todoItems;
        private int _itemCount;
        private readonly int _arrayGrowthFactor;

        public TodoManager(int initialCapacity, int growthFactor)
        {
            _todoItems = new TodoItem[initialCapacity];
            _itemCount = 0;
            _arrayGrowthFactor = growthFactor;
        }

        public void AddTodo(string task)
        {
            if (_itemCount >= _todoItems.Length)
            {
                ResizeArray();
            }

            _todoItems[_itemCount] = new TodoItem(task);
            _itemCount++;
        }

        public bool CompleteTodo(int taskIndex)
        {
            if (taskIndex < 0 || taskIndex >= _itemCount)
            {
                return false;
            }

            _todoItems[taskIndex].MarkAsCompleted();
            return true;
        }

        public bool DeleteTodo(int taskIndex)
        {
            if (taskIndex < 0 || taskIndex >= _itemCount)
            {
                return false;
            }

            // Сдвигаем все элементы после удаляемого
            for (int i = taskIndex; i < _itemCount - 1; i++)
            {
                _todoItems[i] = _todoItems[i + 1];
            }

            // Очищаем последний элемент
            _todoItems[_itemCount - 1] = null;
            _itemCount--;
            
            return true;
        }

        public TodoItem[] GetTodoItems()
        {
            TodoItem[] result = new TodoItem[_itemCount];
            for (int i = 0; i < _itemCount; i++)
            {
                result[i] = _todoItems[i];
            }
            return result;
        }

        public int GetTodoCount()
        {
            return _itemCount;
        }

        public TodoItem GetTodoItem(int index)
        {
            if (index >= 0 && index < _itemCount)
            {
                return _todoItems[index];
            }
            return null;
        }

        private void ResizeArray()
        {
            int newSize = _todoItems.Length * _arrayGrowthFactor;
            TodoItem[] newTodoItems = new TodoItem[newSize];

            for (int i = 0; i < _todoItems.Length; i++)
            {
                newTodoItems[i] = _todoItems[i];
            }

            _todoItems = newTodoItems;
            Console.WriteLine($"Массив задач расширен до {_todoItems.Length} элементов");
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
            Console.WriteLine($"Пользователь: {user.FirstName} {user.LastName}");
            Console.WriteLine($"Год рождения: {user.BirthYear}");
            Console.WriteLine($"Возраст: {user.Age} лет");
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

            if (todoCount == 0)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }
            
            Console.WriteLine("Список задач:");
            Console.WriteLine(new string('-', 60));
            
            for (int i = 0; i < todoCount; i++)
            {
                TodoItem item = todoManager.GetTodoItem(i);
                string status = item.IsCompleted ? "✓ ВЫПОЛНЕНО" : "✗ НЕ ВЫПОЛНЕНО";
                string createdDate = item.CreatedDate.ToString("dd.MM.yyyy HH:mm");
                string modifiedDate = item.ModifiedDate.ToString("dd.MM.yyyy HH:mm");
                
                Console.WriteLine($"{i + 1}. {item.Task}");
                Console.WriteLine($"   Статус: {status}");
                Console.WriteLine($"   Создана: {createdDate}");
                
                if (item.ModifiedDate != item.CreatedDate)
                {
                    Console.WriteLine($"   Изменена: {modifiedDate}");
                }
                
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

            int taskIndex = taskNumber - 1;
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

            int taskIndex = taskNumber - 1;
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