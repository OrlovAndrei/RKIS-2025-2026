using TodoList.classes;
using TodoList.commands;

namespace TodoList;

internal class Program
{
	public static void Main()
	{
		FileManager.EnsureDataDirectory(FileManager.dataDirPath);
		if (!File.Exists(FileManager.profilePath)) File.WriteAllText(FileManager.profilePath, "Default User 2000");
		if (!File.Exists(FileManager.todoPath)) File.WriteAllText(FileManager.todoPath, "");
		
		Console.WriteLine("Работу выполнели Леошко и Петренко 3833");

		while (true)
		{
			Console.Write("\nВведите команду: ");
			var input = Console.ReadLine();

			var command = CommandParser.Parse(input);
			command.Execute();
			FileManager.SaveTodos(CommandParser.todoList);
		}
	}
}