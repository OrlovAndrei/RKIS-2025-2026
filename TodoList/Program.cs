using TodoList.commands;

namespace TodoList
{
    class Program
    {
        private static string _dataDirectory = "Data";
        public static string ProfileFilePath => Path.Combine(_dataDirectory, "profile.txt");
        public static string TodoFilePath => Path.Combine(_dataDirectory, "todo.csv");
        static void Main()
        {
            Console.WriteLine("Работу выполнили: Вдовиченко и Кравец");
            FileManager.EnsureDataDirectory(_dataDirectory);

            Profile profile = FileManager.LoadProfile(ProfileFilePath) ?? new ProfileCommand().SetProfile();
            TodoList todoList = FileManager.LoadTodos(TodoFilePath);

            Console.WriteLine($"Добавлен пользователь: {profile.GetInfo()}");
            Console.WriteLine("Введите help для списка команд.");

            while (true)
            {
                Console.Write("\nВведите команду: ");
                string input = Console.ReadLine();

                ICommand command = CommandParser.Parse(input, todoList, profile);
                if (command is AddCommand or DeleteCommand or UpdateCommand or StatusCommand) AppInfo.UndoStack.Push(command);
                command?.Execute();
            }
        }
    }
}
