namespace TodoList
{
	class Program
	{
		public static void Main()
		{
			Console.WriteLine("Работу выполнили: Десятун и Пономаренко 3833");
            
			Console.Write("Введите ваше имя: ");
			string name = Console.ReadLine();
			Console.Write("Введите вашу фамилию: ");
			string surname = Console.ReadLine();

			Console.Write("Введите ваш год рождения: ");
			int year = int.Parse(Console.ReadLine());
			var age = DateTime.Now.Year - year;
			Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");
			
			string[] todos = new string[2];
			int todosCount = 0;

			while (true)
			{
				Console.WriteLine("\nВведите команду: ");
				string userInput = Console.ReadLine();
				
				if (userInput == "help")
				{
					Console.WriteLine("Команды:");
					Console.WriteLine("help — выводит список всех доступных команд с кратким описанием");
					Console.WriteLine("profile — выводит данные пользователя");
					Console.WriteLine("exit — выход из программы");
				}
				else if (userInput == "profile")
				{
					Console.WriteLine($"{name} {surname} {age}");
				}
				else if (userInput.StartsWith("add "))
				{
					string task = userInput.Split("add ")[1];
					if (todosCount == todos.Length)
					{
						string[] newTodos = new string[todos.Length * 2];
						for (int i = 0; i < todos.Length; i++)
						{
							newTodos[i] = todos[i];
						}

						todos = newTodos;
					}

					todos[todosCount] = task;
					todosCount++;

					Console.WriteLine("Добавлена задача: " + task);
				}
				else if (userInput == "view")
				{
					foreach (string todo in todos)
					{
						if (!string.IsNullOrEmpty(todo))
						{
							Console.WriteLine(todo);
						}
					}
				}
				else
				{
					Console.WriteLine("Неизвестная команда. Воспользуйтесь командой help");
				}
			}
		}
	}
}