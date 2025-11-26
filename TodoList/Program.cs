using System;
using TodoApp.Commands;
using TodoList.Commands;
namespace TodoApp;
class Program
{
	static void Main(string[] args)
	{
		string dataDir = Path.Combine(Environment.CurrentDirectory, "data");
		string profilePath = Path.Combine(dataDir, "profile.txt");
		string todoPath = Path.Combine(dataDir, "todo.txt");
		string donePath = Path.Combine(dataDir, "done.txt");

		FileManager.EnsureDataDirectory(dataDir);
		if (!File.Exists(todoPath))
			File.WriteAllText(todoPath, "", System.Text.Encoding.UTF8);
		if (!File.Exists(donePath))
			File.WriteAllText(donePath, "", System.Text.Encoding.UTF8);

		Profile userProfile = FileManager.LoadProfile(profilePath);
		if (userProfile == null)
		{
			Console.WriteLine("Введите Имя:");
			string name = Console.ReadLine();
			Console.WriteLine("Введите Фамилию:");
			string surname = Console.ReadLine();
			Console.WriteLine("Введите год рождения:");
			int yearOfBirth = Convert.ToInt32(Console.ReadLine());

			userProfile = new Profile(name, surname, yearOfBirth);
			FileManager.SaveProfile(userProfile, profilePath);
			Console.WriteLine($"Создан профиль: {userProfile.GetInfo()}");
		}
		else
		{
			Console.WriteLine($"Загружен профиль: {userProfile.GetInfo()}");
		}

		TodoList todoList = FileManager.LoadTodos(todoPath, donePath);
		if (todoList == null)
		{
			todoList = new TodoList();
			Console.WriteLine("Создан пустой список задач.");
		}
		else
		{
			Console.WriteLine($"Загружено задач: {todoList.Count}");
		}
		AppInfo.Todos = todoList;
		AppInfo.CurrentProfile = userProfile;

		Console.WriteLine("TodoApp с системой Undo/Redo");
		Console.WriteLine("Введите 'help' для списка команд");

		while (true)
		{
			try
			{
				Console.Write("> ");
				string commandInput = Console.ReadLine();

				if (string.IsNullOrWhiteSpace(commandInput)) continue;

				BaseCommand command = CommandParser.Parse(commandInput, todoList, userProfile);
				ExecuteAndStoreCommand(command, todoPath);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка: {ex.Message}");
			}
		}
	}

	static void ExecuteAndStoreCommand(BaseCommand command, string todoPath)
	{
		if (command == null) return;

		command.Execute();

		if (!(command is ExitCommand))
		{
			FileManager.SaveTodos(AppInfo.Todos, todoPath);
		}

		if (ShouldStoreInUndoStack(command))
		{
			AppInfo.UndoStack.Push(command);
			AppInfo.RedoStack.Clear();
		}
	}

	private static bool ShouldStoreInUndoStack(BaseCommand command)
	{
		return !(command is ReadCommand) &&
			   !(command is ViewCommand) &&
			   !(command is ProfileCommand) &&
			   !(command is ExitCommand) &&
			   !(command is HelpCommand) &&
			   !(command is UndoCommand) &&
			   !(command is RedoCommand);
	}
}
