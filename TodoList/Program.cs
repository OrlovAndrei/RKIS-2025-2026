using System;
using System.IO;

class Program
{
    private static TodoList _todoList = new TodoList();
    private static Profile _userProfile;
    private static string _dataDirectory = "Data";
    private static string _profileFilePath;
    private static string _todoFilePath;
    static void Main()
    {
        _profileFilePath = Path.Combine(_dataDirectory, "profile.txt");
        _todoFilePath = Path.Combine(_dataDirectory, "todo.csv");

        FileManager.EnsureDataDirectory(_dataDirectory);

        LoadData();

        RunTodoApplication();
    }
    static void LoadData()
    {
        _ AppInfo.CurrentProfile = FileManager.LoadProfile(_profileFilePath);
        if (AppInfo.CurrentProfile == null)
        {
            InitializeUserProfile();
            FileManager.SaveProfile(AppInfo.CurrentProfile, _profileFilePath);
        }
        else
        {
            Console.WriteLine($"Загружен профиль: {AppInfo.CurrentProfile.GetInfo()}");
        }

        AppInfo.Todos = FileManager.LoadTodos(_todoFilePath);
        if (AppInfo.Todos.Count > 0)
        {
            Console.WriteLine($"Загружено задач: {AppInfo.Todos.Count}");
        }

    }
    static void InitializeUserProfile()
    {
        Console.Write("Введите имя: ");
        string firstName = Console.ReadLine();

        Console.Write("Введите фамилию: ");
        string lastName = Console.ReadLine();

        Console.Write("Введите год рождения: ");
        string yearInput = Console.ReadLine();

        int birthYear;
        if (!int.TryParse(yearInput, out birthYear))
        {
            Console.WriteLine("Неверный формат года. Установлен 2000 год по умолчанию.");
            birthYear = 2000;
        }

        AppInfo.CurrentProfile = new Profile(firstName, lastName, birthYear);

        int currentYear = DateTime.Now.Year;
        int age = currentYear - birthYear;

        Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");
    }

    static void RunTodoApplication()
    {
        Console.WriteLine("Добро пожаловать в TodoList! Введите 'help' для списка команд.");

        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                continue;

            ICommand command = CommandParser.Parse(input, AppInfo.Todos, AppInfo.CurrentProfile, _todoFilePath, _profileFilePath);

            command.Execute();
        }
    }
}