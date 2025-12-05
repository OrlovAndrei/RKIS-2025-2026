using System.IO;
using System.Linq;

namespace TodoList
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
            FileManager.EnsureDataDirectory(dataDir);

            AppInfo.Profiles = FileManager.LoadProfiles(dataDir);
            
            Console.WriteLine("Добро пожаловать в TodoList!");

            if (AppInfo.Profiles.Count > 0)
            {
                Console.Write("Войти в существующий профиль? [y/n]: ");
                string response = Console.ReadLine()?.Trim().ToLower();

                if (response == "y" || response == "yes" || response == "да")
                {
                    if (!LoginToExistingProfile(dataDir))
                    {
                        CreateNewProfile(dataDir);
                    }
                }
                else
                {
                    CreateNewProfile(dataDir);
                }
            }
            else
            {
                Console.WriteLine("Профили не найдены. Создание нового профиля.");
                CreateNewProfile(dataDir);
            }

            if (AppInfo.CurrentProfileId.HasValue)
            {
                StartMainLoop(dataDir);
            }
        }

        private static bool LoginToExistingProfile(string dataDir)
        {
            Console.Write("Логин: ");
            string login = Console.ReadLine();

            Console.Write("Пароль: ");
            string password = Console.ReadLine();

            var profile = AppInfo.Profiles.FirstOrDefault(p => p.Login == login && p.CheckPassword(password));
            if (profile != null)
            {
                AppInfo.CurrentProfileId = profile.Id;
                
                AppInfo.UndoStack.Clear();
                AppInfo.RedoStack.Clear();
                
                if (!AppInfo.TodosByUser.ContainsKey(profile.Id))
                {
                    var todoList = FileManager.LoadTodos(profile.Id, dataDir);
                    AppInfo.TodosByUser[profile.Id] = todoList;
                }
                
                Console.WriteLine($"Вход выполнен. Профиль: {profile.GetInfo()}");
                return true;
            }
            else
            {
                Console.WriteLine("Неверный логин или пароль.");
                return false;
            }
        }

        private static void CreateNewProfile(string dataDir)
        {
            Console.WriteLine("Создание нового профиля:");
            
            Console.Write("Логин: ");
            string login = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(login))
            {
                Console.WriteLine("Логин не может быть пустым.");
                Environment.Exit(1);
            }

            Console.Write("Пароль: ");
            string password = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Пароль не может быть пустым.");
                Environment.Exit(1);
            }

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

            var profile = new Profile(login, password, firstName, lastName, birthYear);
            AppInfo.Profiles.Add(profile);
            AppInfo.CurrentProfileId = profile.Id;
            
            AppInfo.TodosByUser[profile.Id] = new TodoList();
            
            AppInfo.UndoStack.Clear();
            AppInfo.RedoStack.Clear();

            FileManager.SaveProfiles(AppInfo.Profiles, dataDir);
            
            Console.WriteLine($"Профиль создан: {profile.GetInfo()}");
        }

        private static void StartMainLoop(string dataDir)
        {
            while (true)
            {
                Console.Write($"[{AppInfo.CurrentProfile?.Login}]> ");
                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Пусто.");
                    continue;
                }

                try
                {
                    ICommand command = CommandParser.Parse(input);
                    if (command != null)
                    {
                        command.Execute();
                        
                        FileManager.SaveProfiles(AppInfo.Profiles, dataDir);
                        if (AppInfo.CurrentProfileId.HasValue)
                        {
                            FileManager.SaveTodos(AppInfo.CurrentProfileId.Value, AppInfo.CurrentTodos, dataDir);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Неизвестная команда");
                    }
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
    }
}