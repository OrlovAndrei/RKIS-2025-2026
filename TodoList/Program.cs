namespace TodoList
{
	class Program
	{
		private static string name;
		private static string surname;
		private static int age;
		
		private static string[] taskList = new string[2];
		private static int count;

		public static void Main()
		{
			Console.WriteLine("Работу выполнил Турчин 3833.9");
			Console.Write("Введите имя: "); 
			name = Console.ReadLine();
			Console.Write("Введите фамилию: ");
			surname = Console.ReadLine();

			Console.Write("Введите год рождения: ");
			int year = int.Parse(Console.ReadLine());
			age = DateTime.Now.Year - year;

			Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");
			
			while (true)
			{
				Console.WriteLine("Введите команду:");
				var command = Console.ReadLine();

				if (command == "help") Help();
				else if (command == "profile") Profile();
				else if (command.StartsWith("add ")) AddTask(command);
				else if (command == "view") ViewTasks(command);
				else if (command == "exit")
				{
					Console.WriteLine("Программа завершена");
					break;
				}
				else Console.WriteLine("Введите help для списка команд");
			}
		}
		
		private static void ViewTasks(string command)
		{
			Console.WriteLine("Список задач:");
			foreach (var task in taskList)
			{
				if (!string.IsNullOrWhiteSpace(task))
				{
					Console.WriteLine(task);
				}
			}
		}

		private static void AddTask(string command)
		{
			var task = command.Split(" ", 2)[1];
			if (count == taskList.Length)
			{
				var newTaskList = new string[taskList.Length * 2];
				for (var i = 0; i < taskList.Length; i++)
				{
					newTaskList[i] = taskList[i];
				}

				taskList = newTaskList;
			}

			taskList[count] = task;
			count++;
			Console.WriteLine($"Задача добавлена: {task}");
		}

		private static void Profile()
		{
			Console.WriteLine($"{name} {surname}, возраст - {age}");
		}

		private static void Help()
		{
			Console.WriteLine(
				"""
				Доступные команды:
				help — список команд
				profile — выводит данные профиля
				exit — завершить программу
				"""
			);
		}
	}
}