namespace TodoList;

internal class Program
{
	public static void Main()
	{
		Console.WriteLine("Работу  выполнили Лютов и Легатов 3832");
		Console.Write("Введите ваше имя: ");
		var firstName = Console.ReadLine();
		Console.Write("Введите вашу фамилию: ");
		var lastName = Console.ReadLine();

		Console.Write("Введите ваш год рождения: ");
		var year = int.Parse(Console.ReadLine());
		var age = DateTime.Now.Year - year;

		var text = "Добавлен пользователь " + firstName + " " + lastName + ", возраст - " + age;
		Console.WriteLine(text);

		var todos = new string[2];
		var statuses = new bool[2];
		var dates = new DateTime[2];
		var index = 0;

		while (true)
		{
			Console.Write("Введите команду: ");
			var command = Console.ReadLine();

			if (command == "help") HelpCommand();
			else if (command == "profile") ShowProfile(firstName, lastName, age);
			else if (command == "exit") break;
			else if (command.StartsWith("add ")) AddTodo(command, ref todos, ref statuses, ref dates, ref index);
			else if (command == "view") ViewTodo(todos, statuses, dates, index);
			else Console.WriteLine("Неизвестная команда.");
		}
	}

	private static void HelpCommand()
	{
		Console.WriteLine("Команды:");
		Console.WriteLine("help — выводит список всех доступных команд с кратким описанием");
		Console.WriteLine("profile — выводит данные пользователя");
		Console.WriteLine("add text — добавляет новую задачу");
		Console.WriteLine("view — выводит все задачи");
		Console.WriteLine("exit — выход из программы");
	}

	private static void ShowProfile(string firstName, string lastName, int age)
	{
		Console.WriteLine(firstName + " " + lastName + ", - " + age);
	}

	private static void AddTodo(string command, ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int index)
	{
		var task = command.Substring(4).Trim();
		if (index == todos.Length)
		{
			var newTodos = new string[todos.Length * 2];
			for (var i = 0; i < todos.Length; i++) newTodos[i] = todos[i];
			todos = newTodos;
		}

		todos[index] = task;
		statuses[index] = false;
		dates[index] = DateTime.Now;
		index++;

		Console.WriteLine("Добавлена задача: " + task);
	}

	
	private static void ViewTodo(string[] todos, bool[] statuses, DateTime[] dates, int index)
	{
		Console.WriteLine("Задачи:");
		for (var i = 0; i < index; i++)
		{
			var todo = todos[i];
			var status = statuses[i];
			var date = dates[i];

			Console.WriteLine($"{i}) {date} - {todo} статус:{status}");
		}
	}
}