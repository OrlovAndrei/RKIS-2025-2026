internal class Program
{
	public static void Main(string[] args)
	{
		Console.WriteLine("Работу выполнили: Филимонов и Фаградян 3833.9");
		Console.WriteLine("Введите свое имя");
		string name = Console.ReadLine();
		Console.WriteLine("Ведите свою фамилию");
		string surname = Console.ReadLine();
		Console.WriteLine("Ведите свой год рождения");
		int date = int.Parse(Console.ReadLine());
		int age = 2025 - date;
		Console.WriteLine("Добавлен пользователь " + name + " " + surname + ", Возраст " + age);
		
		while (true)
		{
			Console.WriteLine("Введите команду: для помощи напиши команду help");
			string userCommand = Console.ReadLine();
			switch (userCommand.Split()[0])
			{
				case "help":
					Console.WriteLine("help - выводит список всех доступных команд\n" +
					                  "profile - выводит ваши данные");
					break;
				case "profile":
					Console.WriteLine("Пользователь: " + name + " " + surname + ", Возраст " + age);
					break;
				default:
					Console.WriteLine("Неправильно введена команда");
					break;
			}
		}
	}
}