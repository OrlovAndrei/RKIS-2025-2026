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

            Profile profile;
            TodoList todoList;

            if (File.Exists(profilePath))
            {
                profile = FileManager.LoadProfile(profilePath);
                
                if (profile != null)
                {
                    string userTodoPath = Path.Combine(dataDir, $"todo_{profile.Id}.csv");
                    
                    if (File.Exists(userTodoPath))
                    {
                        todoList = FileManager.LoadTodos(userTodoPath);
                        Console.WriteLine("Данные загружены.");
                    }
                    else
                    {
                        todoList = new TodoList();
                        Console.WriteLine("Создан новый список задач для профиля.");
                    }
                }
                else
                {
                    Console.WriteLine("Не удалось загрузить профиль. Создание нового профиля.");
                    profile = CreateNewProfile();
                    todoList = new TodoList();
                    
                    FileManager.SaveProfile(profile, profilePath);
                    FileManager.SaveTodos(todoList, Path.Combine(dataDir, $"todo_{profile.Id}.csv"));
                }
            }
            else
            {
                Console.WriteLine("Профиль не найден. Создание нового профиля.");
                profile = CreateNewProfile();
                todoList = new TodoList();
                
                FileManager.SaveProfile(profile, profilePath);
                FileManager.SaveTodos(todoList, Path.Combine(dataDir, $"todo_{profile.Id}.csv"));
            }

            AppInfo.Todos = todoList;
            AppInfo.CurrentProfile = profile;
            AppInfo.UndoStack.Clear();
            AppInfo.RedoStack.Clear();

            Console.WriteLine("Добро пожаловать в TodoList!");
            Console.WriteLine($"Профиль: {profile.GetInfo()}");
            
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
                    ICommand command = CommandParser.Parse(input, todoList, profile);
                    command.Execute();
                    
                    FileManager.SaveProfile(profile, profilePath);
                    FileManager.SaveTodos(todoList, Path.Combine(dataDir, $"todo_{profile.Id}.csv"));
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
                Console.WriteLine("Имя не может быть пустым.");
                Environment.Exit(1);
            }

            Console.Write("Фамилия: ");
            string lastName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(lastName))
            {
                Console.WriteLine("Фамилия не может быть пустой.");
                Environment.Exit(1);
            }

            Console.Write("Год рождения: ");
            if (!int.TryParse(Console.ReadLine(), out int birthYear))
            {
                Console.WriteLine("Неверный формат года.");
                Environment.Exit(1);
            }

            return new Profile(firstName, lastName, birthYear);
        }
    }
}