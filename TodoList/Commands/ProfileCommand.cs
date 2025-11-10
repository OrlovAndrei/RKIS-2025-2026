namespace TodoList.Commands;

public class ProfileCommand : ICommand
{
	public Profile Profile { get; set; }

	public void Execute()
	{
		Console.WriteLine(Profile.GetInfo());
	}
	
	public static Profile GetProfile()
	{
		Console.Write("Введите ваше имя: ");
		var userFirstName = Console.ReadLine();
		Console.Write("Введите вашу фамилию: ");
		var userLastName = Console.ReadLine();
		Console.Write("Введите ваш год рождения: ");
		var userBirthYear = int.Parse(Console.ReadLine());
		return new Profile(userFirstName, userLastName, userBirthYear);
	}
}