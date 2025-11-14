using TodoList.classes;
using TodoList.commands;

namespace TodoList;

internal class Program
{
	public static void Main()
	{
		FileManager.EnsureDataDirectory(FileManager.DataDirPath);
		if (!File.Exists(FileManager.ProfilePath)) File.WriteAllText(FileManager.ProfilePath, "Default User 2000");
		if (!File.Exists(FileManager.TodoPath)) File.WriteAllText(FileManager.TodoPath, "");
		
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