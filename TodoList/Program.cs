namespace TodoList;

internal class Program
{
	private static string name;
	private static string surname;
	private static int age;

	private static string[] taskList = new string[2];
	private static bool[] taskStatuses = new bool[2];
	private static DateTime[] taskDates = new DateTime[2];
	private static int taskCount;

	public static void Main()
	{
		Console.WriteLine("Работу выполнили Антонов и Мадойкин 3833");
		Console.Write("Введите имя: ");
		name = Console.ReadLine();
		Console.Write("Введите фамилию: ");
		surname = Console.ReadLine();

		Console.Write("Введите год рождения: ");
		var year = int.Parse(Console.ReadLine());
		age = DateTime.Now.Year - year;

		Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");

		while (true)
		{
			Console.WriteLine("Введите команду: ");
			var command = Console.ReadLine();

			if (command == "help")
				Help();
			else if (command == "profile")
				Profile();
			else if (command.StartsWith("add "))
				AddTask(command);
			else if (command == "view")
				ViewTasks();
			else if (command.StartsWith("done "))
				DoneTask(command);
			else if (command.StartsWith("delete "))
				DeleteTask(command);
			else if (command == "exit")
			{
				Console.WriteLine("Программа завершена.");
				break;
			}
			else
				Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
		}
	}

	private static void DeleteTask(string input)
	{
		string[] parts = input.Split(' ', 2);
		var taskIndex = int.Parse(parts[1]) - 1;

		for (var i = taskIndex; i < taskCount - 1; i++)
		{
			taskList[i] = taskList[i + 1];
			taskStatuses[i] = taskStatuses[i + 1];
			taskDates[i] = taskDates[i + 1];
		}

		taskCount--;
		Console.WriteLine($"Задача {taskIndex + 1} удалена.");
	}
	private static void DoneTask(string input)
	{
		var parts = input.Split(' ', 2);
		var taskIndex = int.Parse(parts[1]) - 1;

		taskStatuses[taskIndex] = true;
		taskDates[taskIndex] = DateTime.Now;

		Console.WriteLine($"Задача {taskIndex + 1} выполнена.");
	}

	private static void ViewTasks()
	{
		Console.WriteLine("Список задач:");
		for (var i = 0; i < taskCount; i++)
			Console.WriteLine($"{i + 1}. {taskList[i]} статус:{taskStatuses[i]} {taskDates[i]}");
	}

	private static void AddTask(string input)
	{
		var task = input.Split(" ", 2)[1];
		if (taskCount == taskList.Length) ExpandArrays();

		taskList[taskCount] = task;
		taskStatuses[taskCount] = false;
		taskDates[taskCount] = DateTime.Now;

		taskCount += 1;
		Console.WriteLine($"Задача добавлена: {task}");
	}

	private static void Profile()
	{
		Console.WriteLine($"{name} {surname}, {age}");
	}

	private static void Help()
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

	private static void ExpandArrays()
	{
		var newSize = taskList.Length * 2;
		Array.Resize(ref taskList, newSize);
		Array.Resize(ref taskStatuses, newSize);
		Array.Resize(ref taskDates, newSize);
	}
}