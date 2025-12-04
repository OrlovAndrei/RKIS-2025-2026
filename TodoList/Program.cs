using TodoList.Commands;

namespace TodoList;

internal class Program
{
	private static TodoList _todoList = new();
	private static Profile _userProfile;
	
	static string dataDir = "data";
	public static string ProfileFilePath = Path.Combine(dataDir, "profile.txt");
	public static string TodoFilePath = Path.Combine(dataDir, "todo.csv");

	public static void Main()
	{
		Console.WriteLine("Работу выполнили Лютов и Легатов 3832");
		FileManager.EnsureDataDirectory(dataDir);
		_userProfile = FileManager.LoadProfile(ProfileFilePath) ?? CreateUserProfile();
		_todoList = FileManager.LoadTodos(TodoFilePath);
		Console.WriteLine(_userProfile.GetInfo());

		while (true)
		{
			Console.Write("> ");
			string input = Console.ReadLine();

			if (string.IsNullOrWhiteSpace(input))
				continue;

			ICommand command = CommandParser.Parse(input, _todoList, _userProfile);

			command.Execute();
		}
	}

	private static Profile CreateUserProfile()
	{
		Console.Write("Введите ваше имя: ");
		var firstName = Console.ReadLine();
		Console.Write("Введите вашу фамилию: ");
		var lastName = Console.ReadLine();
		Console.Write("Введите ваш год рождения: ");
		var year = int.Parse(Console.ReadLine());
		var profile = new Profile(firstName, lastName, year);
		FileManager.SaveProfile(profile, ProfileFilePath);
		return profile;
	}
}