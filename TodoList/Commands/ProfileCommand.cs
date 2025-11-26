namespace TodoApp.Commands;
public class ProfileCommand : BaseCommand
{
	public Profile Profile { get; set; }
	public ProfileCommand()
	{
		Profile = AppInfo.CurrentProfile;
	}

	public override void Execute()
	{
		Console.WriteLine(Profile.GetInfo());
	}

	public override void Unexecute()
	{
		Console.WriteLine("Отмена просмотра профиля (нет изменений для отмены)");
	}
}

