namespace TodoList.Commands;

public class ProfileCommand : ICommand
{
	public Profile profile { get; set; }

	public void Execute()
	{
		Console.WriteLine(profile.GetInfo());
	}
}