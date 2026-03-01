namespace TodoList.Commands;

public class SetProfileCommand : ICommand
{
	public void Execute()
	{
		Console.Write("Введите ваше имя: ");
		var name = Console.ReadLine();
		Console.Write("Введите вашу фамилию: ");
		var surname = Console.ReadLine();

		Console.Write("Введите ваш год рождения: ");
		var year = int.Parse(Console.ReadLine());
            
		CommandParser.Profile = new Profile(name, surname, year);
		Console.WriteLine(CommandParser.Profile.GetInfo());
		FileManager.SaveProfile(CommandParser.Profile, Program.profilePath);
	}
}