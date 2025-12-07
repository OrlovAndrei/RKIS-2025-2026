internal class Program
{
	private static void Main(string[] args)
	{
		Console.WriteLine("Работу выполнили: Амелина Яна и Кабанова Арина");
		AppInfo.DataDir = "Data";
		string profilesFilePath = Path.Combine(AppInfo.DataDir, "profiles.csv");
		FileManager.EnsureDataDirectory(AppInfo.DataDir);
		AppInfo.Profiles = FileManager.LoadProfiles(profilesFilePath);
		Profile currentUser = null;
		if (AppInfo.Profiles.Count > 0)
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
		AppInfo.CurrentProfileId = currentUser.Id;
		AppInfo.CurrentUserTodos = FileManager.LoadUserTodos(AppInfo.CurrentProfileId, AppInfo.DataDir);
		Console.WriteLine($"\nТекущий пользователь: {currentUser.GetInfo(2025)}");
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
				FileManager.SaveUserTodos(AppInfo.CurrentProfileId, AppInfo.CurrentUserTodos, AppInfo.DataDir);
				isOpen = false;
				continue;
			}
			if (userCommand?.ToLower() == "out")
			{
				Console.WriteLine("Выход из профиля...");
				FileManager.SaveUserTodos(AppInfo.CurrentProfileId, AppInfo.CurrentUserTodos, AppInfo.DataDir);
				Console.WriteLine("Нажмите любую клавишу...");
			}
			try
			{
				ICommand command = CommandParser.Parse(userCommand, AppInfo.CurrentUserTodos, currentUser, profilesFilePath, AppInfo.DataDir);

				if (command != null)
				{
					command.Execute();
					if (AppInfo.ShouldLogout)
					{
						AppInfo.ShouldLogout = false;
						break;
					}
					if (command is AddCommand || command is DeleteCommand ||
						command is UpdateCommand || command is StatusCommand)
					{
						AppInfo.UndoStack.Push(command);
						FileManager.SaveUserTodos(AppInfo.CurrentProfileId, AppInfo.CurrentUserTodos, AppInfo.DataDir);
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
		foreach (var profile in AppInfo.Profiles)
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
		Console.WriteLine("\n=== СОЗДАНИЕ НОВОГО ПРОФИЛЯ ===");
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
		AppInfo.Profiles.Add(profile);
		FileManager.SaveProfile(profile, profilesFilePath);
		AppInfo.CurrentProfileId = profile.Id;
		AppInfo.CurrentUserTodos = new TodoList();
		return profile;
	}
}