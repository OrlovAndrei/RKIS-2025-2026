namespace TodoList
{
	class Program
	{
		public static void Main()
		{
			Console.WriteLine("Работу выполнил Турчин 3833.9");
			Console.Write("Введите имя: "); 
			string name = Console.ReadLine();
			Console.Write("Введите фамилию: ");
			string surname = Console.ReadLine();

			Console.Write("Введите год рождения: ");
			int year = int.Parse(Console.ReadLine());
			int age = DateTime.Now.Year - year;

			Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");
			
			var taskList = new string[2];
			var count = 0;
			while (true)
			{
				Console.WriteLine("Введите команду:");
				var command = Console.ReadLine();

				if (command == "help") Console.WriteLine(
					"""
					Доступные команды:
					help — список команд
					profile — выводит данные профиля
					exit — завершить программу
					"""
				);
				else if (command == "profile") Console.WriteLine($"{name} {surname}, возраст - {year}");
				else if (command.StartsWith("add "))
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
				else if (command == "view")
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
			}
		}
	}
}