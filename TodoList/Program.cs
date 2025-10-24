using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoListApp
{
    // Класс для представления задачи
    public class TodoItem
    {
        public string Text { get; private set; }
        public bool IsDone { get; private set; }
        public DateTime LastUpdate { get; private set; }

        public TodoItem(string text)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            IsDone = false;
            LastUpdate = DateTime.Now;
        }

        public void MarkDone()
        {
            IsDone = true;
            LastUpdate = DateTime.Now;
        }

        public void UpdateText(string newText)
        {
            if (string.IsNullOrWhiteSpace(newText))
                throw new ArgumentException("Текст задачи не может быть пустым", nameof(newText));

            Text = newText;
            LastUpdate = DateTime.Now;
        }

        public string GetShortInfo()
        {
            string shortText = Text.Length > 30 ? Text.Substring(0, 30) + "..." : Text;
            string status = IsDone ? "✓ Выполнено" : "✗ Не выполнено";
            string date = LastUpdate.ToString("dd.MM.yyyy HH:mm");
            
            return $"{shortText,-33} | {status,-12} | {date}";
        }

        public string GetFullInfo()
        {
            string status = IsDone ? "Выполнена" : "Не выполнена";
            
            return $"Текст задачи: {Text}\n" +
                   $"Статус: {status}\n" +
                   $"Последнее изменение: {LastUpdate:dd.MM.yyyy HH:mm}";
        }

        public override string ToString()
        {
            return GetShortInfo();
        }
    }

    // Класс для представления пользователя
    public class User
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public int BirthYear { get; private set; }
        public int Age => DateTime.Now.Year - BirthYear;

        public User(string firstName, string lastName, int birthYear)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
        }

        public static User CreateFromInput()
        {
            Console.Write("Введите ваше имя: ");
            string userName = Console.ReadLine()?.Trim() ?? "Неизвестно";
            if (string.IsNullOrEmpty(userName)) userName = "Неизвестно";

            string[] nameParts = userName.Split(' ');
            string firstName = nameParts[0];
            string lastName = nameParts.Length > 1 ? nameParts[1] : "Неизвестно";

            Console.Write($"{userName}, введите год вашего рождения: ");
            string yearBirthInput = Console.ReadLine()?.Trim() ?? "";

            if (int.TryParse(yearBirthInput, out int birthYear) && birthYear < DateTime.Now.Year)
            {
                Console.WriteLine($"Добавлен пользователь {userName}, возрастом {DateTime.Now.Year - birthYear}");
                return new User(firstName, lastName, birthYear);
            }
            else
            {
                Console.WriteLine("Пользователь не ввел корректный возраст, установлен год по умолчанию: 2000");
                return new User(firstName, lastName, 2000);
            }
        }

        public void DisplayProfile()
        {
            Console.WriteLine($"\n{FirstName} {LastName}, {BirthYear} (Возраст: {Age})");
        }
    }

    // Класс для управления списком задач (обновлен для работы с TodoItem)
    public class TodoManager
    {
        private List<TodoItem> tasks = new List<TodoItem>();

        public int TaskCount => tasks.Count;
        public bool HasTasks => tasks.Any();

        public void AddTask(string taskText)
        {
            if (!string.IsNullOrWhiteSpace(taskText))
            {
                tasks.Add(new TodoItem(taskText));
                Console.WriteLine("Задача успешно добавлена!");
            }
            else
            {
                Console.WriteLine("Ошибка: текст задачи не может быть пустым");
            }
        }

        public void AddMultilineTask()
        {
            Console.WriteLine("Введите текст задачи (для завершения введите !end):");
            List<string> lines = new List<string>();
            string line;

            while (true)
            {
                line = Console.ReadLine()?.Trim() ?? "";
                if (line == "!end")
                    break;
                lines.Add(line);
            }

            string taskText = string.Join("\n", lines);
            AddTask(taskText);
        }

        public void ViewTasks(bool showNumbers = false, bool showStatus = true, bool showDates = true)
        {
            if (!HasTasks)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }

            Console.WriteLine("\nСписок задач:");

            for (int i = 0; i < tasks.Count; i++)
            {
                var task = tasks[i];
                string number = showNumbers ? $"{i + 1}. " : "";

                // Используем метод GetShortInfo из TodoItem
                string taskInfo = task.GetShortInfo();
                Console.WriteLine($"{number}{taskInfo}");
            }
        }

        public void DisplayTaskDetails(int taskNumber)
        {
            if (!IsValidTaskNumber(taskNumber))
            {
                Console.WriteLine("Ошибка: неверный номер задачи");
                return;
            }

            var task = tasks[taskNumber - 1];

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine($"ЗАДАЧА #{taskNumber}");
            Console.WriteLine(new string('=', 60));
            Console.WriteLine(new string('-', 60));
            Console.WriteLine("ПОЛНАЯ ИНФОРМАЦИЯ О ЗАДАЧЕ:");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine(task.GetFullInfo());
            Console.WriteLine(new string('=', 60));
        }

        public void MarkTaskAsCompleted(int taskNumber)
        {
            if (!IsValidTaskNumber(taskNumber))
            {
                Console.WriteLine("Ошибка: неверный номер задачи");
                return;
            }

            var task = tasks[taskNumber - 1];
            task.MarkDone();
            Console.WriteLine($"Задача '{GetShortText(task.Text)}' отмечена как выполненная!");
        }

        public void DeleteTask(int taskNumber, bool force = false)
        {
            if (!IsValidTaskNumber(taskNumber))
            {
                Console.WriteLine("Ошибка: неверный номер задачи");
                return;
            }

            var task = tasks[taskNumber - 1];
            string taskText = GetShortText(task.Text);

            if (force)
            {
                tasks.RemoveAt(taskNumber - 1);
                Console.WriteLine($"Задача '{taskText}' успешно удалена!");
            }
            else
            {
                Console.Write($"Вы уверены, что хотите удалить задачу '{taskText}'? (y/n): ");
                string confirmation = Console.ReadLine()?.ToLower() ?? "";
                if (confirmation == "y" || confirmation == "yes")
                {
                    tasks.RemoveAt(taskNumber - 1);
                    Console.WriteLine($"Задача '{taskText}' успешно удалена!");
                }
                else
                {
                    Console.WriteLine("Удаление отменено");
                }
            }
        }

        public void UpdateTask(int taskNumber, string newText)
        {
            if (!IsValidTaskNumber(taskNumber))
            {
                Console.WriteLine("Ошибка: неверный номер задачи");
                return;
            }

            if (string.IsNullOrWhiteSpace(newText))
            {
                Console.WriteLine("Ошибка: текст задачи не может быть пустым");
                return;
            }

            var task = tasks[taskNumber - 1];
            task.UpdateText(newText);
            Console.WriteLine("Задача успешно обновлена!");
        }

        private bool IsValidTaskNumber(int taskNumber)
        {
            return taskNumber >= 1 && taskNumber <= tasks.Count;
        }

        private string GetShortText(string text)
        {
            string shortText = text.Replace("\n", " ");
            return shortText.Length > 30 ? shortText.Substring(0, 30) + "..." : shortText;
        }
    }

    // Класс для разбора команд
    public class CommandParser
    {
        public ParsedCommand Parse(string input)
        {
            var parts = input.Split(' ');
            var parsed = new ParsedCommand
            {
                Command = parts[0].ToLower(),
                Flags = new List<string>(),
                Argument = ""
            };

            List<string> remainingParts = new List<string>();

            for (int i = 1; i < parts.Length; i++)
            {
                if (parts[i].StartsWith("--"))
                {
                    parsed.Flags.Add(parts[i].ToLower());
                }
                else if (parts[i].StartsWith("-") && parts[i].Length > 1)
                {
                    string shortFlags = parts[i].Substring(1);
                    foreach (char flagChar in shortFlags)
                    {
                        parsed.Flags.Add($"-{flagChar}");
                    }
                }
                else
                {
                    remainingParts.Add(parts[i]);
                }
            }

            parsed.Argument = string.Join(" ", remainingParts);
            return parsed;
        }
    }

    // Структура для хранения разобранной команды
    public struct ParsedCommand
    {
        public string Command { get; set; }
        public List<string> Flags { get; set; }
        public string Argument { get; set; }
    }

    // Главный класс приложения
    public class TodoApplication
    {
        private User user;
        private TodoManager todoManager;
        private CommandParser commandParser;
        private bool isRunning;

        public TodoApplication()
        {
            Console.WriteLine("выполнил работу Турищев Иван");
            user = User.CreateFromInput();
            todoManager = new TodoManager();
            commandParser = new CommandParser();
            isRunning = true;
        }

        public void Run()
        {
            Console.WriteLine("\nДобро пожаловать в TodoList! Введите 'help' для списка команд.");

            while (isRunning)
            {
                Console.Write("\n> ");
                string input = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                ProcessCommand(input);
            }
        }

        private void ProcessCommand(string input)
        {
            var parsedCommand = commandParser.Parse(input);
            string command = parsedCommand.Command;
            List<string> flags = parsedCommand.Flags;
            string argument = parsedCommand.Argument;

            switch (command)
            {
                case "help":
                    ShowHelp();
                    break;

                case "profile":
                    user.DisplayProfile();
                    break;

                case "add":
                    HandleAddCommand(flags, argument);
                    break;

                case "view":
                    HandleViewCommand(flags);
                    break;

                case "complete":
                    HandleCompleteCommand();
                    break;

                case "remove":
                    HandleRemoveCommand();
                    break;

                case "edit":
                    HandleEditCommand();
                    break;

                case "done":
                    HandleDoneCommand(argument);
                    break;

                case "delete":
                    HandleDeleteCommand(flags, argument);
                    break;

                case "update":
                    HandleUpdateCommand(argument);
                    break;

                case "read":
                    HandleReadCommand(argument);
                    break;

                case "exit":
                    isRunning = false;
                    Console.WriteLine("Программа завершена. До свидания!");
                    break;

                default:
                    Console.WriteLine("Неизвестная команда. Введите 'help' для списка доступных команд.");
                    break;
            }
        }

        private void HandleAddCommand(List<string> flags, string argument)
        {
            if (flags.Contains("--multiline") || flags.Contains("-m"))
            {
                todoManager.AddMultilineTask();
            }
            else
            {
                if (string.IsNullOrWhiteSpace(argument))
                {
                    Console.Write("Введите текст задачи: ");
                    argument = Console.ReadLine()?.Trim() ?? "";
                }

                string taskText = argument.StartsWith("\"") && argument.EndsWith("\"") 
                    ? argument.Substring(1, argument.Length - 2) 
                    : argument;

                todoManager.AddTask(taskText);
            }
        }

        private void HandleViewCommand(List<string> flags)
        {
            bool showNumbers = flags.Contains("-i");
            bool showStatus = !flags.Contains("-s");
            bool showDates = !flags.Contains("-d");

            foreach (var flag in flags.Where(f => f.Length == 2 && f[0] == '-'))
            {
                if (flag.Contains('i')) showNumbers = true;
                if (flag.Contains('s')) showStatus = false;
                if (flag.Contains('d')) showDates = false;
            }

            todoManager.ViewTasks(showNumbers, showStatus, showDates);
        }

        private void HandleCompleteCommand()
        {
            if (!todoManager.HasTasks)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }

            todoManager.ViewTasks();
            Console.Write("Введите номер задачи для отметки как выполненной: ");

            if (int.TryParse(Console.ReadLine(), out int taskNumber))
            {
                todoManager.MarkTaskAsCompleted(taskNumber);
            }
            else
            {
                Console.WriteLine("Ошибка: неверный номер задачи");
            }
        }

        private void HandleRemoveCommand()
        {
            if (!todoManager.HasTasks)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }

            todoManager.ViewTasks();
            Console.Write("Введите номер задачи для удаления: ");

            if (int.TryParse(Console.ReadLine(), out int taskNumber))
            {
                todoManager.DeleteTask(taskNumber);
            }
            else
            {
                Console.WriteLine("Ошибка: неверный номер задачи");
            }
        }

        private void HandleEditCommand()
        {
            if (!todoManager.HasTasks)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }

            todoManager.ViewTasks();
            Console.Write("Введите номер задачи для редактирования: ");

            if (int.TryParse(Console.ReadLine(), out int taskNumber))
            {
                Console.Write("Введите новый текст задачи: ");
                string newTask = Console.ReadLine()?.Trim() ?? "";

                if (!string.IsNullOrWhiteSpace(newTask))
                {
                    todoManager.UpdateTask(taskNumber, newTask);
                }
                else
                {
                    Console.WriteLine("Ошибка: текст задачи не может быть пустым");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: неверный номер задачи");
            }
        }

        private void HandleDoneCommand(string argument)
        {
            if (int.TryParse(argument, out int taskNumber))
            {
                todoManager.MarkTaskAsCompleted(taskNumber);
            }
            else
            {
                Console.WriteLine("Ошибка: неверный номер задачи. Используйте: done <индекс>");
            }
        }

        private void HandleDeleteCommand(List<string> flags, string argument)
        {
            bool force = flags.Contains("-f");
            
            if (int.TryParse(argument, out int taskNumber))
            {
                todoManager.DeleteTask(taskNumber, force);
            }
            else
            {
                Console.WriteLine("Ошибка: неверный номер задачи. Используйте: delete <индекс>");
            }
        }

        private void HandleUpdateCommand(string argument)
        {
            string[] parts = argument.Split(' ', 2);
            if (parts.Length < 2)
            {
                Console.WriteLine("Ошибка: неправильный формат. Используйте: update <индекс> \"новый текст\"");
                return;
            }

            if (int.TryParse(parts[0], out int taskNumber))
            {
                string newText = parts[1].Trim();
                if (newText.StartsWith("\"") && newText.EndsWith("\""))
                {
                    newText = newText.Substring(1, newText.Length - 2);
                }
                todoManager.UpdateTask(taskNumber, newText);
            }
            else
            {
                Console.WriteLine("Ошибка: неверный номер задачи. Используйте: update <индекс> \"новый текст\"");
            }
        }

        private void HandleReadCommand(string argument)
        {
            if (int.TryParse(argument, out int taskNumber))
            {
                todoManager.DisplayTaskDetails(taskNumber);
            }
            else
            {
                Console.WriteLine("Ошибка: укажите номер задачи. Формат: read <индекс>");
            }
        }

        private void ShowHelp()
        {
            Console.WriteLine("\nДоступные команды:");
            Console.WriteLine("help     - выводит список всех доступных команд с кратким описанием");
            Console.WriteLine("profile  - выводит данные пользователя");
            Console.WriteLine("add      - добавляет новую задачу. Формат: add \"текст задачи\"");
            Console.WriteLine("view     - выводит все задачи из списка. Флаги: -i (пронумерованный), -s (только статус), -d (только даты)");
            Console.WriteLine("complete - отмечает задачу как выполненную");
            Console.WriteLine("remove   - удаляет задачу");
            Console.WriteLine("edit     - редактирует текст задачи");
            Console.WriteLine("done     - отмечает задачу выполненной. Формат: done <индекс>");
            Console.WriteLine("delete   - удаляет задачу по индексу. Формат: delete <индекс>");
            Console.WriteLine("update   - обновляет текст задачи. Формат: update <индекс> \"новый текст\"");
            Console.WriteLine("read     - просмотр полного текста задачи. Формат: read <индекс>");
            Console.WriteLine("exit     - завершает программу");

            Console.WriteLine("\nФлаги команд:");
            Console.WriteLine("--multiline, -m - многострочный ввод для команды add (завершить ввод: !end)");
            Console.WriteLine("-i  - пронумерованный вывод (для view)");
            Console.WriteLine("-s  - показывать только статус (для view)");
            Console.WriteLine("-d  - показывать только даты (для view)");
            Console.WriteLine("-f  - принудительное выполнение (для delete)");
            Console.WriteLine("-l  - показывать длинный формат (для read)");
            Console.WriteLine("Комбинации: -is, -id, -sd и т.д.");
        }
    }

    // Точка входа в программу
    class Program
    {
        static void Main()
        {
            TodoApplication app = new TodoApplication();
            app.Run();
        }
    }
}
