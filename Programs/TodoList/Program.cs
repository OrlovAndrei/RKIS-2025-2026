using System;

namespace Todolist
{
	class Program
	{
		static TodoList todoList = new TodoList();
		static Profile userProfile = new Profile();

		static void Main()
		{
			bool isValid = true;
			int currentYear = DateTime.Now.Year;

			Console.Write("Работу сделали Приходько и Бочкарёв\n");
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
				}
				else
				{
					Console.WriteLine($"Неизвестная команда: {input.Split(' ')[0]}");
				}
			}
		}
	}
}