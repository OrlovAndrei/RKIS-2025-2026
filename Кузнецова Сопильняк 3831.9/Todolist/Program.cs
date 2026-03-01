namespace TodoList
{
    public sealed class Program
    {
        private const int intialArraySize = 2;
        private const int arrayExpansionMultiplier = 2;

        static void Main(string[] args)
        {
            Console.WriteLine("Powered by Yar and Angelina");

            bool showHelpAtStart = false;
            bool skipProfile = false;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--help" || args[i] == "-h")
                {
                    showHelpAtStart = true;
                }
                else if (args[i] == "--skip-profile" || args[i] == "-s")
                {
                    skipProfile = true;
                }
                else if (args[i] == "--version" || args[i] == "-v")
                {
                    Console.WriteLine("TodoList v1.0");
                    return;
                }
            }

            if (showHelpAtStart)
            {
                ICommand helpCommand = new HelpCommand();
                helpCommand.Execute();
            }

            string dataDir = "data";
            string profileFilePath = Path.Combine(dataDir, "profile.txt");
            string todoFilePath = Path.Combine(dataDir, "todo.csv");

            FileManager.EnsureDataDirectory(dataDir);

            Profile profile = FileManager.LoadProfile(profileFilePath);
            if (profile == null)
            {
                profile = new Profile();
            }

            TodoList todoList = FileManager.LoadTodos(todoFilePath);
            if (todoList == null)
            {
                todoList = new TodoList();
            }

            if (!skipProfile && profile.FirstName == null)
            {
                Console.Write("Введите имя: ");
                profile.FirstName = Console.ReadLine();

                Console.Write("Введите фамилию: ");
                profile.LastName = Console.ReadLine();

                Console.Write("Введите возраст: ");
                string yearInput = Console.ReadLine();

                int birthYear;
                while (!int.TryParse(yearInput, out birthYear))
                {
                    Console.Write("Ошибка! Введите корректный возраст: ");
                    yearInput = Console.ReadLine();
                }
                profile.BirthYear = birthYear;
                Console.WriteLine($"Добро пожаловать пользователь {profile.GetInfo()}");
                FileManager.SaveProfile(profile, profileFilePath);
            }
            
            while (true)
            {
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                ICommand command = CommandParser.Parse(input, todoList, profile);
                command.Execute();
            }
        }
    }
}