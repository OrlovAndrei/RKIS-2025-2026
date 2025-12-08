using System;
using System.IO;
using TodoList.Commands;

namespace TodoList
{
	internal class Program
	{
		private static string dataDir = "data";
		private static string profilePath = Path.Combine(dataDir, "profiles.csv");

		static void Main(string[] args)
		{
			Console.WriteLine("Работу выполнили Шелепов и Кузьменко");

			FileManager.EnsureDataDirectory(dataDir);
			AppInfo.AllProfiles = FileManager.LoadProfiles(profilePath);

			while (true)
			{
				if (AppInfo.CurrentProfileId == null)
				{
					HandleLogoutState();
				}
				else
				{
					HandleLoginState();
				}
			}
		}

		private static void HandleLogoutState()
		{
			Console.WriteLine("\nВойти в существующий профиль? [y/n] (для выхода введите 'exit')");
			string? choice = Console.ReadLine()?.ToLower();

			switch (choice)
			{
				case "y":
					LoginUser();
					break;
				case "n":
					RegisterUser();
					break;
				case "exit":
					Environment.Exit(0);
					break;
				default:
					Console.WriteLine("Неверный ввод.");
					break;
			}
		}

		private static void LoginUser()
		{
			Console.Write("Введите логин: ");
			string login = Console.ReadLine() ?? "";
			Console.Write("Введите пароль: ");
			string password = Console.ReadLine() ?? "";

			var profile = AppInfo.AllProfiles.FirstOrDefault(p => p.Login == login && p.Password == password);

			if (profile != null)
			{
				AppInfo.CurrentProfileId = profile.Id;
				Console.WriteLine($"Добро пожаловать, {profile.FirstName}!");
				LoadCurrentUserTasks();
			}
			else
			{
				Console.WriteLine("Неверный логин или пароль.");
			}
		}

		private static void RegisterUser()
		{
			Console.Write("Введите логин: ");
			string login = Console.ReadLine() ?? "";
			Console.Write("Введите пароль: ");
			string password = Console.ReadLine() ?? "";
			Console.Write("Введите имя: ");
			string firstName = Console.ReadLine() ?? "";
			Console.Write("Введите фамилию: ");
			string lastName = Console.ReadLine() ?? "";
			Console.Write("Введите год рождения: ");
			int.TryParse(Console.ReadLine(), out int birthYear);

			var newProfile = new Profile(Guid.NewGuid(), login, password, firstName, lastName, birthYear);
			AppInfo.AllProfiles.Add(newProfile);
			FileManager.SaveProfiles(AppInfo.AllProfiles, profilePath);

			AppInfo.CurrentProfileId = newProfile.Id;
			Console.WriteLine("Регистрация прошла успешно!");
			LoadCurrentUserTasks();
		}

		private static void HandleLoginState()
		{
			while (AppInfo.CurrentProfileId != null)
			{
				Console.Write("> ");
				string? inputLine = Console.ReadLine();

				if (string.IsNullOrWhiteSpace(inputLine)) continue;

				if (inputLine.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
				{
					SaveCurrentUserTasks();
					Environment.Exit(0);
					break;
				}

				ICommand? command = CommandParser.Parse(inputLine);

				if (command != null)
				{
					command.Execute();

					if (command is AddCommand || command is DeleteCommand ||
						command is UpdateCommand || command is StatusCommand)
					{
						AppInfo.undoStack.Push(command);
						AppInfo.redoStack.Clear();
					}
				}
				else
				{
					Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
				}
			}
		}

		private static void LoadCurrentUserTasks()
		{
			AppInfo.undoStack.Clear();
			AppInfo.redoStack.Clear();

			Guid userId = AppInfo.CurrentProfileId.Value;
			string userTodoPath = Path.Combine(dataDir, $"todos_{userId}.csv");

			if (!AppInfo.AllTodos.ContainsKey(userId))
			{
				var todos = FileManager.LoadTodos(userTodoPath);

				todos.TaskAdded += (task) => SaveCurrentUserTasks();
				todos.TaskDeleted += (task) => SaveCurrentUserTasks();
				todos.TaskUpdated += (task) => SaveCurrentUserTasks();
				todos.StatusChanged += (task) => SaveCurrentUserTasks();

				AppInfo.AllTodos[userId] = todos;
			}
		}

		public static void SaveCurrentUserTasks()
		{
			if (AppInfo.CurrentProfileId == null) return;
			Guid userId = AppInfo.CurrentProfileId.Value;
			string userTodoPath = Path.Combine(dataDir, $"todos_{userId}.csv");
			FileManager.SaveTodos(AppInfo.CurrentUserTodos, userTodoPath);
		}
	}
}