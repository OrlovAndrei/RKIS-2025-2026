using TodoList.Commands;

namespace TodoList;

internal class Program
{
	private static Profile profile;
	private static readonly TodoList todos = new();

	public static void Main()
	{
		Console.WriteLine("Работу выполнили Антонов и Мадойкин 3833");
		Console.Write("Введите имя: ");
		var name = Console.ReadLine();
		Console.Write("Введите фамилию: ");
		var surname = Console.ReadLine();

		Console.Write("Введите год рождения: ");
		var year = int.Parse(Console.ReadLine());

		profile = new Profile(name, surname, year);
		Console.WriteLine($"Добавлен пользователь {profile.GetInfo()}");

		while (true)
		{
			Console.WriteLine("Введите команду: ");
			var userCommand = Console.ReadLine();
			
			var command = CommandParser.Parse(userCommand, todos, profile);
			command.Execute();
		}
	}
}