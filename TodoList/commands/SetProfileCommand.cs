using TodoList.classes;

namespace TodoList.commands;

public class SetProfileCommand : ICommand
{
	public required string[] Parts { get; init; }
	public void Execute()
	{
		CommandParser.profile = new Profile(Parts[1], Parts[2], int.Parse(Parts[3]));
		Console.WriteLine($"Профиль установлен: {CommandParser.profile.GetInfo()}");
		FileManager.SaveProfile(CommandParser.profile);
	}
}