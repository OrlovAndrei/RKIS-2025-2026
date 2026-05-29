using TodoList.Commands;

namespace TodoList;

internal class Program
{
	public static string dataDirPath = "data";
	public static string profilePath = Path.Combine(dataDirPath, "profile.txt");
	public static string todoPath = Path.Combine(dataDirPath, "todos.csv");
	public static void Main()
	{
		FileManager.EnsureDataDirectory(dataDirPath);
		if (!File.Exists(profilePath)) FileManager.SaveProfile(new Profile("Default", "User", 2000));
		if (!File.Exists(todoPath)) FileManager.SaveTodos(new TodoList());

		Console.WriteLine($"Пользователь: {CommandParser.Profile.GetInfo()}");

		while (true)
		{
			Console.WriteLine("Введите команду: ");
			var userCommand = Console.ReadLine();
			
			var command = CommandParser.Parse(userCommand);
			command.Execute();
			FileManager.SaveTodos(CommandParser.TodoList);
		}
	}
}