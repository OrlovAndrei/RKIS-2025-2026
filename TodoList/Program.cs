namespace TodoList
{
	class Program
	{
		public static void Main()
		{
			Console.WriteLine("Работу выполнили Николаенко Крошняк");
            
			Console.Write("Введите ваше имя: ");
			string name = Console.ReadLine();
			Console.Write("Введите вашу фамилию: ");
			string surname = Console.ReadLine();

			Console.Write("Введите ваш год рождения: ");
			int year = int.Parse(Console.ReadLine());
			int age = DateTime.Now.Year - year;
            
			Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");
			
			string[] todos = new string[2];
			bool isRunning = true;
			int taskCount = 0;

			while (isRunning)
			{
				Console.Write("\nВведите команду: ");
				string command = Console.ReadLine();
				
				if (command == "help")
				{
					Console.WriteLine("""
					Доступные команды:
					help — список команд
					profile — выводит данные 
					add "текст" — добавляет задачу
					view — просмотр всех задач
					exit — завершить программу
					""");
				}
				else if (command == "profile")
				{
					Console.WriteLine($"{name} {surname} {age}");
				}
				else if (command.StartsWith("add "))
				{
					string task = command.Split(" ", 2)[1];
					if (taskCount == todos.Length)
					{
						string[] newTodos = new string[todos.Length * 2];
						for (int i = 0; i < todos.Length; i++)
						{
							newTodos[i] = todos[i];
						}
						todos = newTodos;
					}

					todos[taskCount] = task;
					taskCount++;
					Console.WriteLine($"Задача добавлена: {task}");
				}
				else if (command == "view")
				{
					Console.WriteLine("Задачи:");
					foreach (string todo in todos)
					{
						if (!string.IsNullOrEmpty(todo))
						{
							Console.WriteLine(todo);
						}
					}
				}
				else if (command == "exit")
				{
					Console.WriteLine("Программа завершена.");
					isRunning = false;
				}
				else
				{
					Console.WriteLine("Неизвестная команда");
				}
			}
		}
	}
}