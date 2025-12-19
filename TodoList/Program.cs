using TodoList.Commands;

namespace TodoList;

internal class Program
{
	private static readonly TodoList todos = new();

	public static string dataDirPath = "data";
	public static string profilePath = Path.Combine(dataDirPath, "profile.txt");
	public static void Main()
	{
		FileManager.EnsureDataDirectory(dataDirPath);
		if (!File.Exists(profilePath)) FileManager.SaveProfile(new Profile("Default", "User", 2000));
		
		Console.WriteLine($"Пользователь: {CommandParser.Profile.GetInfo()}");

		while (true)
		{
			Console.WriteLine("Введите команду: ");
			var userCommand = Console.ReadLine();
			
			var command = CommandParser.Parse(userCommand, todos);
			command.Execute();
		}
	}
}