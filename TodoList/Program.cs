using System;
using System.IO;
using System.Text;
using TodoApp.Commands;
using TodoApp.Commands;
class Program
{
	private static bool _shouldReturnToProfileSelection = false;
	static void Main(string[] args)
	{
		string dataDir = Path.Combine(Environment.CurrentDirectory, "data");
    	string profilePath = Path.Combine(dataDir, "profile.csv");
    	AppInfo.ProfilesFilePath = profilePath;
		FileManager.EnsureDataDirectory(dataDir);
		AppInfo.Profiles = FileManager.LoadAllProfiles(profilePath);
		RunApplicationLoop();
	}	
	static void ShowProfileSelection()
	{
    	Console.Write("Войти в существующий профиль? [y/n]: ");
    	string choice = Console.ReadLine()?.Trim().ToLower();
	    if (choice == "y")
    	{
        	LoginUser(AppInfo.ProfilesFilePath);
    	}
    	else
    	{
    	    CreateNewProfile(AppInfo.ProfilesFilePath);
    	}
	}
	static void RunApplicationLoop()
    {
        while (true)
        {
			ShowProfileSelection();
            if (AppInfo.CurrentProfile != null)
            {
                RunMainLoop();
            }
        }
    }
	static void RunMainLoop()
        {
            AppInfo.ResetUndoRedo();
            Console.Clear();
            Console.WriteLine($"Добро пожаловать, {AppInfo.CurrentProfile.FirstName}!");
            Console.WriteLine("Введите 'help' для списка команд");
            Console.WriteLine("Введите 'profile --out' для выхода из профиля");
            Console.WriteLine();
            while (true)
            {
                try
                {
                    Console.Write("> ");
                    string commandInput = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(commandInput)) 
                        continue;
                    BaseCommand command = CommandParser.Parse(commandInput, AppInfo.CurrentProfileId, AppInfo.Todos);
                    ExecuteAndStoreCommand(command);
                    if (AppInfo.CurrentProfile == null)
                    {
                        break;
                    }
                }
        	catch (Exception ex)
        	{
            	Console.WriteLine($"Ошибка: {ex.Message}");
        	}
    	}
	}
	static void LoginUser(string profilePath)
	{
		Console.Write("Логин: ");
		string login = Console.ReadLine()?.Trim();
		Console.Write("Пароль: ");
		string password = Console.ReadLine()?.Trim();

		var profile = AppInfo.Profiles
			.FirstOrDefault(p => p.Login == login && p.Password == password);
		if (profile != null)
		{
			AppInfo.CurrentProfileId = profile.Id;
			Console.WriteLine($"Успешно вошли: {profile.GetInfo()}");
			LoadUserTodos();
			Console.WriteLine($"Загружено задач: {AppInfo.Todos.Count}");
            AppInfo.ResetUndoRedo();
            Console.WriteLine("TodoApp с системой Undo/Redo");
            Console.WriteLine("Введите 'help' для списка команд");
		}
		else
		{
			Console.WriteLine("Неверный логин или пароль.");
			Environment.Exit(1);
		}
	}
	static void CreateNewProfile(string profilePath)
	{
		Console.Write("Логин: ");
		string login = Console.ReadLine()?.Trim() ?? "";
		if (string.IsNullOrWhiteSpace(login))
		{
			Console.WriteLine("Логин не может быть пустым.");
			return;
		}
		if (!IsLoginUnique(login))
		{
			Console.WriteLine("Ошибка: логин уже существует.");
			return;
		}
		Console.Write("Пароль: ");
		string password = Console.ReadLine()?.Trim() ?? "";
		if (string.IsNullOrWhiteSpace(password))
		{
			Console.WriteLine("Пароль не может быть пустым.");
			return;
		}
		Console.Write("Имя: ");
		string firstName = Console.ReadLine()?.Trim() ?? "";
		Console.Write("Фамилия: ");
		string lastName = Console.ReadLine()?.Trim() ?? "";
		Console.Write("Год рождения: ");
		if (!int.TryParse(Console.ReadLine(), out int yearOfBirth))
		{
			Console.WriteLine("Некорректный год рождения. Используется 0.");
			yearOfBirth = 0;
		}
		var newProfile = new Profile(login, password, firstName, lastName, yearOfBirth);
		AppInfo.Profiles.Add(newProfile);
		AppInfo.CurrentProfileId = newProfile.Id;

		FileManager.SaveAllProfiles(AppInfo.Profiles, profilePath);
		Console.WriteLine($"Создан профиль: {newProfile.GetInfo()}");
		LoadUserTodos();
        Console.WriteLine($"Загружено задач: {AppInfo.Todos.Count}");
        Console.WriteLine("Нажмите любую клавишу для продолжения...");
        Console.ReadKey();
	}
	static bool IsLoginUnique(string login)
	{
		return !AppInfo.Profiles.Any(p =>
			p.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
	}

	static void LoadUserTodos()
	{
		if (!AppInfo.CurrentProfileId.HasValue) return;
		string todosPath = Path.Combine("data", $"todos_{AppInfo.CurrentProfileId}.csv");
		var todoList = FileManager.LoadTodosForUser(todosPath);
		if (todoList != null)
            {
				todoList.OnTodoAdded += FileManager.SaveTodoList;
				todoList.OnTodoDeleted += FileManager.SaveTodoList;
				todoList.OnTodoUpdated += (item) => FileManager.SaveTodoList(todoList);
				todoList.OnTodoChanged += FileManager.SaveTodoList;
                AppInfo.UserTodos[AppInfo.CurrentProfileId.Value] = todoList;
            }
            else
            {
				var newTodoList = new TodoList();
				newTodoList.OnTodoAdded += FileManager.SaveTodoList;
				newTodoList.OnTodoDeleted += FileManager.SaveTodoList;
				newTodoList.OnTodoUpdated += (item) => FileManager.SaveTodoList(newTodoList);
				newTodoList.OnTodoChanged += FileManager.SaveTodoList;
                AppInfo.UserTodos[AppInfo.CurrentProfileId.Value] = new TodoList();
            }
	}
	private static readonly HashSet<Type> NonUndoableCommandTypes = new HashSet<Type>
	{
		typeof(ExitCommand),
		typeof(HelpCommand),
		typeof(ProfileCommand),
		typeof(ViewCommand),
		typeof(ReadCommand),
		typeof(UndoCommand),
		typeof(RedoCommand)
	};
	private static bool ShouldStoreInUndoStack(BaseCommand command)
	{
		return !NonUndoableCommandTypes.Contains(command.GetType());
	}

	static bool ExecuteAndStoreCommand(BaseCommand command)
	{
		if (command == null)
			return false;

		command.Execute();

		if (ShouldStoreInUndoStack(command) && command.CurrentProfileId.HasValue)
		{
			string todosPath = Path.Combine("data", $"todos_{command.CurrentProfileId}.csv");
			var todoList = AppInfo.UserTodos[command.CurrentProfileId.Value];
			FileManager.SaveTodosForUser(todoList, todosPath);
		}

		if (ShouldStoreInUndoStack(command))
		{
			AppInfo.UndoStack.Push(command);
			AppInfo.RedoStack.Clear();
		}

		return true;
	}
	public static void ReturnToMainMenu()
	{
    	Console.Write("\nВойти в существующий профиль? [y/n]: ");
    	string choice = Console.ReadLine()?.Trim().ToLower();

    	if (choice == "y")
    	{
        	string profilePath = Path.Combine("data", "profile.csv");
        	LoginUser(profilePath);
    	}
    	else
    	{
        	string profilePath = Path.Combine("data", "profile.csv");
        	CreateNewProfile(profilePath);
    	}
	    LoadUserTodos();
    	Console.WriteLine($"Загружено задач: {AppInfo.Todos.Count}");
    	AppInfo.ResetUndoRedo();
    	Console.WriteLine("Введите 'help' для списка команд");
	}
}
