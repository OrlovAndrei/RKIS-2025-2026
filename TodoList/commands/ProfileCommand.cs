namespace TodoList.commands;

public class ProfileCommand : ICommand
{
	public required Profile Profile { get; init; }

	public void Execute()
	{
		Console.WriteLine(Profile.GetInfo());
	}
}