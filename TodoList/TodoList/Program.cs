internal class Program
{
	private static List<Profile> profiles = new List<Profile>();
	private static Guid currentProfileId = Guid.Empty;
	private static TodoList currentUserTodos = new TodoList();
	private static string dataDir = "Data";
	private static void Main(string[] args)
	{
		Console.WriteLine("Я верю, что Арина вам обьяснят каждую непонятную строчку кода!(Секретное послание на субботу)");
		Console.WriteLine("Работу выполнили: Амелина Яна и Кабанова Арина");
		string profilesFilePath = Path.Combine(dataDir, "profiles.csv");
		FileManager.EnsureDataDirectory(dataDir);
		profiles = FileManager.LoadProfiles(profilesFilePath);
		currentUserTodos = FileManager.LoadUserTodos(currentProfileId, dataDir);
		Profile currentUser = null;
		if (profiles.Count > 0)
		{
			Console.Write("Войти в существующий профиль? [y/n]: ");
			string choice = Console.ReadLine()?.ToLower();
			if (choice == "y")
			{
				currentUser = LoginToProfile();
			}
			else
			{
				currentUser = CreateNewProfile(profilesFilePath);
			}
		}
		else
		{
			Console.WriteLine("Профили не найдены. Создайте новый профиль.");
			currentUser = CreateNewProfile(profilesFilePath);
		}
		if (currentUser == null)
		{
			Console.WriteLine("Не удалось загрузить профиль. Программа завершается.");
			return;
		}
		currentProfileId = currentUser.Id;
		Console.WriteLine($"\nТекущий пользователь: {currentUser.GetInfo(2025)}");
		currentUserTodos = FileManager.LoadUserTodos(currentProfileId, dataDir);
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
				FileManager.SaveUserTodos(currentProfileId, currentUserTodos, dataDir);
				isOpen = false;
				continue;
			}
			try
			{
				ICommand command = CommandParser.Parse(userCommand, currentUserTodos, currentUser, profilesFilePath, dataDir);

				if (command != null)
				{
					command.Execute();
					if (command is AddCommand || command is DeleteCommand ||
						command is UpdateCommand || command is StatusCommand)
					{
						AppInfo.UndoStack.Push(command);
						FileManager.SaveUserTodos(currentProfileId, currentUserTodos, dataDir);
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
	private static Profile LoginToProfile()
	{
		Console.Write("Введите логин: ");
		string login = Console.ReadLine();
		Console.Write("Введите пароль: ");
		string password = Console.ReadLine();
		foreach (var profile in profiles)
		{
			if (profile.Login == login && profile.Password == password)
			{
				Console.WriteLine($"Вход выполнен. Привет, {profile.FirstName}!");
				return profile;
			}
		}
		Console.WriteLine("Неверный логин или пароль. Создайте новый профиль.");
		return null;
	}
	private static Profile CreateNewProfile(string profilesFilePath)
	{
		Console.WriteLine("\n==!!!СОЗДАНИЕ НОВОГО ПРОФИЛЯ!!!==");
		Console.Write("Логин: ");
		string login = Console.ReadLine();
		Console.Write("Пароль: ");
		string password = Console.ReadLine();
		Console.Write("Имя: ");
		string firstName = Console.ReadLine();
		Console.Write("Фамилия: ");
		string lastName = Console.ReadLine();
		Console.Write("Год рождения: ");
		int birthYear;
		while (!int.TryParse(Console.ReadLine(), out birthYear))
		{
			Console.Write("Введите корректный год рождения: ");
		}
		var profile = new Profile(login, password, firstName, lastName, birthYear);
		Console.WriteLine($"\nПрофиль создан: {profile.GetInfo(2025)}");
		profiles.Add(profile);
		FileManager.SaveProfile(profile, profilesFilePath);
		return profile;
	}
}