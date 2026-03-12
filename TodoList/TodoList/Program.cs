using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TodoApp.Exceptions;
public class Program
{
	private static void Main(string[] args)
	{
		Console.WriteLine("Работу выполнили: Амелина Яна и Кабанова Арина");
		Console.OutputEncoding = System.Text.Encoding.UTF8;
		string profilesFilePath = "data/profiles.json.enc";
		string todosDirectoryPath = "data/todos";
		IDataStorage dataStorage = new FileManager(
			profilesFilePath,
			todosDirectoryPath,
			EncryptionSettings.Key,
			EncryptionSettings.IV
		);
		AppInfo.Initialize(dataStorage);
		try
		{
			AppInfo.LoadData();
		}
		catch (StorageException ex)
		{
			Console.WriteLine($"Ошибка хранилища: {ex.Message}");
			return;
		}
		Profile currentProfile = null;
		if (AppInfo.Profiles.Count > 0)
		{
			Console.Write("Войти в существующий профиль? [y/n]: ");
			string choice = Console.ReadLine()?.ToLower();
			if (choice == "y")
			{
				try
				{
					currentProfile = LoginToProfile();
				}
				catch (AuthenticationException ex)
				{
					Console.WriteLine($"Ошибка входа: {ex.Message}");
				}
			}
			else
			{
				currentProfile = CreateNewProfile(profilesFilePath);
			}
		}
		else
		{
			Console.WriteLine("Профили не найдены. Создайте новый профиль.");
			currentProfile = CreateNewProfile(profilesFilePath);
		}
		if (currentProfile == null)
		{
			Console.WriteLine("Вход не выполнен. Программа завершается.");
			return;
		}
		AppInfo.CurrentProfileId = currentProfile.Id;
		Console.WriteLine($"Добро пожаловать, {currentProfile.FirstName}!");
		Console.WriteLine("\nВведите 'help' для списка команд.");
		while (true)
		{
			Console.Write("> ");
			var input = Console.ReadLine();

			if (string.IsNullOrWhiteSpace(input)) continue;

			if (input.ToLower() == "exit")
			{
				AppInfo.SaveData();
				Console.WriteLine("До свидания!");
				break;
			}

			try
			{
				ICommand command = CommandParser.Parse(input);
				if (command != null)
				{
					command.Execute();
					AppInfo.SaveData();
				}
			}
			catch (StorageException e)
			{
				Console.WriteLine($"Ошибка записи данных: {e.Message}");
			}
			catch (InvalidArgumentException e)
			{
				Console.WriteLine($"Ошибка: {e.Message}");
			}
			catch (Exception e)
			{
				Console.WriteLine($"Произошла ошибка: {e.Message}");
			}
		}
	}
	private static Profile LoginToProfile()
	{
		Console.Write("Введите логин: ");
		string login = Console.ReadLine();
		Console.Write("Введите пароль: ");
		string password = Console.ReadLine();
		var profile = AppInfo.Profiles.FirstOrDefault(p => p.Login == login && p.Password == password);
		if (profile == null)
		{
			throw new AuthenticationException("Неверный логин или пароль");
		}
		return profile;
	}
	private static Profile CreateNewProfile(string profilesFilePath)
	{
		Console.Write("Введите имя: ");
		string firstName = Console.ReadLine();
		Console.Write("Введите фамилию: ");
		string lastName = Console.ReadLine();
		int birthYear;
		while (true)
		{
			Console.Write("Введите год рождения: ");
			string input = Console.ReadLine();

			if (int.TryParse(input, out birthYear))
			{
				int currentYear = DateTime.Now.Year;
				int age = currentYear - birthYear;

				if (age >= 15 && age <= 100)
				{
					break;
				}
				else if (age < 15)
				{
					Console.WriteLine("Какой чудесный на улице день, солнышко светит, птички поют, таким маленьким детям как ты ВХОД ЗАПРЕЩЕН");
					Console.WriteLine("Программа завершается.");
					Environment.Exit(0);
				}
				else
				{
					Console.WriteLine("Пожалуйста, введите корректный год рождения (возраст от 15 лет).");
				}
			}
			else
			{
				Console.WriteLine("Пожалуйста, введите корректный год рождения (число).");
			}
		}
		Console.Write("Придумайте логин: ");
		string login = Console.ReadLine();
		while (AppInfo.Profiles.Any(p => p.Login == login))
		{
			Console.WriteLine("Этот логин уже занят. Пожалуйста, придумайте другой.");
			Console.Write("Придумайте логин: ");
			login = Console.ReadLine();
		}
		Console.Write("Придумайте пароль: ");
		string password = Console.ReadLine();
		var newProfile = new Profile(login, password, firstName, lastName, birthYear);
		AppInfo.Profiles.Add(newProfile);
		AppInfo.SaveData();
		Console.WriteLine("Профиль успешно создан!");
		return newProfile;
	}
}