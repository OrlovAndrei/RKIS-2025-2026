internal class Program
{
	public static void Main(string[] args)
	{
		Console.WriteLine("Работу выполнили: Филимонов и Фаградян 3833.9");
		
		string name, surname;
		int age;
		AddUser(out name, out surname, out age);
		
		var arrayLength = 2;
		var todos = new string[arrayLength];
		var statuses = new bool[arrayLength];
		var dates = new DateTime[arrayLength];
		int currentTaskNumber = 0;
		
		while (true)
		{
			Console.WriteLine("Введите команду: для помощи напиши команду help");
			string userCommand = Console.ReadLine();
			if (userCommand == "exit") break;
			switch (userCommand.Split()[0])
			{
				case "help":
					Console.WriteLine("help - выводит список всех доступных команд\n" +
					                  "profile - выводит ваши данные\n" +
					                  "add - добавляет новую задачу\n" +
					                  "view - просмотр задач");
					break;
				case "profile":
					Console.WriteLine("Пользователь: " + name + " " + surname + ", Возраст " + age);
					break;
				case "add":
					if (currentTaskNumber == todos.Length)
						ArrayExpansion(ref todos, ref statuses, ref dates);
					AddTask(todos, statuses, dates, ref currentTaskNumber, userCommand);
					break;
				case "view":
					ViewTasks(todos, statuses, dates);
					break;
				case "done":
					MarkTaskDone(statuses, dates, userCommand);
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
		int date = int.Parse(Console.ReadLine());
		age = 2025 - date;
		Console.WriteLine("Добавлен пользователь " + GetUserInfo(name, surname, age));
	}

	private static string GetUserInfo(string name, string surname, int age) =>
		name + " " + surname + ", возраст: " + age;
	
	private static void AddTask(string[] todos, bool[] statuses, DateTime[] dates, ref int currentTaskNumber, string task)
	{
		var taskText = task.Split('\"', 3);
		todos[currentTaskNumber] = taskText[1];
		dates[currentTaskNumber] = DateTime.Now;
		statuses[currentTaskNumber] = false;
		currentTaskNumber++;
	}
	private static void ViewTasks(string[] todos, bool[] statuses, DateTime[] dates)
	{
		for (int i = 0; i < todos.Length; i++)
		{
			if (!string.IsNullOrEmpty(todos[i]))
				Console.WriteLine($"{i} {todos[i]} {statuses[i]} {dates[i]}");
		}
	}
	private static void MarkTaskDone(bool[] statuses, DateTime[] dates, string doneCommandText)
	{
		var taskDone = doneCommandText.Split(' ', 2);
		var taskNumber = int.Parse(taskDone[1]);
		statuses[taskNumber] = true;
		dates[taskNumber] = DateTime.Now;
	}
	private static void ArrayExpansion(ref string[] todos, ref bool[] statuses, ref DateTime[] dates)
	{
		var tempTodos = new string[todos.Length * 2];
		var tempDates = new DateTime[todos.Length * 2];
		var tempStatuses = new bool[todos.Length * 2];
		for (var i = 0; i < todos.Length; i++)
		{
			tempTodos[i] = todos[i];
			tempDates[i] = dates[i];
			tempStatuses[i] = statuses[i];
		}

		todos = tempTodos;
		dates = tempDates;
		statuses = tempStatuses;
	}
}