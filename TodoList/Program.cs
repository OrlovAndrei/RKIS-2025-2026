namespace TodoList
{
	class Program
	{
		private static string name, surname;
		private static int age;
		
		private static string[] todos = new string[2];
		private static bool[] statuses = new bool[2];
		private static DateTime[] dates = new DateTime[2];
		private static int todosCount = 0;
		public static void Main()
		{
			Console.WriteLine("Работу выполнили: Десятун и Пономаренко 3833");
            
			Console.Write("Введите ваше имя: ");
			name = Console.ReadLine();
			Console.Write("Введите вашу фамилию: ");
			surname = Console.ReadLine();

			Console.Write("Введите ваш год рождения: ");
			int year = int.Parse(Console.ReadLine());
			age = DateTime.Now.Year - year;
			Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");
			
			while (true)
			{
				Console.WriteLine("\nВведите команду: ");
				string userInput = Console.ReadLine();
				
				if (userInput == "help")
				{
					Console.WriteLine("Команды:");
					Console.WriteLine("help — выводит список всех доступных команд с кратким описанием");
					Console.WriteLine("add \"текст\" — добавляет задачу");
					Console.WriteLine("view — просмотр задач");
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
					if (todosCount == todos.Length) ExpandArrays();

					todos[todosCount] = task;
					todosCount++;

					Console.WriteLine("Добавлена задача: " + task);
				}
				else if (userInput == "view")
				{
					for (var i = 0; i < todos.Length; i++)
					{
						var todo = todos[i];
						var status = statuses[i];
						var date = dates[i];

						if (!string.IsNullOrEmpty(todo))
							Console.WriteLine(i + ") " + date + " - " + todo + " выполнена: " + status);
					}
				}
				else
				{
					Console.WriteLine("Неизвестная команда. Воспользуйтесь командой help");
				}
			}
		}
		
		private static void ExpandArrays()
		{
			var newSize = todos.Length * 2;
			Array.Resize(ref todos, newSize);
			Array.Resize(ref statuses, newSize);
			Array.Resize(ref dates, newSize);
		}
	}
}