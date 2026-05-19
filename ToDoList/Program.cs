using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TodoList.Commands;
using TodoList.Exceptions;
using TodoList.Interfaces;
using ToDoList;

namespace TodoList
{
	class Program
	{
		static Profile userProfile;
		static IDataStorage storage;
		static ApiDataStorage apiStorage;
		static SyncCommand syncCommand;

		static void Main(string[] args)
		{
			Console.WriteLine("Работу выполнил Фучаджи");

			string key = "12345678901234567890123456789012";
			string iv = "1234567890123456";

			var localFileManager = new FileManager(key, iv);
			storage = localFileManager;
			AppInfo.Storage = storage;

			apiStorage = new ApiDataStorage(localFileManager);
			syncCommand = new SyncCommand(apiStorage);

			LoadData();
			Console.WriteLine("Введите help для вывода доступных команд");

			while (true)
			{
				try
				{
					Console.Write("> ");
					string input = Console.ReadLine();

					if (string.IsNullOrWhiteSpace(input)) continue;

					string[] parts = input.Split(' ');
					string commandName = parts[0].ToLower();

					if (commandName == "sync")
					{
						syncCommand.Execute(parts);
						continue;
					}

					var command = CommandParser.Parse(input);

					if (command != null)
					{
						command.Execute();

						if (command is IUndo)
						{
							AppInfo.undoStack.Push(command);
						}

						SaveData();
					}
				}
				catch (InvalidCommandException ex)
				{
					Console.WriteLine("Ошибка команды: " + ex.Message);
				}
				catch (Exception ex)
				{
					Console.WriteLine("Критическая ошибка: " + ex.Message);
				}
			}
		}

		static void CreateUser()
		{
			if (userProfile != null)
			{
				throw new DuplicateLoginException("Профиль уже существует.");
			}

			Console.Write("Введите имя: ");
			string firstName = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(firstName))
			{
				throw new InvalidArgumentException("Имя не может быть пустым.");
			}

			Console.Write("Введите фамилию: ");
			string lastName = Console.ReadLine();

			Console.Write("Введите год рождения: ");
			string yearInput = Console.ReadLine();
			if (!int.TryParse(yearInput, out int birthYear) || birthYear < 1900 || birthYear > DateTime.Now.Year)
			{
				throw new InvalidArgumentException("Некорректный год рождения.");
			}

			userProfile = new Profile(firstName, lastName, birthYear);
			AppInfo.CurrentProfile = userProfile;
			SaveData();
			Console.WriteLine("Профиль успешно создан.");
		}

		static void SaveData()
		{
			if (userProfile != null)
			{
				storage.SaveProfiles(new List<Profile> { userProfile });
				storage.SaveTodos(userProfile.Id, AppInfo.Todos);
			}
		}

		static void LoadData()
		{
			if (AppInfo.Todos == null)
				AppInfo.Todos = new TodoList();

			var profiles = storage.LoadProfiles();
			userProfile = profiles.FirstOrDefault();

			if (userProfile == null)
			{
				CreateUser();
			}
			else
			{
				AppInfo.CurrentProfile = userProfile;
				Console.WriteLine($"Загружен профиль: {userProfile.GetInfo()}");

				var loadedTodos = storage.LoadTodos(userProfile.Id);
				foreach (var todo in loadedTodos)
				{
					AppInfo.Todos.Add(todo);
				}
			}
		}
	}
}