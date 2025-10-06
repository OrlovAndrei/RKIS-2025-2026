
namespace TodoList
{
	class MainClass
	{
		public static void Main()
		{
			Console.WriteLine("Работу выполнили: Вдовиченко и Кравец");
			Console.Write("Введите ваше имя: ");
			string name = Console.ReadLine();
			Console.Write("Введите вашу фамилию: ");
			string surname = Console.ReadLine();

			Console.Write("Введите ваш год рождения: ");
			int year = int.Parse(Console.ReadLine());

			string text = $"Добавлен пользователь {name} {surname}, возраст - {DateTime.Now.Year - year}";
			Console.WriteLine(text);

			string[] todoList = new string[2];
			int count = 0;

			while (true)
			{
				Console.WriteLine("Введите команду:");
				string command = Console.ReadLine();

				if (command == "help")
				{
					Console.WriteLine("Список команд:");
					Console.WriteLine("help — выводит список доступных команд");
					Console.WriteLine("profile — выводит данные пользователя");
					Console.WriteLine("exit — выход из цилка");
					Console.WriteLine("add \"текст задачи\" — добавляет новую задачу");
					Console.WriteLine("view — выводит все задачи");
				}
				else if (command == "profile")
				{
					Console.WriteLine(name + " " + surname + " - " + (DateTime.Now.Year - year));
				}
				else if (command == "exit")
				{
					Console.WriteLine("Выход из цилка.");
					break;
				}
				else if (command.StartsWith("add "))
				{//add make breakfast
					string task = command.Split(" ", 2)[1];
					if (count == todoList.Length)
					{
						string[] newTodoList = new string[todoList.Length*2];
						for (int i = 0; i < todoList.Length; i++)
						{
							newTodoList[i] = todoList[i];
						}

						todoList = newTodoList;
					}

					todoList[count] = task;
					count += 1;

					Console.WriteLine("Добавлена задача: " + task);
				}
				else if (command == "view")
				{
					Console.WriteLine("Список задач:");
					foreach (string todo in todoList)
					{
						if (!string.IsNullOrEmpty(todo))
						{
							Console.WriteLine(todo);
						}
					}
				}
				else
				{
					Console.WriteLine("Неизвестная команда.");
				}
			}
		}
	}
}