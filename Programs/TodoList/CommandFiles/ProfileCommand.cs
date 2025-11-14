using System;

namespace Todolist
{
	public class ProfileCommand : ICommand
	{
		public Profile UserProfile { get; set; }
		public string ProfileFilePath { get; set; }
		TodoList ICommand.TodoList { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public void Execute()
		{
			if (string.IsNullOrEmpty(UserProfile.FirstName) || string.IsNullOrEmpty(UserProfile.LastName))
			{
				Console.WriteLine("Данные пользователя не заполнены");
				InitializeProfile();
			}
			else
			{
				Console.WriteLine(UserProfile.GetInfo());
			}
		}

		private void InitializeProfile()
		{
			bool isValid = true;
			int currentYear = DateTime.Now.Year;

			Console.Write("Введите свое имя: ");
			UserProfile.FirstName = Console.ReadLine();
			Console.Write("Введите свою фамилию: ");
			UserProfile.LastName = Console.ReadLine();
			Console.Write("Введите свой год рождения: ");

			try
			{
				UserProfile.BirthYear = int.Parse(Console.ReadLine());
			}
			catch (Exception)
			{
				isValid = false;
			}

			if ((isValid == true) && (UserProfile.BirthYear <= currentYear))
			{
				Console.WriteLine($"Добавлен пользователь: {UserProfile.GetInfo()}");

				// Сохраняем профиль после инициализации
				if (!string.IsNullOrEmpty(ProfileFilePath))
				{
					FileManager.SaveProfile(UserProfile, ProfileFilePath);
				}
			}
			else
			{
				Console.WriteLine("Неверно введен год рождения");
			}
		}
	}
}