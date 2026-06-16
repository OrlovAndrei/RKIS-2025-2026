namespace TodoList.Commands;
public class SetProfileCommand : ICommand
{
	public required string[] Parts { get; init; }
	public void Execute()
	{
		CommandParser.profile = new Profile(Parts[0], Parts[1], int.Parse(Parts[2]));
		Console.WriteLine($"Профиль установлен: {CommandParser.profile.GetInfo()}");
		FileManager.SaveProfile(CommandParser.profile);
	}
}
