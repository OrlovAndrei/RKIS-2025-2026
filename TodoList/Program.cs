using TodoList.Commands;

namespace TodoList
{
	class Program
	{
		public static void Main()
		{
			Console.WriteLine("Работу выполнили Поплевин и Музыка 3831");

			TodoList todoList = new();
			var profile = ProfileCommand.GetProfile();
			Console.WriteLine(profile.GetInfo());

			while (true)
			{
				Console.Write("Введите команду: ");
				var input = Console.ReadLine();

				var command = CommandParser.Parse(input, todoList, profile);
				command.Execute();
			}
		}
	}
}