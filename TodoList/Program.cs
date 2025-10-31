using TodoList.classes;
using TodoList.commands;

namespace TodoList;

internal class Program
{
	public static void Main()
	{
		Console.WriteLine("Работу выполнели Леошко и Петренко 3833");

		TodoList todoList = new();
		var profile = ProfileCommand.GetProfile();
		Console.WriteLine(profile.GetInfo());

		while (true)
		{
			Console.Write("\nВведите команду: ");
			var input = Console.ReadLine();

			var command = CommandParser.Parse(input, todoList, profile);
			command.Execute();
		}
	}
}