using System;
using System.IO;
using System.Linq;
using TodoList.Commands;
using TodoList.Exceptions;

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

			if (choice == "profile")
			{
				Console.WriteLine("Вы не вошли в систему");
				return;
			}

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
			try
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
					throw new AuthenticationException("Неверный логин или пароль.");
				}
			}
			catch (AuthenticationException ex)
			{
				Console.WriteLine($"Ошибка авторизации: {ex.Message}");
			}
		}

		private static void RegisterUser()
		{
			try
			{
				Console.Write("Введите логин: ");
				string login = Console.ReadLine() ?? "";
				
				if (AppInfo.AllProfiles.Any(p => p.Login == login))
				{
					throw new DuplicateLoginException(login);
				}
				
				Console.Write("Введите пароль: ");
				string password = Console.ReadLine() ?? "";
				Console.Write("Введите имя: ");
				string firstName = Console.ReadLine() ?? "";
				Console.Write("Введите фамилию: ");
				string lastName = Console.ReadLine() ?? "";
				Console.Write("Введите год рождения: ");
				
				if (!int.TryParse(Console.ReadLine(), out int birthYear))
				{
					throw new InvalidArgumentException("Некорректный год рождения.");
				}

				var newProfile = new Profile(Guid.NewGuid(), login, password, firstName, lastName, birthYear);
				AppInfo.AllProfiles.Add(newProfile);
				FileManager.SaveProfiles(AppInfo.AllProfiles, profilePath);

				AppInfo.CurrentProfileId = newProfile.Id;
				Console.WriteLine("Регистрация прошла успешно!");
				LoadCurrentUserTasks();
			}
			catch (DuplicateLoginException ex)
			{
				Console.WriteLine($"Ошибка регистрации: {ex.Message}");
			}
			catch (InvalidArgumentException ex)
			{
				Console.WriteLine($"Ошибка ввода: {ex.Message}");
			}
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

				try
				{
					ICommand? command = CommandParser.Parse(inputLine);

					if (command != null)
					{
						command.Execute();

						if (command is IUndo)
						{
							AppInfo.undoStack.Push(command);
							AppInfo.redoStack.Clear();
						}
					}
					else
					{
						throw new InvalidCommandException($"Неизвестная команда: {inputLine}");
					}
				}
				catch (InvalidCommandException ex)
				{
					Console.WriteLine($"Ошибка: {ex.Message}. Введите 'help' для списка команд.");
				}
				catch (TaskNotFoundException ex)
				{
					Console.WriteLine($"Ошибка задачи: {ex.Message}");
				}
				catch (InvalidArgumentException ex)
				{
					Console.WriteLine($"Ошибка аргументов: {ex.Message}");
				}
				catch (AuthenticationException ex)
				{
					Console.WriteLine($"Ошибка авторизации: {ex.Message}");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
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