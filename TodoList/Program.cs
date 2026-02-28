using System;

namespace TodoList
{
	public class Program
	{
		public static void Main()
		{
			Console.WriteLine("Работу выполнил: Измайлов");

			FileManager.EnsureDataDirectory("data");

			AppInfo.CurrentProfile = FileManager.LoadProfile(AppInfo.ProfileFilePath);
			AppInfo.Todos = FileManager.LoadTasks(AppInfo.TodoFilePath);

			if (AppInfo.CurrentProfile == null)
			{
				InitializeUserProfile();
			}
			else
			{
				Console.WriteLine($"Добро пожаловать назад, {AppInfo.CurrentProfile.FirstName}!");
			}

			while (true)
			{
				Console.Write("\nВведите команду: ");
				string input = Console.ReadLine();

				if (string.IsNullOrWhiteSpace(input)) continue;

				ICommand command = CommandParser.Parse(input);

				if (command != null)
				{
					if (command is ExitCommand)
					{
						command.Execute();
						break;
					}
					command.Execute();
				}
			}
		}

		private static void InitializeUserProfile()
		{
			Console.WriteLine("Пожалуйста, заполните данные профиля.");

			Console.Write("Введите ваше имя: ");
			string firstName = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(firstName)) firstName = "Гость";

			Console.Write("Введите вашу фамилию: ");
			string lastName = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(lastName)) lastName = "Гость";

			int birthYear;
			while (true)
			{
				Console.Write("Введите ваш год рождения: ");
				if (int.TryParse(Console.ReadLine(), out birthYear) && birthYear > 1900 && birthYear <= DateTime.Now.Year)
					break;
				Console.WriteLine("Ошибка: Введите корректный год.");
			}

			AppInfo.CurrentProfile = new Profile(firstName, lastName, birthYear);
			FileManager.SaveProfile(AppInfo.CurrentProfile, AppInfo.ProfileFilePath);
		}
	}
}