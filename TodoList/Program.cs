namespace TodoList;

internal class Program
{
	public static void Main()
	{
		Console.WriteLine("Работу выполнили Антонов и Мадойкин 3833");
		Console.Write("Введите имя: ");
		var name = Console.ReadLine();
		Console.Write("Введите фамилию: ");
		var surname = Console.ReadLine();

		Console.Write("Введите год рождения: ");
		var year = int.Parse(Console.ReadLine());
		var age = DateTime.Now.Year - year;

		Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");

		while (true)
		{
			Console.Write("\nВведите команду: ");
			var command = Console.ReadLine();

			if (command == "help") Console.WriteLine(
					"""
					Доступные команды:
					help — список команд
					profile — выводит данные профиля
					exit — завершить программу
					"""
				);
			else if (command == "profile") Console.WriteLine($"{name} {surname}, {year}");
			else if (command == "exit")
			{
				Console.WriteLine("Программа завершена.");
				break;
			}
			else Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
		}
	}
}