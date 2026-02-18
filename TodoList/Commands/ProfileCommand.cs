
namespace TodoList;
internal class ProfileCommand : ICommand
{
	private bool _out;

	public ProfileCommand(bool outFlag)
	{
		_out = outFlag;
	}

	public void Execute()
	{
		if (_out)
		{
			Console.WriteLine("Войти в уже существующий профиль 1 / Создать новый профиль 2.");
			var answer = Console.ReadLine();
			if (answer == "1") Program.EnterExistProfile();
			else Program.CreateNewProfile();
		}
		else
		{
			int currentYear = DateTime.Now.Year;
			string info = $"{AppInfo.CurrentProfile.FirstName} {AppInfo.CurrentProfile.LastName}, возраст {currentYear - AppInfo.CurrentProfile.BirthYear}";
			Console.WriteLine($"Пользователь {info}");
		}
	}

	public void Unexecute() { }
}


