namespace TodoList;

internal class Program
{
	static TodoList todoList = new();
	public static void Main(string[] args)
	{
		Console.WriteLine("Работу выполнили: Филимонов и Фаградян 3833.9");
		Profile profile = AddUser();
		
		while (true)
		{
			Console.WriteLine("Введите команду: ");
			var userCommand = Console.ReadLine();
			
			var command = CommandParser.Parse(userCommand, todoList, profile);
			command.Execute();
		}
	}

	private static Profile AddUser()
	{
		Console.WriteLine("Введите свое имя");
		var name = Console.ReadLine();
		Console.WriteLine("Ведите свою фамилию");
		var surname = Console.ReadLine();
		Console.WriteLine("Ведите свой год рождения");
		int date = int.Parse(Console.ReadLine());
		var age = 2025 - date;
		
		var user = new Profile(name, surname, age);
		Console.WriteLine("Добавлен пользователь " + user.GetInfo());
		return user;
	}
}