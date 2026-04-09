using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Commands;
using TodoList.Exceptions;
using TodoList.Interfaces;
using TodoList.Data;
using TodoList.Models;
using TodoList.Services;

namespace TodoList
{
	internal class Program
	{
		private static string dataDir = "data";
		private static IDataStorage _storage = null!;
		private static bool _useApiStorage = false;
		
		private static IProfileRepository _profileRepository = null!;
		private static ITodoRepository _todoRepository = null!;
		private static bool _useDatabase = false;

		static void Main(string[] args)
		{
			Console.WriteLine("Работу выполнили Шелепов и Кузьменко");
			Console.WriteLine("Выберите режим хранения данных:");
			Console.WriteLine("1 - Локальное файловое хранилище (FileStorage)");
			Console.WriteLine("2 - Удаленное API-хранилище (ApiDataStorage)");
			Console.WriteLine("3 - База данных SQLite (Entity Framework Core)");
			Console.Write("Ваш выбор: ");
			
			string? choice = Console.ReadLine();
			
			if (choice == "2")
			{
				_useApiStorage = true;
				_useDatabase = false;
				Console.WriteLine("Выбрано API-хранилище");
			}
			else if (choice == "3")
			{
				_useApiStorage = false;
				_useDatabase = true;
				Console.WriteLine("Выбрана база данных SQLite");
			}
			else
			{
				_useApiStorage = false;
				_useDatabase = false;
				Console.WriteLine("Выбрано локальное файловое хранилище");
			}

			if (_useDatabase)
			{
				_profileRepository = new ProfileRepository();
				_todoRepository = new TodoRepository();
				
				var profiles = _profileRepository.GetAllAsync().Result;
				AppInfo.AllProfiles = profiles.ToList();
			}
			else
			{
				EnsureDataDirectory(dataDir);

				if (_useApiStorage)
				{
					_storage = new ApiDataStorage();
				}
				else
				{
					_storage = new FileStorage(dataDir);
				}
				
				AppInfo.Storage = _storage;
				AppInfo.AllProfiles = _storage.LoadProfiles().ToList();
			}

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

		private static void EnsureDataDirectory(string dirPath)
		{
			if (!Directory.Exists(dirPath))
				Directory.CreateDirectory(dirPath);
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

				Profile? profile = null;
				
				if (_useDatabase)
				{
					profile = _profileRepository.GetByLoginAsync(login).Result;
					if (profile != null && profile.Password != password)
						profile = null;
				}
				else
				{
					profile = AppInfo.AllProfiles.FirstOrDefault(p => p.Login == login && p.Password == password);
				}

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
				
				bool loginExists = false;
				
				if (_useDatabase)
				{
					var existing = _profileRepository.GetByLoginAsync(login).Result;
					loginExists = existing != null;
				}
				else
				{
					loginExists = AppInfo.AllProfiles.Any(p => p.Login == login);
				}
				
				if (loginExists)
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
				
				if (_useDatabase)
				{
					_profileRepository.AddAsync(newProfile).Wait();
					AppInfo.AllProfiles = _profileRepository.GetAllAsync().Result.ToList();
				}
				else
				{
					AppInfo.AllProfiles.Add(newProfile);
					AppInfo.Storage.SaveProfiles(AppInfo.AllProfiles);
				}

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
					if (!_useDatabase)
						SaveCurrentUserTasks();
					Environment.Exit(0);
					break;
				}

				try
				{
					ICommand? command = CommandParser.Parse(inputLine);

					if (command != null)
					{
						if (command is IRepositoryCommand repoCommand && _useDatabase)
						{
							repoCommand.SetRepositories(_profileRepository, _todoRepository);
						}
						
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
				catch (InvalidOperationException ex)
				{
					Console.WriteLine($"Ошибка синхронизации: {ex.Message}");
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

			if (AppInfo.CurrentProfileId == null) return;
			
			Guid userId = AppInfo.CurrentProfileId.Value;

			if (!AppInfo.AllTodos.ContainsKey(userId))
			{
				if (_useDatabase)
				{
					var tasksList = _todoRepository.GetAllAsync(userId).Result.ToList();
					var todos = new TodoList(tasksList);
					AppInfo.AllTodos[userId] = todos;
				}
				else
				{
					var todosList = AppInfo.Storage.LoadTodos(userId).ToList();
					var todos = new TodoList(todosList);

					todos.TaskAdded += (task) => SaveCurrentUserTasks();
					todos.TaskDeleted += (task) => SaveCurrentUserTasks();
					todos.TaskUpdated += (task) => SaveCurrentUserTasks();
					todos.StatusChanged += (task) => SaveCurrentUserTasks();

					AppInfo.AllTodos[userId] = todos;
				}
			}
		}

		public static void SaveCurrentUserTasks()
		{
			if (_useDatabase) return;
			
			if (AppInfo.CurrentProfileId == null) return;
			Guid userId = AppInfo.CurrentProfileId.Value;
			
			var todos = AppInfo.CurrentUserTodos;
			if (todos != null)
			{
				AppInfo.Storage.SaveTodos(userId, todos.GetAllTasks());
			}
		}
		
		public static IProfileRepository GetProfileRepository() => _profileRepository;
		public static ITodoRepository GetTodoRepository() => _todoRepository;
		public static bool UseDatabase => _useDatabase;
	}
}