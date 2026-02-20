using System;
using System.IO;
using System.Linq;
using TodoList.Commands;
namespace TodoList
{
	internal class Program
	{
		private const string DataDirectory = "Data";
		private const string ProfilesFileName = "profiles.csv";
		static void Main(string[] args)
		{
			Console.WriteLine("Работу выполнили Нестеренко и Горелов");
			try
			{
				FileManager.EnsureDataDirectory(DataDirectory);
				AppInfo.AllProfiles = FileManager.LoadProfiles(Path.Combine(DataDirectory, ProfilesFileName));
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Критическая ошибка при загрузке данных: {ex.Message}");
				return;
			}
			while (true)
			{
				while (AppInfo.CurrentProfile == null)
				{
					Console.Write("\nВойти в существующий профиль? [y/n] (или 'exit' для выхода): ");
					string choice = Console.ReadLine()?.ToLower().Trim();

					if (choice == "y") Login();
					else if (choice == "n") Register();
					else if (choice == "exit") return;
					else Console.WriteLine("Неверный ввод. Введите 'y', 'n' или 'exit'.");
				}
				Console.WriteLine($"\nДобро пожаловать, {AppInfo.CurrentProfile.FirstName}! Введите команду (help — справка):");
				while (AppInfo.CurrentProfile != null)
				{
					Console.Write("> ");
					string input = Console.ReadLine()?.Trim();
					if (string.IsNullOrWhiteSpace(input)) continue;
					if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
					{
						Console.WriteLine("Программа завершена.");
						return;
					}
					try
					{
						ICommand command = CommandParser.Parse(input);
						if (command != null)
						{
							command.Execute();
							if (command is AddCommand || command is DeleteCommand || command is UpdateCommand || command is StatusCommand)
							{
								AppInfo.UndoStack.Push(command);
								AppInfo.RedoStack.Clear();
							}
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine($"Произошла неожиданная ошибка: {ex.Message}");
					}
				}
			}
		}
		static void Login()
		{
			while (true)
			{
				Console.WriteLine("\n--- Вход (введите !cancel для отмены) ---");
				Console.Write("Введите логин: ");
				string login = Console.ReadLine()?.Trim();
				if (login == "!cancel") return;
				if (string.IsNullOrWhiteSpace(login))
				{
					Console.WriteLine("Логин не может быть пустым.");
					continue;
				}
				Console.Write("Введите пароль: ");
				string password = Console.ReadLine()?.Trim();
				Profile foundProfile = AppInfo.AllProfiles
					.FirstOrDefault(p => p.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
				if (foundProfile != null && foundProfile.Password == password)
				{
					InitializeSession(foundProfile);
					Console.WriteLine("Вход выполнен успешно!");
					return;
				}
				else
				{
					Console.WriteLine("Неверный логин или пароль. Попробуйте снова.");
				}
			}
		}
		static void Register()
		{
			Console.WriteLine("\n--- Регистрация (введите !cancel для отмены) ---");
			string firstName = GetValidInput("Введите имя: ");
			if (firstName == "!cancel") return;
			string lastName = GetValidInput("Введите фамилию: ");
			if (lastName == "!cancel") return;
			int birthYear;
			while (true)
			{
				Console.Write("Введите год рождения (гггг): ");
				string input = Console.ReadLine();
				if (input == "!cancel") return;
				if (int.TryParse(input, out birthYear) && birthYear >= 1900 && birthYear <= DateTime.Now.Year)
				{
					break;
				}
				Console.WriteLine($"Ошибка: введите число от 1900 до {DateTime.Now.Year}.");
			}
			string login;
			while (true)
			{
				login = GetValidInput("Введите логин: ");
				if (login == "!cancel") return;
				if (AppInfo.AllProfiles.Any(p => p.Login.Equals(login, StringComparison.OrdinalIgnoreCase)))
				{
					Console.WriteLine("Этот логин уже занят. Придумайте другой.");
				}
				else
				{
					break;
				}
			}
			string password = GetValidInput("Введите пароль: ");
			if (password == "!cancel") return;
			var newProfile = new Profile(firstName, lastName, birthYear, login, password);
			AppInfo.AllProfiles.Add(newProfile);
			FileManager.SaveProfiles(AppInfo.AllProfiles, Path.Combine(DataDirectory, ProfilesFileName));
			InitializeSession(newProfile);
			Console.WriteLine("Регистрация успешна!");
		}
		static string GetValidInput(string prompt)
		{
			while (true)
			{
				Console.Write(prompt);
				string input = Console.ReadLine()?.Trim();
				if (!string.IsNullOrWhiteSpace(input)) return input;
				Console.WriteLine("Поле не может быть пустым.");
			}
		}
		static void InitializeSession(Profile profile)
		{
			AppInfo.CurrentProfileId = profile.Id;
			string path = Path.Combine(DataDirectory, $"todos_{profile.Id}.csv");
			TodoList todos = FileManager.LoadTodos(path);
			todos.OnTodoAdded += FileManager.SaveTodoList;
			todos.OnTodoDeleted += FileManager.SaveTodoList;
			todos.OnTodoUpdated += FileManager.SaveTodoList;
			todos.OnStatusChanged += FileManager.SaveTodoList;
			AppInfo.AllTodos[profile.Id] = todos;
			AppInfo.UndoStack.Clear();
			AppInfo.RedoStack.Clear();
		}
	}
}