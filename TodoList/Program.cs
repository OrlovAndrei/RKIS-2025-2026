namespace TodoList
{
	class Program
	{
		private static string name;
		private static string surname;
		private static int age;
		
		private static string[] taskList = new string[2];
		private static bool[] statuses = new bool[2];
		private static DateTime[] dates = new DateTime[2];
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
				else if (command == "view") ViewTasks();
				else if (command == "exit")
				{
					Console.WriteLine("Программа завершена");
					break;
				}
				else Console.WriteLine("Введите help для списка команд");
			}
		}
		
		private static void ViewTasks()
		{
			Console.WriteLine("Список задач:");
			for (var i = 0; i < count; i++)
			{
				Console.WriteLine($"{i + 1}) {taskList[i]} статус:{statuses[i]} {dates[i]}");
			}
		}

		private static void AddTask(string command)
		{
			var task = command.Split(" ", 2)[1];
			if (count == taskList.Length) ExpandArrays();

			taskList[count] = task;
			count++;
			Console.WriteLine($"Задача добавлена: {task}");
		}
		private static void ExpandArrays()
		{
			var newSize = taskList.Length * 2;
			Array.Resize(ref taskList, newSize);
			Array.Resize(ref statuses, newSize);
			Array.Resize(ref dates, newSize);
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
				add "текст" — добавляет задачу
				view — просмотр всех задач
				exit — завершить программу
				"""
			);
		}
	}
}