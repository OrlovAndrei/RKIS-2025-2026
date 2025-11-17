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
						ArrayExpansion(ref todos);

					AddTask(ref todos, ref currentTaskNumber, userCommand);
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

	private static void AddTask(ref string[] todoArray, ref int currentTaskNumber, string task)
	{
		var taskText = task.Split('\"', 3);
		todoArray[currentTaskNumber] = taskText[1];
		currentTaskNumber++;
	}

	private static void TodoInfo(string[] todos)
	{
		Console.WriteLine("Ваш список задач:");
		for (var i = 0; i < todos.Length; i++)
			if (!string.IsNullOrEmpty(todos[i]))
				Console.WriteLine(todos[i]);
	}

	private static void ArrayExpansion(ref string[] array)
	{
		var tempArray = new string[array.Length * 2];
		for (var i = 0; i < array.Length; i++)
			tempArray[i] = array[i];
		array = tempArray;
	}
}