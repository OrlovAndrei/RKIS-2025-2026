using System;

namespace Todolist
{
	class Program
	{
		static TodoList todoList = new TodoList();
		static Profile userProfile = new Profile();
		static string dataDirectory = "data";
		static string profileFilePath = Path.Combine("data", "profile.txt");
		static string todosFilePath = Path.Combine("data", "todo.csv");

		static void Main()
		{
			// Создаем директорию для данных и файлы
			InitializeFileSystem();

			// Загружаем данные при запуске
			LoadData();

			Console.Write("Работу сделали Приходько и Бочкарёв\n");

			// Если профиль не загружен, запрашиваем данные
			if (string.IsNullOrEmpty(userProfile.FirstName) || userProfile.BirthYear == 0)
			{
				InitializeProfile();
				SaveData(); // Сохраняем новый профиль
			}
			else
			{
				Console.WriteLine($"Авторизован пользователь: {userProfile.GetInfo()}");
			}

			Console.WriteLine("Добро пожаловать в программу");
			Console.WriteLine("Введите 'help' для списка команд");

			while (true)
			{
				Console.WriteLine("=-=-=-=-=-=-=-=");
				string input = Console.ReadLine();

				if (string.IsNullOrWhiteSpace(input))
					continue;

				ICommand command = CommandParser.Parse(input, todoList, userProfile);

				if (command != null)
				{
					command.Execute();

					// Сохраняем данные после команд, которые меняют состояние
					if (IsStateChangingCommand(command))
					{
						SaveData();
					}
				}
				else
				{
					Console.WriteLine($"Неизвестная команда: {input.Split(' ')[0]}");
				}
			}
		}

		static void InitializeFileSystem()
		{
			FileManager.EnsureDataDirectory(dataDirectory);
			FileManager.EnsureFilesExist(profileFilePath, todosFilePath);
		}

		static void InitializeProfile()
		{
			bool isValid = true;
			int currentYear = DateTime.Now.Year;

			Console.Write("Введите свое имя: ");
			userProfile.FirstName = Console.ReadLine();
			Console.Write("Введите свою фамилию: ");
			userProfile.LastName = Console.ReadLine();
			Console.Write("Введите свой год рождения: ");

			try
			{
				userProfile.BirthYear = int.Parse(Console.ReadLine());
			}
			catch (Exception)
			{
				isValid = false;
			}

			if ((isValid == true) && (userProfile.BirthYear <= currentYear))
			{
				Console.WriteLine($"Добавлен пользователь:{userProfile.GetInfo()}");
			}
			else
			{
				Console.WriteLine("Неверно введен год рождения");
			}
		}
		static void LoadData()
		{
			userProfile = FileManager.LoadProfile(profileFilePath);
			todoList = FileManager.LoadTodos(todosFilePath);
		}

		static void SaveData()
		{
			FileManager.SaveProfile(userProfile, profileFilePath);
			FileManager.SaveTodos(todoList, todosFilePath);
		}

		static bool IsStateChangingCommand(ICommand command)
		{
			// Команды, которые не меняют состояние приложения
			return !(command is HelpCommand) &&
				   !(command is ViewCommand) &&
				   !(command is ProfileCommand) &&
				   !(command is ReadCommand);
		}
	}
}