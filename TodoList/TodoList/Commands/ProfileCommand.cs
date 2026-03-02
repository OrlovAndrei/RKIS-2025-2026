using TodoApp.Exceptions;
public class ProfileCommand : ICommand
{
	public Profile UserProfile {get; set;}
	public int CurrentYear {get; set;} = 2025;
	public bool LogoutFlag { get; set; }
	public void Execute()
	{
		if (LogoutFlag)
		{
			Console.WriteLine("Выход из профиля...");
			AppInfo.ShouldLogout = true;
		}
		else
		{
			Console.WriteLine("Пользователь: " + UserProfile.GetInfo(CurrentYear));
		}
	}
}
