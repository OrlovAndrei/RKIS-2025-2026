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
			string task = command.Split(" ", 2)[1];
			if (count == todos.Length)
			{
				string[] newTodoList = new string[todos.Length * 2];
				for (int i = 0; i < todos.Length; i++)
				{
					newTodoList[i] = todos[i];
				}

				todos = newTodoList;
			}

			todos[count] = task;
			count += 1;

			Console.WriteLine("Добавлена задача: " + task);
		}

		private static void ViewTasks(string[] todos, bool[] statuses, DateTime[] dates, int count)
		{
			Console.WriteLine("Список задач:");
			foreach (string todo in todos)
			{
				if (!string.IsNullOrEmpty(todo))
				{
					Console.WriteLine(todo);
				}
			}
		}

		private static void MarkTaskDone(string command, bool[] statuses, DateTime[] dates)
		{
			throw new NotImplementedException();
		}

		private static void DeleteTask(string command, ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int count)
		{
			throw new NotImplementedException();
		}

		private static void UpdateTask(string command, string[] todos, DateTime[] dates)
		{
			throw new NotImplementedException();
		}
	}
}