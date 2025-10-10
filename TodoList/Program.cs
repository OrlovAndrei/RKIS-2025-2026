namespace TodoList
{
	class Program
	{

		public static void Main()
		{
			Console.WriteLine("Работу выполнили: Вдовиченко и Кравец");

			Console.Write("Введите ваше имя: ");
			string userName = Console.ReadLine();
			Console.Write("Введите вашу фамилию: ");
			string userSurname = Console.ReadLine();
			Console.Write("Введите ваш год рождения: ");
			int birthYear = int.Parse(Console.ReadLine());

			int userAge = DateTime.Now.Year - birthYear;
			Console.WriteLine($"Добавлен пользователь {userName} {userSurname}, возраст - {userAge}");

			string[] todos = new string[2];
			bool[] statuses = new bool[2];
			DateTime[] dates = new DateTime[2];
			int taskCount = 0;

			while (true)
			{
				Console.Write("\nВведите команду: ");
				string command = Console.ReadLine();

				if (command.StartsWith("add "))
					AddTask(command, ref todos, ref statuses, ref dates, ref taskCount);
				else if (command == "view")
					ViewTasks(todos, statuses, dates, taskCount);
				else if (command == "help")
					ShowHelp();
				else if (command == "profile")
					Console.WriteLine($"{userName} {userSurname} — {userAge} лет");
				else if (command.StartsWith("done "))
					MarkTaskDone(command, statuses, dates);
				else if (command.StartsWith("delete "))
					DeleteTask(command, ref todos, ref statuses, ref dates, ref taskCount);
				else if (command.StartsWith("update "))
					UpdateTask(command, todos, dates);
				else if (command == "exit")
				{
					Console.WriteLine("Выход из программы.");
					break;
				}
			}
		}

		private static void ShowHelp()
		{
			Console.WriteLine("\nКоманды:");
			Console.WriteLine("add <текст> — добавить задачу");
			Console.WriteLine("view — показать задачи");
			Console.WriteLine("done <номер> — отметить выполненной");
			Console.WriteLine("delete <номер> — удалить задачу");
			Console.WriteLine("update <номер> <новый текст> — изменить текст");
			Console.WriteLine("profile — профиль пользователя");
			Console.WriteLine("exit — выход");
		}

		private static void AddTask(string command, ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int count)
		{
			string text = command.Substring(4);
			if (count == todos.Length)
				ExpandArrays(ref todos, ref statuses, ref dates);

			todos[count] = text;
			statuses[count] = false;
			dates[count] = DateTime.Now;
			count++;

			Console.WriteLine($"Добавлена задача: \"{text}\"");
		}

		private static void ViewTasks(string[] todos, bool[] statuses, DateTime[] dates, int count)
		{
			Console.WriteLine("\nСписок задач:");
			for (int i = 0; i < count; i++)
				Console.WriteLine($"{i + 1}. {todos[i]} — {(statuses[i] ? "Сделано" : "Не сделано")} — {dates[i]:dd.MM.yyyy HH:mm}");
		}

		private static void MarkTaskDone(string command, bool[] statuses, DateTime[] dates)
		{
			int index = int.Parse(command.Split(' ')[1]) - 1;
			statuses[index] = true;
			dates[index] = DateTime.Now;
			Console.WriteLine($"Задача #{index + 1} отмечена как выполненная.");
		}

		private static void DeleteTask(string command, ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int count)
		{
			throw new NotImplementedException();
		}

		private static void UpdateTask(string command, string[] todos, DateTime[] dates)
		{
			string[] parts = command.Split(' ', 3);
			int index = int.Parse(parts[1]) - 1;
			todos[index] = parts[2];
			dates[index] = DateTime.Now;
			Console.WriteLine($"Задача #{index + 1} обновлена.");
		}

		private static void ExpandArrays(ref string[] todos, ref bool[] statuses, ref DateTime[] dates)
		{
			int newSize = todos.Length * 2;
			Array.Resize(ref todos, newSize);
			Array.Resize(ref statuses, newSize);
			Array.Resize(ref dates, newSize);
		}
	}
}