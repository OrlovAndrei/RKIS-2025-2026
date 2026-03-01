namespace TodoList.Commands;

public class ProfileCommand : ICommand
{
	public Profile Profile { get; set; }
	public string Text { get;set; }

	public void Execute()
	{
		var parts = Text.Split(" ");
		if (parts.Length == 4)
		{
			Program.UserProfile = new Profile(parts[1], parts[2], int.Parse(parts[3]));
			Console.WriteLine("Ваш профиль был изменён");
		}
		else
		{
			Console.WriteLine(Profile.GetInfo());
		}
	}
}