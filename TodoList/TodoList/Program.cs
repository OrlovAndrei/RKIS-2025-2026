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
				ICommand command = CommandParser.Parse(userCommand, todos, userProfile);
				if (command != null)
				{
					command.Execute();

					if (command is AddCommand || command is MarkDoneCommand ||
						command is DeleteCommand || command is UpdateCommand)
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