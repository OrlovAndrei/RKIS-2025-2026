namespace TodoList;

internal class Program
{
	static TodoList todoList = new ();
	private static void Main(string[] args)
	{
		Console.WriteLine("Работу выполнил: Морозов Иван 3833.9");

		string name, surname;
		int age;
		AddUser(out name, out surname, out age);

		while (true)
		{
			Console.WriteLine("Введите команду: ");
			var userCommand = Console.ReadLine();
			if (userCommand == "exit") break;
			switch (userCommand.Split()[0])
			{
				case "help":
					Console.WriteLine("help - выводит список всех доступных команд\n" +
					                  "profile - выводит ваши данные\n" +
					                  "add \"Новая задача\" - (флаги: --multiline/-m)\n" +
					                  "view - просмотр задач (флаги: --index/-i, --status/-s, --update-date/-d, --all/-a)\n" +
					                  "read idx - просмотр полного текста задач\n" +
					                  "done idx - отмечает задачу выполненной\n" +
					                  "delete idx - удаляет задачу по индексу\n" +
					                  "update idx \"Новая задача\" - обновляет текст задачи\n" +
					                  "exit - выйти");
					break;
				case "profile":
					GetUserInfo(name, surname, age);
					break;
				case "add":
					if (userCommand.Contains("-m") || userCommand.Contains("--multiline"))
						MultiLineAddTask();
					else
						AddTask(userCommand);
					break;
				case "done":
					MarkTaskDone(userCommand);
					break;
				case "delete":
					DeleteTask(userCommand);
					break;
				case "update":
					UpdateTask(userCommand);
					break;
				case "view":
					ViewTasks(userCommand);
					break;
				case "read":
					ReadTask(userCommand);
					break;
				default:
					Console.WriteLine("Неправильно введена команда");
					break;
			}
		}
	}

	private static void AddUser(out string name, out string surname, out int age)
	{
		Console.WriteLine("Введите свое имя");
		name = Console.ReadLine();
		Console.WriteLine("Ведите свою фамилию");
		surname = Console.ReadLine();
		Console.WriteLine("Ведите свой год рождения");
		var date = int.Parse(Console.ReadLine());
		age = 2025 - date;
		Console.WriteLine("Добавлен пользователь " + name + " " + surname + ", Возраст " + age);
	}

	private static void GetUserInfo(string name, string surname, int age)
	{
		Console.WriteLine("Пользователь: " + name + " " + surname + ", возраст: " + age);
	}

	private static void AddTask(string command)
	{
		var taskText = command.Split('\"', 3);
		todoList.Add(new TodoItem(taskText[1]));
	}

	private static void MultiLineAddTask()
	{
		var userTask = "";
		while (true)
		{
			var input = Console.ReadLine();
			if (input == "!end") break;
			userTask = userTask + "\n" + input;
		}
		
		todoList.Add(new TodoItem(userTask));
	}

	private static void MarkTaskDone(string command)
	{
		var taskDone = command.Split(' ', 2);
		var taskNumber = int.Parse(taskDone[1]);
		todoList.MarkDone(taskNumber);
	}

	private static void DeleteTask(string command)
	{
		var split = command.Split(' ', 2);
		var taskNumber = int.Parse(split[1]);
		todoList.Delete(taskNumber);
	}

	private static void UpdateTask(string command)
	{
		var split = command.Split(' ');
		var taskNumber = int.Parse(split[1]);
		todoList.Update(taskNumber, split[2]);
	}

	private static void ViewTasks(string command)
	{
		var flags = ParseFlags(command);
		var showAll = flags.Contains("--all") || flags.Contains("-a");
		var showIndex = flags.Contains("--index") || flags.Contains("-i") || showAll;
		var showStatus = flags.Contains("--status") || flags.Contains("-s") || showAll;
		var showUpdateDate = flags.Contains("--update-date") || flags.Contains("-d") || showAll;

		todoList.View(showIndex, showStatus ,showUpdateDate);
	}

	private static void ReadTask(string command)
	{
		int taskId = int.Parse(command.Split()[1]);
		todoList.Read(taskId);
	}
	
	private static List<string> ParseFlags(string command)
	{
		List<string> flags = [];

		foreach (var part in command.Split(' '))
			if (part.StartsWith("--"))
				flags.Add(part);
			else if (part.StartsWith('-'))
				for (var i = 1; i < part.Length; i++)
					flags.Add("-" + part[i]);

		return flags;
	}
}