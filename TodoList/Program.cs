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
					{
						arrayLength *= 2;
						var tempTodos = new string[arrayLength];
						for (var i = 0; i < todos.Length; i++)
							tempTodos[i] = todos[i];
						todos = tempTodos;
					}

					var taskText = userCommand.Split('\"', 3);
					todos[currentTaskNumber] = taskText[1];
					currentTaskNumber++;
					break;
				case "view":
					for (var i = 0; i < arrayLength; i++)
						if (!string.IsNullOrEmpty(todos[i]))
							Console.WriteLine(todos[i]);
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
		Console.WriteLine("Добавлен пользователь " + name + " " + surname + ", Возраст " + age);
	}

	private static void GetUserInfo(string name, string surname, int age)
	{
		Console.WriteLine("Пользователь: " + name + " " + surname + ", возраст: " + age);
	}
}