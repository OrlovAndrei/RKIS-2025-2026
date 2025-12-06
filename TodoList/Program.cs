using System;
using System.IO;
using System.Text;
using TodoApp.Commands;
using TodoApp.Commands;
class Program
{
	static void Main(string[] args)
	{
		string dataDir = Path.Combine(Environment.CurrentDirectory, "data");
		string profilePath = Path.Combine(dataDir, "profile.csv");
		FileManager.EnsureDataDirectory(dataDir);
		AppInfo.Profiles = FileManager.LoadAllProfiles(profilePath);
		Console.Write("Войти в существующий профиль? [y/n]: ");
		string choice = Console.ReadLine()?.Trim().ToLower();

		if (choice == "y")
		{
			LoginUser(profilePath);
		}
		else
		{
			CreateNewProfile(profilePath);
		}
		LoadUserTodos();
		Console.WriteLine($"Загружено задач: {AppInfo.Todos.Count}");
		AppInfo.ResetUndoRedo();
		Console.WriteLine("TodoApp с системой Undo/Redo");
		Console.WriteLine("Введите 'help' для списка команд");
		while (true)
		{
			try
			{
				Console.Write("> ");
				string commandInput = Console.ReadLine();
				if (string.IsNullOrWhiteSpace(commandInput)) continue;
				BaseCommand command = CommandParser.Parse(commandInput, AppInfo.CurrentProfileId, AppInfo.Todos);
				ExecuteAndStoreCommand(command);
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
		AppInfo.UserTodos[AppInfo.CurrentProfileId.Value] = todoList ?? new TodoList();
	}
	static bool ExecuteAndStoreCommand(BaseCommand command)
	{
		if (command == null)
			return false;
		command.Execute();
		if (!(command is ExitCommand) &&
			!(command is HelpCommand) &&
			!(command is ProfileCommand) &&
			!(command is ViewCommand) &&
			!(command is ReadCommand) &&
			!(command is UndoCommand) &&
			!(command is RedoCommand))
		{
			if (command.CurrentProfileId.HasValue)
			{
				string todosPath = Path.Combine("data", $"todos_{command.CurrentProfileId}.csv");
				var todoList = AppInfo.UserTodos[command.CurrentProfileId.Value];
				FileManager.SaveTodosForUser(todoList, todosPath);
			}
		}
		if (ShouldStoreInUndoStack(command))
		{
			AppInfo.UndoStack.Push(command);
			AppInfo.RedoStack.Clear();
		}
		return true;
	}
	private static bool ShouldStoreInUndoStack(BaseCommand command)
	{
		return !(command is ExitCommand) &&
			   !(command is HelpCommand) &&
			   !(command is ProfileCommand) &&
			   !(command is ViewCommand) &&
			   !(command is ReadCommand) &&
			   !(command is UndoCommand) &&
			   !(command is RedoCommand);
	}
}
