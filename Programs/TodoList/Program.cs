namespace Todolist;

class Program
{
	static void Main()
	{
		Console.WriteLine("Работу сделали Приходько и Бочкарёв\n");

		Console.WriteLine("Программа запускается, брбрбрбжбжбж...");

		// Директория для данных
		string dataDirectory = "data";
		string profileFilePath = Path.Combine(dataDirectory, "profile.txt");
		string todosFilePath = Path.Combine(dataDirectory, "todo.csv");

		AppInfo.ProfileFilePath = profileFilePath;
		AppInfo.TodosFilePath = todosFilePath;

		FileManager.EnsureDataDirectory(dataDirectory);
		FileManager.EnsureFilesExist(profileFilePath, todosFilePath);

		AppInfo.LoadFromFiles(profileFilePath, todosFilePath);

		// Проверочка профиля
		if (string.IsNullOrEmpty(AppInfo.CurrentProfile.FirstName) || AppInfo.CurrentProfile.BirthYear == 0)
		{
			InitializeProfile();
		}
		else
		{
			Console.WriteLine($"Авторизован пользователь: {AppInfo.CurrentProfile.GetInfo()}");
		}

		Console.WriteLine("Добро пожаловать в программу");
		Console.WriteLine("Введите 'help' для списка команд");

		while (true)
		{
			Console.WriteLine("=-=-=-=-=-=-=-=");
			string input = Console.ReadLine();

			if (string.IsNullOrWhiteSpace(input))
				continue;

			ICommand command = CommandParser.Parse(
				input,
				AppInfo.CurrentProfile,
				todosFilePath,
				profileFilePath
			);


			if (command != null)
			{
				command.Execute();

				if (command is not ViewCommand &&
					command is not ReadCommand &&
					command is not HelpCommand &&
					command is not ProfileCommand &&
					command is not ExitCommand &&
					command is not UndoCommand &&
					command is not RedoCommand)
				{
					AppInfo.UndoStack.Push(command);
					AppInfo.RedoStack.Clear();
				}

				FileManager.SaveProfile(AppInfo.CurrentProfile, profileFilePath);
				FileManager.SaveTodos(AppInfo.Todos, todosFilePath);

				// Чисто ради проверки
				Console.WriteLine($" В Undo стеке сейчас: {AppInfo.UndoStack.Count} команд");
			}
			else
			{
				Console.WriteLine($"Неизвестная команда: {input.Split(' ')[0]}");
			}
		}
	}

	static void InitializeProfile()
	{
		bool isValid = true;
		int currentYear = DateTime.Now.Year;

		Console.Write("Введите свое имя: ");
		AppInfo.CurrentProfile.FirstName = Console.ReadLine();

		Console.Write("Введите свою фамилию: ");
		AppInfo.CurrentProfile.LastName = Console.ReadLine();

		Console.Write("Введите свой год рождения: ");

		try
		{
			AppInfo.CurrentProfile.BirthYear = int.Parse(Console.ReadLine());
		}
		catch (Exception)
		{
			isValid = false;
		}

		if (isValid && (AppInfo.CurrentProfile.BirthYear <= currentYear))
		{
			Console.WriteLine($"Добавлен пользователь: {AppInfo.CurrentProfile.GetInfo()}");
		}
		else
		{
			Console.WriteLine("Неверно введен год рождения. Установлен год по умолчанию.");
			AppInfo.CurrentProfile.BirthYear = DateTime.Now.Year;
		}
	}
}