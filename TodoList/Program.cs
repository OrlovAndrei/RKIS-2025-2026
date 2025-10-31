namespace TodoList;

class Program
{
	static void Main()
	{
		Console.WriteLine("Работу выполнили Кулаков и Рублёв 3833");
		Console.Write("Введите имя: ");
		var name = Console.ReadLine();
		Console.Write("Введите фамилию: ");
		var surname = Console.ReadLine();
		Console.Write("Введите год рождения: ");
		var year = int.Parse(Console.ReadLine());

		var profile = new Profile(name, surname, year);
		TodoList todoList = new();

		Console.WriteLine($"Добавлен пользователь {profile.GetInfo()}");

		while (true)
		{
			Console.WriteLine("Введите команду: ");
			string input = Console.ReadLine();

			ICommand command = CommandParser.Parse(input, todoList, profile);
			command.Execute();
		}
	}
}