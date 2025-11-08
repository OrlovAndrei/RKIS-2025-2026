using TodoList.classes;

namespace TodoList.commands;

public class SetProfileCommand : ICommand
{
	public string[] parts { get; set; }
	public void Execute()
	{
		CommandParser.profile = new Profile(parts[1], parts[2], int.Parse(parts[3]));
		Console.WriteLine($"Профиль установлен: {CommandParser.profile.GetInfo()}");
		FileManager.SaveProfile(CommandParser.profile);
	}
}