internal class Program
{
	private static void Main(string[] args)
	{
		Console.WriteLine("Работу выполнили: Амелина Яна и Кабанова Арина");
		string dataDir = "Data";
		string profileFilePath = Path.Combine(dataDir, "profile.txt");
		string todoFilePath = Path.Combine(dataDir, "todo.csv");
		FileManager.EnsureDataDirectory(dataDir);
		Profile userProfile = FileManager.LoadProfile(profileFilePath) ?? CreateUserProfile(profileFilePath);
		TodoList todos = FileManager.LoadTodos(todoFilePath);
		bool isOpen = true;
		Console.ReadKey();
		while (isOpen)
		{
			Console.Clear();
			string userCommand = "";
			Console.WriteLine("Введите команду:\nдля помощи напиши команду help");
			userCommand = Console.ReadLine();
			if (userCommand?.ToLower() == "exit")
			{
				FileManager.SaveProfile(userProfile, profileFilePath);
				FileManager.SaveTodos(todos, todoFilePath);
				isOpen = false;
				continue;
			}
			try
			{
				ICommand command = CommandParser.Parse(userCommand, todos, userProfile, profileFilePath, todoFilePath);

				if (command != null)
				{
					command.Execute();
					if (command is AddCommand || command is DeleteCommand ||
						command is UpdateCommand || command is StatusCommand)
					{
						AppInfo.UndoStack.Push(command);
						AppInfo.RedoStack.Clear();
						FileManager.SaveTodos(todos, todoFilePath);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка: {ex.Message}");
			}
			Console.ReadKey();
		}
	}
	private static Profile CreateUserProfile(string profilesFilePath)
	{
		Console.WriteLine("Напишите ваше имя и фамилию:");
		string fullName;
		while (string.IsNullOrEmpty(fullName = Console.ReadLine()))
		{
			Console.WriteLine("Вы ничего не ввели");
		}
		string[] splitFullName = fullName.Split(' ', 2);
		string name = splitFullName[0];
		string surname = splitFullName.Length > 1 ? splitFullName[1] : "";
		Console.WriteLine("Напишите свой год рождения:");
		int yearOfBirth = int.Parse(Console.ReadLine());
		Console.WriteLine("Придумайте логин:");
		string login = Console.ReadLine();
		Console.WriteLine("Придумайте пароль:");
		string password = Console.ReadLine();
		var profile = new Profile(login, password, name, surname, yearOfBirth);
		Console.WriteLine("Добавлен пользователь: " + profile.GetInfo(2025));

		FileManager.SaveProfile(profile, profilesFilePath);
		return profile;
	}
}