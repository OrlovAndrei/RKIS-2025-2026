namespace TodoList
{
    class Program
    {
		public static void Main()
		{
			Console.WriteLine("Работу выполнил: Измайлов");

			Console.Write("Введите ваше имя: ");
			string name = Console.ReadLine();
			Console.Write("Введите вашу фамилию: ");
			string surname = Console.ReadLine();

			Console.Write("Введите ваш год рождения: ");
			int year = int.Parse(Console.ReadLine());
			int age = DateTime.Now.Year - year;

			Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");
		}
			string[] todos = new string[2];
			int index = 0;
			while (true)
			{
				Console.Write("Введите команду: ");
				string command = Console.ReadLine();

				if (command == "help")
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
				else if (command == "profile")
				{
					Console.WriteLine($"{firstName} {lastName}, {year}");
				}
				else if (input.StartsWith("add"))
				{
					string task = input.Split(" ", 2)[1];
					if (index >= todos.Length)
					{
						string[] newTodos = new string[todos.Length * 2];
						for (int i = 0; i < todos.Length; i++)
							newTodos[i] = todos[i];

						todos = newTodos;
					}

					todos[index] = task;
					index++;
					Console.WriteLine($"Задача добавлена: {task}");
				}
				else if (input == "view")
				{
					Console.WriteLine("Список задач:");
					for (int i = 0; i < index; i++)
					{
						Console.WriteLine($"{i + 1}) {todos[i]}");
					}
				}
				else if (command == "exit")
				{
					Console.WriteLine("Программа завершена.");
					break;
				}
				else
				{
					Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
				}
			}
    }
}