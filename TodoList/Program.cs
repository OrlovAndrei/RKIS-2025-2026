using TodoApp.Commands;
using TodoApp;
using TodoApp.Exceptions;
using System.Security.Cryptography;

internal class Program
{
	private static IDataStorage _storage;
	private static CommandParser _commandParser;
	private static ApiDataStorage _apiStorage;
	private static void Main(string[] args)
	{
		ArgumentNullException.ThrowIfNull(args);
		byte[] key = Convert.FromBase64String("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA="); 
		byte[] iv = Convert.FromBase64String("AAAAAAAAAAAAAAAAAAAAAA=="); 
		_storage = new FileManager("data", key, iv);
		_commandParser = new CommandParser(_storage);
		AppInfo.Profiles = _storage.LoadProfiles().ToList();
		RunApplicationLoop();
	}
	public static void ShowProfileSelection()
	{
		Console.Write("Войти в существующий профиль? [y/n]: ");
		string choice = Console.ReadLine()?.Trim().ToLower() ?? "";
		if (choice == "y")
			LoginUser();
		else
			CreateNewProfile();
	}

	private static void RunApplicationLoop()
	{
		while (true)
		{
			ShowProfileSelection();
			if (AppInfo.CurrentProfile != null)
				RunMainLoop();
		}
	}

	private static void RunMainLoop()
	{
		AppInfo.ResetUndoRedo();
		Console.Clear();
		Console.WriteLine($"Добро пожаловать, {AppInfo.CurrentProfile?.FirstName}!");
		Console.WriteLine("Введите 'help' для списка команд");
		Console.WriteLine("Введите 'profile --out' для выхода из профиля\n");

		while (true)
		{
			try
			{
				Console.Write("> ");
				string commandInput = Console.ReadLine() ?? "";
				if (string.IsNullOrWhiteSpace(commandInput))
					continue;

				string commandName = commandInput.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0].ToLower();
				string[] authRequiredCommands = { "add", "delete", "read", "status", "view", "update", "search", "undo", "redo" };
				if (authRequiredCommands.Contains(commandName) && AppInfo.CurrentProfileId == null)
					throw new AuthenticationException("Необходимо авторизоваться для работы с задачами.");

				BaseCommand command = _commandParser.Parse(commandInput, AppInfo.CurrentProfileId, AppInfo.Todos);
				ExecuteAndStoreCommand(command);

				if (AppInfo.CurrentProfile == null)
					break;
			}
			catch (TaskNotFoundException ex) { Console.WriteLine($"Ошибка задачи: {ex.Message}"); }
			catch (AuthenticationException ex) { Console.WriteLine($"Ошибка авторизации: {ex.Message}"); }
			catch (InvalidCommandException ex) { Console.WriteLine($"Ошибка команды: {ex.Message}"); }
			catch (InvalidArgumentException ex) { Console.WriteLine($"Ошибка аргументов: {ex.Message}"); }
			catch (DuplicateLoginException ex) { Console.WriteLine($"Ошибка регистрации: {ex.Message}"); }
			catch (DataStorageException ex) { Console.WriteLine($"Ошибка работы с данными: {ex.Message}"); }
			catch (Exception ex) { Console.WriteLine($"Неожиданная ошибка: {ex.Message}"); }
		}
	}

	private static void LoginUser()
	{
		try
		{
			Console.Write("Логин: ");
			string login = Console.ReadLine()?.Trim() ?? "";
			if (string.IsNullOrWhiteSpace(login))
				throw new InvalidArgumentException("Логин не может быть пустым.");

			Console.Write("Пароль: ");
			string password = Console.ReadLine()?.Trim() ?? "";
			if (string.IsNullOrWhiteSpace(password))
				throw new InvalidArgumentException("Пароль не может быть пустым.");

			var profile = AppInfo.Profiles.FirstOrDefault(p => p.Login == login && p.Password == password);
			if (profile == null)
				throw new AuthenticationException("Неверный логин или пароль.");

			AppInfo.CurrentProfileId = profile.Id;
			Console.WriteLine($"Успешно вошли: {profile.GetInfo()}");
			LoadUserTodos();
			Console.WriteLine($"Загружено задач: {AppInfo.Todos.Count}");
			AppInfo.ResetUndoRedo();
			Console.WriteLine("TodoApp с системой Undo/Redo");
			Console.WriteLine("Введите 'help' для списка команд");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка при входе в профиль: {ex.Message}");
		}
	}

	private static void CreateNewProfile()
	{
		try
		{
			Console.Write("Логин: ");
			string login = Console.ReadLine()?.Trim() ?? "";
			if (string.IsNullOrWhiteSpace(login))
				throw new InvalidArgumentException("Логин не может быть пустым.");
			if (!IsLoginUnique(login))
				throw new DuplicateLoginException($"Логин '{login}' уже существует.");

			Console.Write("Пароль: ");
			string password = Console.ReadLine()?.Trim() ?? "";
			if (string.IsNullOrWhiteSpace(password))
				throw new InvalidArgumentException("Пароль не может быть пустым.");

			Console.Write("Имя: ");
			string firstName = Console.ReadLine()?.Trim() ?? "";
			Console.Write("Фамилия: ");
			string lastName = Console.ReadLine()?.Trim() ?? "";
			Console.Write("Год рождения: ");
			if (!int.TryParse(Console.ReadLine(), out int yearOfBirth))
				throw new InvalidArgumentException("Некорректный год рождения.");

			var newProfile = new Profile(login, password, firstName, lastName, yearOfBirth);
			AppInfo.Profiles.Add(newProfile);
			AppInfo.CurrentProfileId = newProfile.Id;

			_storage.SaveProfiles(AppInfo.Profiles);

			Console.WriteLine($"Создан профиль: {newProfile.GetInfo()}");
			LoadUserTodos();
			Console.WriteLine($"Загружено задач: {AppInfo.Todos.Count}");
			Console.WriteLine("Нажмите любую клавишу для продолжения...");
			Console.ReadKey();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка при создании профиля: {ex.Message}");
		}
	}

	private static bool IsLoginUnique(string login) =>
		!AppInfo.Profiles.Any(p => p.Login.Equals(login, StringComparison.OrdinalIgnoreCase));

	private static void LoadUserTodos()
	{
		if (!AppInfo.CurrentProfileId.HasValue) return;
		var userId = AppInfo.CurrentProfileId.Value;

		var todos = _storage.LoadTodos(userId).ToList();
		var todoList = todos.Any() ? new TodoList(todos) : new TodoList(new List<TodoItem>());

		todoList.OnTodoAdded += (item) => _storage.SaveTodos(userId, todoList);
		todoList.OnTodoDeleted += (item) => _storage.SaveTodos(userId, todoList);
		todoList.OnTodoUpdated += (item) => _storage.SaveTodos(userId, todoList);
		todoList.OnStatusChanged += (item) => _storage.SaveTodos(userId, todoList);

		AppInfo.UserTodos[userId] = todoList;
	}

	private static bool ExecuteAndStoreCommand(BaseCommand command)
	{
		try
		{
			if (command == null) return false;
			if (command is UndoCommand && AppInfo.UndoStack.Count == 0)
				throw new InvalidCommandException("Нет действий для отмены (undo).");
			if (command is RedoCommand && AppInfo.RedoStack.Count == 0)
				throw new InvalidCommandException("Нет действий для возврата (redo).");

			command.Execute();

			if (command.CurrentProfileId.HasValue)
				_storage.SaveTodos(command.CurrentProfileId.Value, AppInfo.Todos);

			if (command is IUndo undoCommand)
			{
				AppInfo.UndoStack.Push(undoCommand);
				AppInfo.RedoStack.Clear();
			}
			return true;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка при выполнении команды: {ex.Message}");
			return false;
		}
	}

	public static void ReturnToMainMenu()
	{
		Console.Write("\nВойти в существующий профиль? [y/n]: ");
		string choice = Console.ReadLine()?.Trim().ToLower() ?? "";
		if (choice == "y")
			LoginUser();
		else
			CreateNewProfile();

		LoadUserTodos();
		Console.WriteLine($"Загружено задач: {AppInfo.Todos.Count}");
		AppInfo.ResetUndoRedo();
		Console.WriteLine("Введите 'help' для списка команд");
	}
}