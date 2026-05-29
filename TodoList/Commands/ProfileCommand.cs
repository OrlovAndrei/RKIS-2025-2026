namespace TodoList.Commands;

public class ProfileCommand : ICommand
{
	public string Text { get;set; }

	public void Execute()
	{
		var parts = Text.Split(" ");
		if (parts.Length == 5)
		{
			if (parts[1] == "set")
			{
				CommandParser.Profile = new Profile(parts[2], parts[3], int.Parse(parts[4]));
				FileManager.SaveProfile(CommandParser.Profile);
			}
		}
		
		Console.WriteLine(CommandParser.Profile.GetInfo());
	}
}