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
			if (userCommand?.ToLower() == "undo")
			{
				if (AppInfo.UndoStack.Count > 0)
				{
					ICommand command = AppInfo.UndoStack.Pop();
					command.Unexecute();
					AppInfo.RedoStack.Push(command);
					Console.WriteLine("Команда отменена");
				}
				else
				{
					Console.WriteLine("Нет команд для отмены");
				}
				Console.ReadKey();
				continue;
			}
			if (userCommand?.ToLower() == "redo")
			{
				if (AppInfo.RedoStack.Count > 0)
				{
					ICommand command = AppInfo.RedoStack.Pop();
					command.Execute();
					AppInfo.UndoStack.Push(command);
					Console.WriteLine("Команда повторена");
				}
				else
				{
					Console.WriteLine("Нет команд для повтора");
				}
				Console.ReadKey();
				continue;
			}
			try
			{
				ICommand command = CommandParser.Parse(userCommand, todos, userProfile, profileFilePath, todoFilePath);

				if (command != null)
				{
					command.Execute();
					AppInfo.UndoStack.Push(command);
					AppInfo.RedoStack.Clear();

					if (command is AddCommand || command is DeleteCommand ||
						command is UpdateCommand || command is StatusCommand)
					{
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
	private static Profile CreateUserProfile(string profileFilePath)
	{
		string name, surname;
		int yearOfBirth;
		Console.WriteLine("Напишите ваше имя и фамилию:");
		string fullName;
		while (string.IsNullOrEmpty(fullName = Console.ReadLine()))
		{
			Console.WriteLine("Вы ничего не ввели");
		}
		string[] splitFullName = fullName.Split(' ', 2);
		name = splitFullName[0];
		surname = splitFullName.Length > 1 ? splitFullName[1] : "";
		Console.WriteLine("Напишите свой год рождения:");
		yearOfBirth = int.Parse(Console.ReadLine());
		Profile profile = new Profile(name, surname, yearOfBirth);
		Console.WriteLine("Добавлен пользователь: " + profile.GetInfo(2025));
		FileManager.SaveProfile(profile, profileFilePath);
		return profile;
	}
}