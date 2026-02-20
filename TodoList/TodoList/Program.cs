using System;
using System.IO;
using System.Linq;
internal class Program
{
	private static void Main(string[] args)
	{
		Console.WriteLine("Работу выполнили: Амелина Яна и Кабанова Арина");
		AppInfo.DataDir = "Data";
		string profilesFilePath = Path.Combine(AppInfo.DataDir, "profiles.csv");
		FileManager.EnsureDataDirectory(AppInfo.DataDir);
		AppInfo.Profiles = FileManager.LoadProfiles(profilesFilePath);
		Profile currentUser = null;
		if (AppInfo.Profiles.Count > 0)
		{
			Console.Write("Войти в существующий профиль? [y/n]: ");
			string choice = Console.ReadLine()?.ToLower();
			if (choice == "y")
			{
				currentUser = LoginToProfile();
			}
			else
			{
				currentUser = CreateNewProfile(profilesFilePath);
			}
		}
		else
		{
			Console.WriteLine("Профили не найдены. Создайте новый профиль.");
			currentUser = CreateNewProfile(profilesFilePath);
		}
		if (currentUser == null)
		{
			Console.WriteLine("Вход не выполнен. Программа завершается.");
			return;
		}
		AppInfo.CurrentProfileId = currentUser.Id;
		AppInfo.CurrentUserTodos = FileManager.LoadUserTodos(AppInfo.CurrentProfileId, AppInfo.DataDir);
		AppInfo.CurrentUserTodos.OnTodoAdded += SaveTodoList;
		AppInfo.CurrentUserTodos.OnTodoDeleted += SaveTodoList;
		AppInfo.CurrentUserTodos.OnTodoUpdated += SaveTodoList;
		AppInfo.CurrentUserTodos.OnStatusChanged += SaveTodoList;
		while (!AppInfo.ShouldLogout)
		{
			Console.Write("\nВведите команду: ");
			string input = Console.ReadLine();
			try
			{
				ICommand command = CommandParser.Parse(input, AppInfo.CurrentUserTodos, currentUser, profilesFilePath, AppInfo.DataDir);
				if (command != null)
				{
					command.Execute();
					if (command is IUndo)
					{
						AppInfo.UndoStack.Push(command);
						AppInfo.RedoStack.Clear();
					}
				}
			}
			catch (ArgumentException ex)
			{
				Console.WriteLine($"Ошибка ввода: {ex.Message}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Произошла системная ошибка: {ex.Message}");
			}
		}
	}
	private static void SaveTodoList(TodoItem item)
	{
		FileManager.SaveUserTodos(AppInfo.CurrentProfileId, AppInfo.CurrentUserTodos, AppInfo.DataDir);
	}
	private static Profile LoginToProfile()
	{
		Console.Write("Введите логин: ");
		string login = Console.ReadLine()?.Trim();
		Console.Write("Введите пароль: ");
		string password = Console.ReadLine();
		if (string.IsNullOrEmpty(login)) return null;
		foreach (var profile in AppInfo.Profiles)
		{
			if (profile.Login == login && profile.Password == password)
			{
				Console.WriteLine($"Вход выполнен. Привет, {profile.FirstName}!");
				return profile;
			}
		}
		Console.WriteLine("Неверный логин или пароль.");
		return null;
	}
	private static Profile CreateNewProfile(string profilesFilePath)
	{
		Console.WriteLine("\n=== СОЗДАНИЕ НОВОГО ПРОФИЛЯ ===");
		Console.Write("Логин: ");
		string login = Console.ReadLine()?.Trim();

		if (string.IsNullOrEmpty(login))
		{
			Console.WriteLine("Ошибка: Логин не может быть пустым.");
			return null;
		}
		if (AppInfo.Profiles.Any(p => p.Login.Equals(login, StringComparison.OrdinalIgnoreCase)))
		{
			Console.WriteLine("Ошибка: Этот логин уже занят.");
			return null;
		}
		Console.Write("Пароль: ");
		string password = Console.ReadLine();
		Console.Write("Имя: ");
		string firstName = Console.ReadLine();
		Console.Write("Фамилия: ");
		string lastName = Console.ReadLine();
		Console.Write("Год рождения: ");
		int birthYear;
		while (!int.TryParse(Console.ReadLine(), out birthYear) || birthYear < 1900 || birthYear > DateTime.Now.Year)
		{
			Console.Write($"Введите корректный год (1900-{DateTime.Now.Year}): ");
		}
		var profile = new Profile(login, password, firstName, lastName, birthYear);
		FileManager.SaveProfile(profile, profilesFilePath);
		AppInfo.Profiles.Add(profile);
		return profile;
	}
}