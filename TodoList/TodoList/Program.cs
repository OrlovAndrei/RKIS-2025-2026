using System;
using System.IO;
using System.Linq;
using TodoApp.Exceptions;
using TodoApp.Exceptions;
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
				try
				{
					currentUser = LoginToProfile();
				}
				catch (AuthenticationException ex)
				{
					Console.WriteLine($"Ошибка входа: {ex.Message}");
				}
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
		Console.WriteLine("\nВведите 'help' для списка команд.");
		while (true)
		{
			Console.Write("> ");
			string input = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(input)) continue;
			if (input.ToLower() == "exit") break;
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
			catch (TaskNotFoundException ex)
			{
				Console.WriteLine($"Ошибка задачи: {ex.Message}");
			}
			catch (InvalidCommandException ex)
			{
				Console.WriteLine($"Ошибка команды: {ex.Message}");
			}
			catch (InvalidArgumentException ex)
			{
				Console.WriteLine($"Ошибка аргумента: {ex.Message}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Произошла непредвиденная ошибка: {ex.Message}");
			}
			if (AppInfo.ShouldLogout) break;
		}
		FileManager.SaveUserTodos(AppInfo.CurrentProfileId, AppInfo.CurrentUserTodos, AppInfo.DataDir);
		Console.WriteLine("Изменения сохранены. До свидания!");
	}
	private static Profile LoginToProfile()
	{
		Console.Write("Логин: ");
		string login = Console.ReadLine();
		Console.Write("Пароль: ");
		string password = Console.ReadLine();

		foreach (var profile in AppInfo.Profiles)
		{
			if (profile.Login == login && profile.Password == password)
			{
				Console.WriteLine($"Вход выполнен. Привет, {profile.FirstName}!");
				return profile;
			}
		}
		throw new AuthenticationException("Неверный логин или пароль.");
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
		if (!int.TryParse(Console.ReadLine(), out int birthYear))
		{
			birthYear = DateTime.Now.Year - 20;
		}
		Profile newProfile = new Profile(login, password, firstName, lastName, birthYear);
		AppInfo.Profiles.Add(newProfile);
		FileManager.SaveProfile(newProfile, profilesFilePath);
		Console.WriteLine("Профиль успешно создан!");
		return newProfile;
	}
}