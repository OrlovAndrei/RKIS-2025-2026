namespace TodoList;

internal class Program
{
	static TodoList todoList = new();
	public static void Main(string[] args)
	{
		Console.WriteLine("Работу выполнили: Филимонов и Фаградян 3833.9");
		Profile profile = AddUser();
		
		while (true)
		{
			Console.WriteLine("Введите команду: для помощи напиши команду help");
			string userCommand = Console.ReadLine();
			if (userCommand == "exit") break;
			switch (userCommand.Split()[0])
			{
				case "help":
					HelpCommand();
					break;
				case "profile":
					GetUserInfo(profile);
					break;
				case "add":
					if (userCommand.Contains("-m") || userCommand.Contains("--multiline"))
						MultiLineAddTask();
					else
						AddTask(userCommand);
					break;
				case "view":
					ViewTasks(userCommand);
					break;
				case "read":
					ReadTask(userCommand);
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
				default:
					Console.WriteLine("Неправильно введена команда");
					break;
			}
		}
	}

	private static void HelpCommand()
	{
		Console.WriteLine("help - выводит список всех доступных команд\n" +
		                  "profile - выводит ваши данные\n" +
		                  "add text (--multiline/-m) - добавляет новую задачу\n" +
		                  "view (--all/-a, --index/-i, --status/-s, --update-date/-d) - просмотр задач\n" +
		                  "read idx - просмотр полного текста задач\n" +
		                  "done idx - отмечает задачу выполненной\n" +
		                  "delete idx - удаляет задачу по индексу\n" +
		                  "update idx text - обновляет текст задачи");
	}

	private static Profile AddUser()
	{
		Console.WriteLine("Введите свое имя");
		var name = Console.ReadLine();
		Console.WriteLine("Ведите свою фамилию");
		var surname = Console.ReadLine();
		Console.WriteLine("Ведите свой год рождения");
		int date = int.Parse(Console.ReadLine());
		var age = 2025 - date;
		
		var user = new Profile(name, surname, age);
		Console.WriteLine("Добавлен пользователь " + user.GetInfo());
		return user;
	}
	private static void GetUserInfo(Profile profile)
	{
		Console.WriteLine("Пользователь: " + profile.GetInfo());
	}

	private static void AddTask(string task)
	{
		var taskText = task.Split('\"', 3);
		todoList.Add(new TodoItem(taskText[1]));
	}
	private static void MultiLineAddTask()
	{
		string userTask = "";
		while (true)
		{
			string input = Console.ReadLine();
			if (input == "!end") break;
			userTask = $"{userTask}\n{input}";
		}
		todoList.Add(new TodoItem(userTask));
	}
	private static void ViewTasks(string command)
	{
		var flags = ParseFlags(command);
		var showAll = flags.Contains("--all") || flags.Contains("-a");
		var showIndex = flags.Contains("--index") || flags.Contains("-i") || showAll;
		var showStatus = flags.Contains("--status") || flags.Contains("-s") || showAll;
		var showUpdateDate = flags.Contains("--update-date") || flags.Contains("-d") || showAll;

		todoList.View(showIndex, showStatus, showUpdateDate);
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
	private static void ReadTask(string command)
	{
		int taskId = int.Parse(command.Split()[1]);
		todoList.Read(taskId);
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
}