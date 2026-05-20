using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace TodoList
{
    internal static class AppInfo
    {
        public static Dictionary<Guid, TodoList> TodosByProfile { get; set; } = new Dictionary<Guid, TodoList>();
        public static List<Profile> Profiles { get; set; } = new List<Profile>();
        public static Guid? CurrentProfileId { get; set; }
        public static Stack<ICommand> UndoStack { get; } = new Stack<ICommand>();
        public static Stack<ICommand> RedoStack { get; } = new Stack<ICommand>();
        public static string DataDirectory { get; set; } = string.Empty;
        
        public static TodoList Todos
        {
            get
            {
                if (CurrentProfileId == null)
                    return null!;
                if (!TodosByProfile.ContainsKey(CurrentProfileId.Value))
                    TodosByProfile[CurrentProfileId.Value] = new TodoList();
                return TodosByProfile[CurrentProfileId.Value];
            }
        }
    }

    internal class Program
    {
        private const string Prompt = "> ";
        private const string DataDirectory = "data";

        private static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Буряк Степан Геннадьевич и Голубев Данил Сергеевич");

            // Определяем пути к файлам
            string dataDirPath = Path.Combine(Directory.GetCurrentDirectory(), DataDirectory);
            string profilePath = Path.Combine(dataDirPath, "profile.csv");

            // Создаем папку для данных, если её нет
            FileManager.EnsureDataDirectory(dataDirPath);
            AppInfo.DataDirectory = dataDirPath;

            // Загружаем все профили
            AppInfo.Profiles = FileManager.LoadProfiles(profilePath);
            if (AppInfo.Profiles.Count == 0)
            {
                // Если файл не существует, создаем его с заголовком
                FileManager.SaveProfiles(AppInfo.Profiles, profilePath);
            }

            // Выбор профиля при запуске
            SelectOrCreateProfile(profilePath);

            Console.WriteLine("Введите 'help' чтобы увидеть список команд.");
            while (true)
            {
                Console.Write(Prompt);
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    continue;

                ICommand command = CommandParser.Parse(input);
                command.Execute();

                // Сохраняем в undo-стек только команды, изменяющие состояние
                if (command is AddCommand || command is DeleteCommand || command is UpdateCommand || command is StatusCommand)
                {
                    AppInfo.UndoStack.Push(command);
                    AppInfo.RedoStack.Clear();
                }

                // Проверяем, не вышел ли пользователь из профиля
                if (AppInfo.CurrentProfileId == null)
                {
                    SelectOrCreateProfile(profilePath);
                }
            }
        }

        private static void SelectOrCreateProfile(string profilePath)
        {
            while (AppInfo.CurrentProfileId == null)
            {
                Console.Write("Войти в существующий профиль? [y/n]: ");
                string? answer = Console.ReadLine()?.Trim().ToLowerInvariant();

                if (answer == "y")
                {
                    LoginToProfile(profilePath);
                }
                else if (answer == "n")
                {
                    CreateNewProfile(profilePath);
                }
                else
                {
                    Console.WriteLine("Некорректный ответ. Введите 'y' или 'n'.");
                }
            }
        }

        private static void LoginToProfile(string profilePath)
        {
            Console.Write("Введите логин: ");
            string? login = Console.ReadLine()?.Trim();

            Console.Write("Введите пароль: ");
            string? password = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Логин и пароль не могут быть пустыми.");
                return;
            }

            var profile = AppInfo.Profiles.FirstOrDefault(p => p.Login == login && p.Password == password);
            if (profile == null)
            {
                Console.WriteLine("Неверный логин или пароль.");
                return;
            }

            AppInfo.CurrentProfileId = profile.Id;
            AppInfo.UndoStack.Clear();
            AppInfo.RedoStack.Clear();

            // Загружаем заметки для этого профиля
            string todoPath = Path.Combine(AppInfo.DataDirectory, $"todos_{profile.Id}.csv");
            TodoList todos;
            if (File.Exists(todoPath))
            {
                try
                {
                    todos = FileManager.LoadTodos(todoPath);
                    AppInfo.TodosByProfile[profile.Id] = todos;
                    Console.WriteLine($"Загружено задач: {todos.Count}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при загрузке задач: {ex.Message}");
                    todos = new TodoList();
                    AppInfo.TodosByProfile[profile.Id] = todos;
                    FileManager.SaveTodos(todos, todoPath);
                }
            }
            else
            {
                todos = new TodoList();
                AppInfo.TodosByProfile[profile.Id] = todos;
                FileManager.SaveTodos(todos, todoPath);
            }

            // Подписываем FileManager на события
            SubscribeToTodoListEvents(todos, todoPath);

            Console.WriteLine($"Вход выполнен. Пользователь: {profile.GetInfo()}");
        }

        private static void CreateNewProfile(string profilePath)
        {
            Console.Write("Введите логин: ");
            string? login = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(login))
            {
                Console.WriteLine("Логин не может быть пустым.");
                return;
            }

            // Проверяем, не существует ли уже такой логин
            if (AppInfo.Profiles.Any(p => p.Login == login))
            {
                Console.WriteLine("Пользователь с таким логином уже существует.");
                return;
            }

            Console.Write("Введите пароль: ");
            string? password = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Пароль не может быть пустым.");
                return;
            }

            Console.Write("Введите имя: ");
            string? userFirstName = Console.ReadLine();

            Console.Write("Введите фамилию: ");
            string? userLastName = Console.ReadLine();

            Console.Write("Введите год рождения: ");
            string? birthYearInput = Console.ReadLine();
            if (!int.TryParse(birthYearInput, out int userBirthYear))
            {
                Console.WriteLine("Некорректный год рождения. Используется значение по умолчанию: 2000");
                userBirthYear = 2000;
            }

            var profile = new Profile(userFirstName ?? string.Empty, userLastName ?? string.Empty, userBirthYear)
            {
                Login = login,
                Password = password
            };

            AppInfo.Profiles.Add(profile);
            FileManager.SaveProfiles(AppInfo.Profiles, profilePath);

            AppInfo.CurrentProfileId = profile.Id;
            AppInfo.UndoStack.Clear();
            AppInfo.RedoStack.Clear();

            // Создаем пустой список задач для нового профиля
            var todos = new TodoList();
            AppInfo.TodosByProfile[profile.Id] = todos;
            string todoPath = Path.Combine(AppInfo.DataDirectory, $"todos_{profile.Id}.csv");
            FileManager.SaveTodos(todos, todoPath);

            // Подписываем FileManager на события
            SubscribeToTodoListEvents(todos, todoPath);

            Console.WriteLine($"Создан новый профиль: {profile.GetInfo()}");
        }

        /// <summary>
        /// Подписывает FileManager на события TodoList для автоматического сохранения.
        /// </summary>
        private static void SubscribeToTodoListEvents(TodoList todos, string todoPath)
        {
            todos.OnTodoAdded += (item) => FileManager.SaveTodos(todos, todoPath);
            todos.OnTodoDeleted += (item) => FileManager.SaveTodos(todos, todoPath);
            todos.OnTodoUpdated += (item) => FileManager.SaveTodos(todos, todoPath);
            todos.OnStatusChanged += (item) => FileManager.SaveTodos(todos, todoPath);
        }
    }
}
