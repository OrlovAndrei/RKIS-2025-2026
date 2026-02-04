
using TodoList.Commands;

namespace TodoList;
public class Program
{

	public static void Main(string[] args)
	{
		Console.WriteLine("Работу выполнили Чернова Юлия и Соловьев Иван 3833");

		// checking if data directory exist, and create if not
		FileManager.EnsureDataDirectory(FileManager.DataDirPath);

		// initialize users and tasks data
		InitializeUserData();

		// main loop
		while (true)
		{
			var commandLine = Console.ReadLine();
			var command = CommandParser.Parse(commandLine);
			command.Execute();
			if ((command is DeleteCommand || command is IndexedCommand || command is ReadCommand ||
				  command is StatusCommand || command is UpdateCommand))
			{
				AppInfo.RedoStack.Clear();
			}
		}
	}

	private static void GetUserAge(Profile profile)
	{
		Console.WriteLine("Введите год рождения:");
		string birthYearString = Console.ReadLine();

		bool isBirthYearValid = int.TryParse(birthYearString, out int birthYear);

		if (isBirthYearValid)
		{
			profile.BirthYear = birthYear;
			return;
		}
		else
		{
			Console.WriteLine("Ошибка: Некорректный год рождения, пожалуйста, введите целое число.");
			GetUserAge(profile);
		}
	}

	private static void EnterLoginPassword(int index)
	{
		Console.WriteLine("Введите пароль:");
		var password = Console.ReadLine();
		if (AppInfo.Profiles[index].Password == password)
			AppInfo.CurrentProfileID = index;
		else
			EnterLoginPassword(index);
	}

	public static void EnterExistProfile()
	{
		Console.WriteLine("Введите логин:");
		var login = Console.ReadLine();
		if (AppInfo.IsHaveLogin(login, out int index))
		{
			AppInfo.ClearCommandsHistory();
			EnterLoginPassword(index);
			Console.WriteLine($"С возвращением {AppInfo.CurrentProfile.FirstName} больше известный как {AppInfo.CurrentProfile.Login}!");
		}
		else
			EnterExistProfile();
	}

	private static void EnterProfile()
	{
		Console.WriteLine("Войти в существующий профиль? [y/n]");
		var answer = Console.ReadLine();
		if (answer == "y" || answer == "Y")
			EnterExistProfile();
		else
			CreateNewProfile();
	}

	public static void CreateNewProfile()
	{
		// setup profile
		Console.WriteLine("****\tСоздание нового профиля\t****");
		AppInfo.ClearCommandsHistory();

		// filling
		Profile profile = new Profile();

		Console.WriteLine("Введите имя:");
		profile.FirstName = Console.ReadLine();

		Console.WriteLine("Введите фамилию:");
		profile.LastName = Console.ReadLine();

		GetUserAge(profile);

		Console.WriteLine("Придумайте логин:");
		profile.Login = Console.ReadLine();

		Console.WriteLine("Придумайте пароль:");
		profile.Password = Console.ReadLine();

		profile.Id = Guid.NewGuid();

		// complete
		AppInfo.InsertNewProfile(profile);
		FileManager.SaveProfiles(FileManager.ProfileInfoPath);
	}

    // Получение данных пользователя и их обработка 
    private static void InitializeUserData()
    {
		AppInfo.Todos = new List<TodoList>();

		if (FileManager.IsThereProfilesFile() && FileManager.LoadProfiles(FileManager.ProfileInfoPath, out var profiles))
		{
			AppInfo.Profiles = profiles;
			FileManager.PrecacheTodoLists();
			EnterProfile();
		}
		else
		{
			// create new
			AppInfo.Profiles = new List<Profile>();
			CreateNewProfile();
		}
	}

}