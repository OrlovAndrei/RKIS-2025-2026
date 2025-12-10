namespace TodoList
{
	class Program
	{
		private const string CommandHelp = "help";
		private const string CommandProfile = "profile";
		private const string CommandAdd = "add";
		private const string CommandView = "view";
		private const string CommandExit = "exit";

		private static string firstName;
		private static string lastName;
		private static int birthYear;

		private static string[] todos = new string[2];
		private static int index = 0;

		private static bool[] statuses = new bool[2];
		private static DateTime[] dates = new DateTime[2];

		public static void Main()
		{
			Console.WriteLine("Работу выполнил: Измайлов");

			Console.Write("Введите ваше имя: ");
			firstName = Console.ReadLine();
			Console.Write("Введите вашу фамилию: ");
			lastName = Console.ReadLine();

			Console.Write("Введите ваш год рождения: ");
			birthYear = int.Parse(Console.ReadLine());
			int age = DateTime.Now.Year - birthYear;

			Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");

			while (true)
			{
				Console.Write("Введите команду: ");
				string command = Console.ReadLine();

				if (command == CommandHelp)
				{
					ShowHelp();
				}
				else if (command == CommandProfile)
				{
					ShowProfile();
				}
				else if (command.StartsWith(CommandAdd))
				{
					AddTask(command);
				}
				else if (command == CommandView)
				{
					ViewTasks();
				}
				else if (command == CommandExit)
				{
					Console.WriteLine("Программа завершена.");
					break;
				}
				else
				{
					Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
				}
			}
		}

		private static void ShowHelp()
		{
			Console.WriteLine("""
            Доступные команды:
            help — список команд
            profile — выводит данные профиля
            add "текст задачи" — добавляет задачу
            view — просмотр всех задач
            exit — завершить программу
            """);
		}

		private static void ShowProfile()
		{
			Console.WriteLine($"{firstName} {lastName}, {birthYear}");
		}

		private static void AddTask(string command)
		{
			string task = command.Split(" ", 2)[1];
			if (index >= todos.Length)
			{
				ExpandArray();
			}

			todos[index] = task;
			statuses[index] = false;
			dates[index] = DateTime.Now;

			index++;
			Console.WriteLine($"Задача добавлена: {task}");
		}

		private static void ExpandArray()
		{
			string[] newTodos = new string[todos.Length * 2];
			bool[] newStatuses = new bool[statuses.Length * 2];
			DateTime[] newDates = new DateTime[dates.Length * 2];

			for (int i = 0; i < todos.Length; i++)
				newTodos[i] = todos[i];

			for (int i = 0; i < statuses.Length; i++)
				newStatuses[i] = statuses[i];

			for (int i = 0; i < dates.Length; i++)
				newDates[i] = dates[i];

			todos = newTodos;
			statuses = newStatuses;
			dates = newDates;
		}

		private static void ViewTasks()
		{
			Console.WriteLine("Список задач:");
			for (int i = 0; i < index; i++)
			{
				Console.WriteLine($"{i + 1}) {todos[i]} | статус: {(statuses[i] ? "выполнено" : "не выполнено")} | дата: {dates[i]}");
			}
		}
	}
}
