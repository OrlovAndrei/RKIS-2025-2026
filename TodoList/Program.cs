internal class Program
{
	private static void Main(string[] args)
	{
		Console.WriteLine("Работу выполнил: Морозов Иван 3833.9");

		string name, surname;
		int age;
		AddUser(out name, out surname, out age);

		var arrayLength = 2;
		var todos = new string[arrayLength];
		var statuses = new bool[arrayLength];
		var dates = new DateTime[arrayLength];
		var currentTaskNumber = 0;

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
					                  "add - добавляет новую задачу\n" +
					                  "view - просмотр задач\n" +
					                  "exit - выйти");
					break;
				case "profile":
					GetUserInfo(name, surname, age);
					break;
				case "add":
					if (currentTaskNumber == todos.Length)
						ArrayExpansion(ref todos, ref statuses, ref dates);

					AddTask(todos, statuses, dates, ref currentTaskNumber, userCommand);
					break;
				case "view":
					TodoInfo(todos);
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

	private static void AddTask(string[] todos, bool[] statuses, DateTime[] dates, ref int currentTaskNumber, string task)
	{
		var taskText = task.Split('\"', 3);
		todos[currentTaskNumber] = taskText[1];
		dates[currentTaskNumber] = DateTime.Now;
		statuses[currentTaskNumber] = false;
		currentTaskNumber++;
	}

	private static void TodoInfo(string[] todos)
	{
		Console.WriteLine("Ваш список задач:");
		for (var i = 0; i < todos.Length; i++)
			if (!string.IsNullOrEmpty(todos[i]))
				Console.WriteLine(todos[i]);
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