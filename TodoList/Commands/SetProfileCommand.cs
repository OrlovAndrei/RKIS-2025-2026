namespace TodoList.Commands;

public class SetProfileCommand : ICommand
{
	public string[] parts { get; set; }
	public void Execute()
	{
		CommandParser.profile = new Profile(parts[0], parts[1], int.Parse(parts[2]));
		Console.WriteLine($"Профиль установлен: {CommandParser.profile.GetInfo()}");
		FileManager.SaveProfile(CommandParser.profile);
	}
}