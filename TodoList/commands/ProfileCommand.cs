namespace TodoList.commands;

public class ProfileCommand : ICommand
{
	public Profile Profile { get; set; }

	public void Execute()
	{
		Console.WriteLine(Profile.GetInfo());
	}

	public static Profile AddUser()
	{
		Console.WriteLine("Введите свое имя");
		var name = Console.ReadLine();
		Console.WriteLine("Ведите свою фамилию");
		var surname = Console.ReadLine();
		Console.WriteLine("Ведите свой год рождения");
		var date = int.Parse(Console.ReadLine());
		var age = 2025 - date;
		
		var user = new Profile(name, surname, age);
		Console.WriteLine("Добавлен пользователь " + user.GetInfo());
		return user;
	}
}