using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoListApp
{
    // Класс для представления профиля пользователя
    public class Profile
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public int BirthYear { get; private set; }
        public int Age => DateTime.Now.Year - BirthYear;

        public Profile(string firstName, string lastName, int birthYear)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthYear = birthYear;
        }

        public string GetInfo()
        {
            return $"{FirstName} {LastName}, возраст {Age}";
        }

        public static Profile CreateFromInput()
        {
            Console.Write("Введите ваше имя и фамилию: ");
            string userName = Console.ReadLine()?.Trim() ?? "Неизвестно";
            
            if (string.IsNullOrEmpty(userName)) 
            {
                userName = "Неизвестно Неизвестно";
            }

            // Разделяем ввод на имя и фамилию
            string[] nameParts = userName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string firstName, lastName;

            if (nameParts.Length >= 2)
            {
                // Если введены и имя и фамилия
                firstName = nameParts[0];
                lastName = nameParts[1];
                
                // Если есть больше двух слов, объединяем остальные в фамилию
                for (int i = 2; i < nameParts.Length; i++)
                {
                    lastName += " " + nameParts[i];
                }
            }
            else if (nameParts.Length == 1)
            {
                // Если введено только одно слово
                firstName = nameParts[0];
                lastName = "Неизвестно";
            }
            else
            {
                // Если ничего не введено
                firstName = "Неизвестно";
                lastName = "Неизвестно";
            }

            Console.Write($"{userName}, введите год вашего рождения: ");
            string yearBirthInput = Console.ReadLine()?.Trim() ?? "";

            if (int.TryParse(yearBirthInput, out int birthYear) && birthYear < DateTime.Now.Year)
            {
                Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возрастом {DateTime.Now.Year - birthYear}");
                return new Profile(firstName, lastName, birthYear);
            }
            else
            {
                Console.WriteLine("Пользователь не ввел корректный возраст, установлен год по умолчанию: 2000");
                return new Profile(firstName, lastName, 2000);
            }
        }

        public void DisplayProfile()
        {
            Console.WriteLine($"\n{GetInfo()}");
        }
    }

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
            string status = IsDone ? " Выполнено" : " Не выполнено";
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

    // Класс для управления списком задач
    public class TodoList
    {
        // Приватное поле: массив задач
        private TodoItem[] tasks;
        private int count;

        public int Count => count;
        public bool HasTasks => count > 0;

        // Конструктор
        public TodoList(int initialCapacity = 10)
        {
            tasks = new TodoItem[initialCapacity];
            count = 0;
        }

        // Добавить задачу
        public void Add(TodoItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Задача не может быть null");

            if (count >= tasks.Length)
            {
                IncreaseArray(tasks, item);
            }
            else
            {
                tasks[count] = item;
                count++;
            }
        }

        // Удалить задачу по индексу
        public void Delete(int index)
        {
            if (index < 0 || index >= count)
                throw new ArgumentOutOfRangeException(nameof(index), "Индекс находится вне диапазона");

            // Сдвигаем элементы массива
            for (int i = index; i < count - 1; i++)
            {
                tasks[i] = tasks[i + 1];
            }

            tasks[count - 1] = null; // Очищаем последний элемент
            count--;
        }

        // Вывод задач в виде таблицы
        public void View(bool showIndex = false, bool showDone = true, bool showDate = true)
        {
            if (!HasTasks)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }

            Console.WriteLine("\nСписок задач:");

            for (int i = 0; i < count; i++)
            {
                var task = tasks[i];
                string number = showIndex ? $"{i + 1}. " : "";

                // Используем метод GetShortInfo из TodoItem
                string taskInfo = task.GetShortInfo();
                Console.WriteLine($"{number}{taskInfo}");
            }
        }

        // Получить задачу по индексу
        public TodoItem GetItem(int index)
        {
            if (index < 0 || index >= count)
                throw new ArgumentOutOfRangeException(nameof(index), "Индекс находится вне диапазона");

            return tasks[index];
        }

        // Метод, который увеличивает размер массива при переполнении
        private void IncreaseArray(TodoItem[] items, TodoItem item)
        {
            // Увеличиваем размер массива в 2 раза
            int newSize = items.Length * 2;
            TodoItem[] newArray = new TodoItem[newSize];

            // Копируем существующие элементы
            for (int i = 0; i < items.Length; i++)
            {
                newArray[i] = items[i];
            }

            // Добавляем новый элемент
            newArray[count] = item;
            count++;

            // Заменяем старый массив новым
            tasks = newArray;
        }

        // Дополнительные методы для совместимости со старым кодом
        public void MarkAsDone(int index)
        {
            if (index < 0 || index >= count)
                throw new ArgumentOutOfRangeException(nameof(index), "Индекс находится вне диапазона");

            tasks[index].MarkDone();
        }

        public void UpdateText(int index, string newText)
        {
            if (index < 0 || index >= count)
                throw new ArgumentOutOfRangeException(nameof(index), "Индекс находится вне диапазона");

            tasks[index].UpdateText(newText);
        }

        public bool IsValidIndex(int index)
        {
            return index >= 0 && index < count;
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
        private Profile user;
        private TodoList todoList;
        private CommandParser commandParser;
        private bool isRunning;

        public TodoApplication()
        {
            Console.WriteLine("выполнил работу Турищев Иван");
            user = Profile.CreateFromInput();
            todoList = new TodoList();
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
                AddMultilineTask();
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

                AddTask(taskText);
            }
        }

        private void AddTask(string taskText)
        {
            if (!string.IsNullOrWhiteSpace(taskText))
            {
                todoList.Add(new TodoItem(taskText));
                Console.WriteLine("Задача успешно добавлена!");
            }
            else
            {
                Console.WriteLine("Ошибка: текст задачи не может быть пустым");
            }
        }

        private void AddMultilineTask()
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

        private void HandleViewCommand(List<string> flags)
        {
            bool showNumbers = flags.Contains("-i");
            bool showStatus = !flags.Contains("-s");
            bool showDates = !flags.Contains("-d");

            todoList.View(showIndex: showNumbers, showDone: showStatus, showDate: showDates);
        }

        private void HandleCompleteCommand()
        {
            if (!todoList.HasTasks)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }

            todoList.View(showIndex: true);
            Console.Write("Введите номер задачи для отметки как выполненной: ");

            if (int.TryParse(Console.ReadLine(), out int taskNumber) && todoList.IsValidIndex(taskNumber - 1))
            {
                todoList.MarkAsDone(taskNumber - 1);
                Console.WriteLine($"Задача отмечена как выполненная!");
            }
            else
            {
                Console.WriteLine("Ошибка: неверный номер задачи");
            }
        }

        private void HandleRemoveCommand()
        {
            if (!todoList.HasTasks)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }

            todoList.View(showIndex: true);
            Console.Write("Введите номер задачи для удаления: ");

            if (int.TryParse(Console.ReadLine(), out int taskNumber) && todoList.IsValidIndex(taskNumber - 1))
            {
                var task = todoList.GetItem(taskNumber - 1);
                string taskText = GetShortText(task.Text);

                Console.Write($"Вы уверены, что хотите удалить задачу '{taskText}'? (y/n): ");
                string confirmation = Console.ReadLine()?.ToLower() ?? "";
                if (confirmation == "y" || confirmation == "yes")
                {
                    todoList.Delete(taskNumber - 1);
                    Console.WriteLine($"Задача '{taskText}' успешно удалена!");
                }
                else
                {
                    Console.WriteLine("Удаление отменено");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: неверный номер задачи");
            }
        }

        private void HandleEditCommand()
        {
            if (!todoList.HasTasks)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }

            todoList.View(showIndex: true);
            Console.Write("Введите номер задачи для редактирования: ");

            if (int.TryParse(Console.ReadLine(), out int taskNumber) && todoList.IsValidIndex(taskNumber - 1))
            {
                Console.Write("Введите новый текст задачи: ");
                string newTask = Console.ReadLine()?.Trim() ?? "";

                if (!string.IsNullOrWhiteSpace(newTask))
                {
                    todoList.UpdateText(taskNumber - 1, newTask);
                    Console.WriteLine("Задача успешно обновлена!");
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
            if (int.TryParse(argument, out int taskNumber) && todoList.IsValidIndex(taskNumber - 1))
            {
                todoList.MarkAsDone(taskNumber - 1);
                var task = todoList.GetItem(taskNumber - 1);
                Console.WriteLine($"Задача '{GetShortText(task.Text)}' отмечена как выполненная!");
            }
            else
            {
                Console.WriteLine("Ошибка: неверный номер задачи. Используйте: done <индекс>");
            }
        }

        private void HandleDeleteCommand(List<string> flags, string argument)
        {
            bool force = flags.Contains("-f");
            
            if (int.TryParse(argument, out int taskNumber) && todoList.IsValidIndex(taskNumber - 1))
            {
                var task = todoList.GetItem(taskNumber - 1);
                string taskText = GetShortText(task.Text);

                if (force)
                {
                    todoList.Delete(taskNumber - 1);
                    Console.WriteLine($"Задача '{taskText}' успешно удалена!");
                }
                else
                {
                    Console.Write($"Вы уверены, что хотите удалить задачу '{taskText}'? (y/n): ");
                    string confirmation = Console.ReadLine()?.ToLower() ?? "";
                    if (confirmation == "y" || confirmation == "yes")
                    {
                        todoList.Delete(taskNumber - 1);
                        Console.WriteLine($"Задача '{taskText}' успешно удалена!");
                    }
                    else
                    {
                        Console.WriteLine("Удаление отменено");
                    }
                }
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

            if (int.TryParse(parts[0], out int taskNumber) && todoList.IsValidIndex(taskNumber - 1))
            {
                string newText = parts[1].Trim();
                if (newText.StartsWith("\"") && newText.EndsWith("\""))
                {
                    newText = newText.Substring(1, newText.Length - 2);
                }
                
                if (!string.IsNullOrWhiteSpace(newText))
                {
                    todoList.UpdateText(taskNumber - 1, newText);
                    Console.WriteLine("Задача успешно обновлена!");
                }
                else
                {
                    Console.WriteLine("Ошибка: текст задачи не может быть пустым");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: неверный номер задачи. Используйте: update <индекс> \"новый текст\"");
            }
        }

        private void HandleReadCommand(string argument)
        {
            if (int.TryParse(argument, out int taskNumber) && todoList.IsValidIndex(taskNumber - 1))
            {
                var task = todoList.GetItem(taskNumber - 1);

                Console.WriteLine("\n" + new string('=', 60));
                Console.WriteLine($"ЗАДАЧА #{taskNumber}");
                Console.WriteLine(new string('=', 60));
                Console.WriteLine(new string('-', 60));
                Console.WriteLine("ПОЛНАЯ ИНФОРМАЦИЯ О ЗАДАЧЕ:");
                Console.WriteLine(new string('-', 60));
                Console.WriteLine(task.GetFullInfo());
                Console.WriteLine(new string('=', 60));
            }
            else
            {
                Console.WriteLine("Ошибка: укажите номер задачи. Формат: read <индекс>");
            }
        }

        private string GetShortText(string text)
        {
            string shortText = text.Replace("\n", " ");
            return shortText.Length > 30 ? shortText.Substring(0, 30) + "..." : shortText;
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
