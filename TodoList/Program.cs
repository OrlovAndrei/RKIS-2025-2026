using TodoList.commands;

namespace TodoList;

internal class Program
{
	private static void Main(string[] args)
	{
		Console.WriteLine("Работу выполнил: Морозов Иван 3833.9");
		TodoList todoList = new();
		var profile = ProfileCommand.AddUser();

		while (true)
		{
			Console.WriteLine("Введите команду: ");
			var userCommand = Console.ReadLine();
			
			var command = CommandParser.Parse(userCommand, todoList, profile);
			command.Execute();
		}
	}
}