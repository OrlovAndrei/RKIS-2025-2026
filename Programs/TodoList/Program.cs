using System;
using System.IO;

namespace Todolist
{
	class Program
	{
		static TodoList todoList;
		static Profile userProfile;
		static string dataDirectory = "data";
		static string profileFilePath;
		static string todosFilePath;

		static void Main()
		{
			// Инициализация путей с использованием Path.Combine
			profileFilePath = Path.Combine(dataDirectory, "profile.txt");
			todosFilePath = Path.Combine(dataDirectory, "todo.csv");

			// Создаем директорию для данных
			FileManager.EnsureDataDirectory(dataDirectory);

			// Загружаем данные при запуске или создаем новые
			InitializeData();

			Console.Write("Работу сделали Приходько и Бочкарёв\n");

			// Если профиль не загружен, запрашиваем данные
			if (string.IsNullOrEmpty(userProfile.FirstName) || userProfile.BirthYear == 0)
			{
				InitializeProfile();
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

				ICommand command = CommandParser.Parse(input, todoList, userProfile, todosFilePath, profileFilePath);

				if (command != null)
				{
					command.Execute();
				}
				else
				{
					Console.WriteLine($"Неизвестная команда: {input.Split(' ')[0]}");
				}
			}
		}

		static void InitializeData()
		{
			// Проверяем существование файлов и загружаем данные
			if (File.Exists(profileFilePath) && File.Exists(todosFilePath))
			{
				// Загружаем существующие данные
				userProfile = FileManager.LoadProfile(profileFilePath);
				todoList = FileManager.LoadTodos(todosFilePath);
				Console.WriteLine("Данные загружены из файлов");
			}
			else
			{
				// Создаем новые объекты
				userProfile = new Profile();
				todoList = new TodoList();

				// Создаем файлы
				FileManager.EnsureFilesExist(profileFilePath, todosFilePath);
				Console.WriteLine("Созданы новые файлы данных");
			}
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
				Console.WriteLine($"Добавлен пользователь: {userProfile.GetInfo()}");

				// Сохраняем профиль
				FileManager.SaveProfile(userProfile, profileFilePath);
			}
			else
			{
				Console.WriteLine("Неверно введен год рождения. Установлен год по умолчанию.");
				userProfile.BirthYear = DateTime.Now.Year;
				FileManager.SaveProfile(userProfile, profileFilePath);
			}
		}
	}
}