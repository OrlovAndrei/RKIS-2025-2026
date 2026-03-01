using TodoList.Commands;

namespace TodoList
{
    class Program
    {
	    public static string dataDirPath = "data";
	    public static string profilePath = Path.Combine(dataDirPath, "profile.txt");
	    public static string todoFilePath = Path.Combine(dataDirPath, "todo.csv");
        public static void Main()
        {
	        FileManager.EnsureDataDirectory(dataDirPath);
	        if (!File.Exists(profilePath)) File.WriteAllText(profilePath, "Default;User;2000");
	        if (!File.Exists(todoFilePath)) File.WriteAllText(todoFilePath, "");
	        
            Console.WriteLine("Работу выполнили: Галстян и Дзуцев");
            Console.WriteLine(CommandParser.Profile.GetInfo());
            while (true)
            {
	            Console.WriteLine("Введите команду: ");
	            string input = Console.ReadLine();

	            ICommand command = CommandParser.Parse(input);
	            command.Execute();
	            FileManager.SaveTodos(CommandParser.TodoList, todoFilePath);
            }
        }
    }
}