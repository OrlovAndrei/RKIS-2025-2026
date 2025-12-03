using TodoList.Commands;

namespace TodoList;

internal class Program
{
	private static readonly TodoList _todoList = new();
	private static Profile _userProfile;

	public static void Main()
	{
		Console.WriteLine("Работу  выполнили Лютов и Легатов 3832");
		Console.Write("Введите ваше имя: ");
		var firstName = Console.ReadLine();
		Console.Write("Введите вашу фамилию: ");
		var lastName = Console.ReadLine();

		Console.Write("Введите ваш год рождения: ");
		var yearInput = Console.ReadLine();
		int year;
		if (!int.TryParse(yearInput, out year))
		{
			Console.WriteLine("Неверный формат года. Установлен 2000 год по умолчанию.");
			year = 2000;
		}

		_userProfile = new Profile(firstName, lastName, year);

		var text = "Добавлен пользователь " + _userProfile.GetInfo();
		Console.WriteLine(text);

		while (true)
		{
			Console.Write("> ");
			string input = Console.ReadLine();

			if (string.IsNullOrWhiteSpace(input))
				continue;

			ICommand command = CommandParser.Parse(input, _todoList, _userProfile);

			command.Execute();
		}
	}
}