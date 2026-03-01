using TodoList.Commands;

namespace TodoList;

internal class Program
{

	public static void Main()
	{
		FileManager.EnsureDataDirectory(FileManager.DataDirPath);
		if (!File.Exists(FileManager.TodoPath)) File.WriteAllText(FileManager.TodoPath, "");
		if (!File.Exists(FileManager.ProfilePath)) File.WriteAllText(FileManager.ProfilePath, "Default User 2000");
		Console.WriteLine("Выполнили курсанты группы 3833 Кучеренко и Мурза");
		TodoList todoList = new();

		while (true)
		{
			Console.WriteLine("Введите команду: ");
			string input = Console.ReadLine();

			ICommand command = CommandParser.Parse(input);
			command.Execute();
			FileManager.SaveTodos(CommandParser.todoList);
		}
	}

}