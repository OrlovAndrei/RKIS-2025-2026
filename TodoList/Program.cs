namespace TodoList;

internal class Program
{
	public static void Main()
	{
		Console.WriteLine("Работу выполнили Антонов и Мадойкин 3833");
		Console.Write("Введите имя: ");
		var name = Console.ReadLine();
		Console.Write("Введите фамилию: ");
		var surname = Console.ReadLine();

		Console.Write("Введите год рождения: ");
		var year = int.Parse(Console.ReadLine());
		var age = DateTime.Now.Year - year;

		Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");

		var taskList = new string[2];
		var taskCount = 0;

		while (true)
		{
			Console.WriteLine("Введите команду: ");
			var command = Console.ReadLine();

			if (command == "help")
				Console.WriteLine("""
				                  Доступные команды:
				                  help — список команд
				                  profile — выводит данные профиля
				                  add "текст задачи" — добавляет задачу
				                  view — просмотр всех задач
				                  exit — завершить программу
				                  """);
			else if (command == "profile") Console.WriteLine($"{name} {surname}, {year}");
			else if (command.StartsWith("add "))
			{
				var task = command.Split(" ", 2)[1];
				if (taskCount == taskList.Length)
				{
					var newTaskList = new string[taskList.Length * 2];
					for (var i = 0; i < taskList.Length; i++) newTaskList[i] = taskList[i];
					taskList = newTaskList;
				}

				taskList[taskCount] = task;
				taskCount += 1;
				Console.WriteLine($"Задача добавлена: {task}");
			}
			else if (command == "view")
			{
				Console.WriteLine("Список задач:");
				foreach (var task in taskList)
					if (!string.IsNullOrWhiteSpace(task))
						Console.WriteLine(task);
			}
			else if (command == "exit")
			{
				Console.WriteLine("Программа завершена.");
				break;
			}
			else Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
		}
	}
}