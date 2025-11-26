using System.IO;

namespace TodoList
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
            string profilePath = Path.Combine(dataDir, "profile.txt");
            string todoPath = Path.Combine(dataDir, "todo.csv");

            FileManager.EnsureDataDirectory(dataDir);

            if (File.Exists(profilePath) && File.Exists(todoPath))
            {
                AppInfo.CurrentProfile = FileManager.LoadProfile(profilePath);
                AppInfo.Todos = FileManager.LoadTodos(todoPath);
                Console.WriteLine("Данные загружены.");
            }
            else
            {
                AppInfo.CurrentProfile = CreateNewProfile();
                AppInfo.Todos = new TodoList();
                
                FileManager.SaveProfile(AppInfo.CurrentProfile, profilePath);
                FileManager.SaveTodos(AppInfo.Todos, todoPath);
                Console.WriteLine("Созданы новые файлы данных.");
            }

            Console.WriteLine("Добро пожаловать в TodoList!");
            Console.WriteLine($"Профиль: {AppInfo.CurrentProfile.GetInfo()}");
            
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Пусто.");
                    continue;
                }

                try
                {
                    ICommand command = CommandParser.Parse(input);
                    command.Execute();
                    
                    FileManager.SaveProfile(AppInfo.CurrentProfile, profilePath);
                    FileManager.SaveTodos(AppInfo.Todos, todoPath);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
                }
            }
        }

        private static Profile CreateNewProfile()
        {
            Console.WriteLine("Создание нового профиля:");
            
            Console.Write("Имя: ");
            string firstName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(firstName))
            {
                Console.WriteLine("Имя пустое.");
                Environment.Exit(1);
            }

            Console.Write("Фамилия: ");
            string lastName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(lastName))
            {
                Console.WriteLine("Фамилия пустая.");
                Environment.Exit(1);
            }

            Console.Write("Год рождения: ");
            if (!int.TryParse(Console.ReadLine(), out int birthYear))
            {
                Console.WriteLine("Неверный год.");
                Environment.Exit(1);
            }

            return new Profile(firstName, lastName, birthYear);
        }
    }
}