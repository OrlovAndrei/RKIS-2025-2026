using System;
using System.IO;
using TodoList.Commands;
using ToDoList;

namespace TodoList
{
	class Program
	{
		static Profile userProfile;
		static string dataDir = Path.Combine(Directory.GetCurrentDirectory(), "data");
		static string profileFilePath = Path.Combine(dataDir, "profile.txt");
		static string todoFilePath = Path.Combine(dataDir, "todo.csv");

		static void Main(string[] args)
		{
			Console.WriteLine("Работу выполнили Фучаджи и Клюев");

			FileManager.EnsureDataDirectory(dataDir);
			LoadData();
			Console.WriteLine("Введите help для вывода доступных команд");

			while (true)
			{
				Console.Write("Введите команду: ");
				string input = Console.ReadLine()?.Trim();

				if (string.IsNullOrEmpty(input) || input.Equals("exit", StringComparison.OrdinalIgnoreCase))
				{
					break;
				}

				ICommand command = CommandParser.Parse(input);
				if (command != null)
				{
					command.Execute();

					if (command is IUndo)
					{
						AppInfo.undoStack.Push(command);
					}
				}

				SaveData();
			}
		}

		static void CreateUser()
		{
			Console.Write("Введите ваше имя: ");
			string firstName = Console.ReadLine();
			if (string.IsNullOrEmpty(firstName))
			{
				Console.WriteLine("Имя не может быть пустым");
				return;
			}
			Console.Write("Введите вашу Фамилию: ");
			string lastName = Console.ReadLine();

			Console.Write("Введите вашу дату рождения: ");
			string birthYearInput = Console.ReadLine();
			if (!int.TryParse(birthYearInput, out int birthYear) || birthYear <= 0)
			{
				Console.WriteLine("Введите реальный возраст");
				return;
			}
			userProfile = new Profile(firstName, lastName, birthYear);
			Console.WriteLine($"Добавлен пользователь {userProfile.GetInfo()}");
		}

		static void SaveData()
		{
			if (userProfile != null)
				FileManager.SaveProfile(userProfile, profileFilePath);

			FileManager.SaveTodos(AppInfo.Todos, todoFilePath);
		}

		static void LoadData()
		{
			if (AppInfo.Todos == null)
				AppInfo.Todos = new TodoList();

			FileManager.EnsureDataDirectory(dataDir);

			if (!File.Exists(profileFilePath))
				File.WriteAllText(profileFilePath, "Default User");
			if (!File.Exists(todoFilePath))
				File.WriteAllText(todoFilePath, "");

			userProfile = FileManager.LoadProfile(profileFilePath);
			AppInfo.CurrentProfile = userProfile;

			if (userProfile == null)
			{
				CreateUser();
			}
			else
			{
				Console.WriteLine($"Загружен профиль: {userProfile.GetInfo()}");
			}

			var loadedTodos = FileManager.LoadTodos(todoFilePath);
			if (loadedTodos != null && loadedTodos.Count > 0)
			{
				AppInfo.Todos = loadedTodos;
				Console.WriteLine($"Загружено задач: {AppInfo.Todos.Count}");
			}
		}
	}
}