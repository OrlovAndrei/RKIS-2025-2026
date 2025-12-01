using System;
using System.IO;

namespace TodoList
{
    internal class Program
    {
        private const string Prompt = "> ";
        private const string DataDirectory = "data";

        private static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Буряк Степан Геннадьевич и Голубев Данил Сергеевич");

            // Определяем пути к файлам
            string dataDirPath = Path.Combine(Directory.GetCurrentDirectory(), DataDirectory);
            string profilePath = Path.Combine(dataDirPath, "profile.txt");
            string todoPath = Path.Combine(dataDirPath, "todo.csv");

            // Создаем папку для данных, если её нет
            FileManager.EnsureDataDirectory(dataDirPath);

            Profile profile;
            TodoList todos;

            // Загружаем или создаем профиль
            if (File.Exists(profilePath))
            {
                try
                {
                    profile = FileManager.LoadProfile(profilePath);
                    Console.WriteLine($"Загружен пользователь {profile.GetInfo()}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при загрузке профиля: {ex.Message}");
                    // Создаем новый профиль
                    profile = CreateNewProfile();
                    FileManager.SaveProfile(profile, profilePath);
                }
            }
            else
            {
                profile = CreateNewProfile();
                FileManager.SaveProfile(profile, profilePath);
                Console.WriteLine($"Добавлен пользователь {profile.GetInfo()}");
            }

            // Загружаем или создаем список задач
            if (File.Exists(todoPath))
            {
                try
                {
                    todos = FileManager.LoadTodos(todoPath);
                    Console.WriteLine($"Загружено задач: {todos.Count}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при загрузке задач: {ex.Message}");
                    todos = new TodoList();
                    FileManager.SaveTodos(todos, todoPath);
                }
            }
            else
            {
                todos = new TodoList();
                FileManager.SaveTodos(todos, todoPath);
            }

            Console.WriteLine("Введите 'help' чтобы увидеть список команд.");
            while (true)
            {
                Console.Write(Prompt);
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    continue;

                ICommand command = CommandParser.Parse(input, todos, profile, todoPath, profilePath);
                command.Execute();
            }
        }

        private static Profile CreateNewProfile()
        {
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

            return new Profile(userFirstName ?? string.Empty, userLastName ?? string.Empty, userBirthYear);
        }
    }
}
