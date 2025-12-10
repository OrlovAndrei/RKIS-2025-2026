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
					ShowHelp();
				}
				else if (userInput == "profile")
				{
					ShowProfile();
				}
				else if (userInput.StartsWith("add "))
				{
					AddTodo(userInput);
				}
				else if (userInput == "view")
				{
					ViewTodos();
				}
				else if (userInput.StartsWith("done "))
                {
                    DoneTodo(userInput);
                }
				else if (userInput.StartsWith("delete "))
				{
					DeleteTodo(userInput);
				}
				else if (userInput == "exit")
				{
					Console.WriteLine("Завершение работы программы...");
            		Environment.Exit(0);
				}
				else
				{
					Console.WriteLine("Неизвестная команда. Воспользуйтесь командой help");
				}
			}
		}
		
		private static void ShowHelp()
        {
            Console.WriteLine("Команды:");
            Console.WriteLine("help — выводит список всех доступных команд с кратким описанием");
            Console.WriteLine("add \"текст\" — добавляет задачу");
            Console.WriteLine("done idx — помечает задачу под номером idx как выполненную");
            Console.WriteLine("view — просмотр задач");
            Console.WriteLine("profile — выводит данные пользователя");
            Console.WriteLine("exit — выход из программы");
        }

        private static void ShowProfile()
        {
            Console.WriteLine($"{name} {surname} {age}");
        }

        private static void AddTodo(string userInput)
        {
            string task = userInput.Split("add ")[1];
            if (todosCount == todos.Length)
                ExpandArrays();

            todos[todosCount] = task;
            statuses[todosCount] = false;
            dates[todosCount] = DateTime.Now;
            todosCount++;

            Console.WriteLine("Добавлена задача: " + task);
        }

 		private static void DoneTodo(string userInput)
        {
            var parts = userInput.Split(' ', 2);
            var index = int.Parse(parts[1]);
            if (index < 0 || index >= todosCount)
            {
                Console.WriteLine("Задача с таким номером не найдена.");
                return;
            }

            statuses[index] = true;
            dates[index] = DateTime.Now;

            Console.WriteLine($"Задача {index} помечена как выполненная.");
        }
        private static void DeleteTodo(string userInput)
        {
	        var parts = userInput.Split(' ', 2);
	        var index = int.Parse(parts[1]);
	        if (index < 0 || index >= todosCount)
	        {
		        Console.WriteLine("Задача с таким номером не найдена.");
		        return;
	        }

	        for (int i = index; i < todosCount - 1; i++)
	        {
		        todos[i] = todos[i + 1];
		        statuses[i] = statuses[i + 1];
		        dates[i] = dates[i + 1];
	        }

	        todosCount--;
	        todos[todosCount] = null;
	        statuses[todosCount] = false;
	        dates[todosCount] = default;

	        Console.WriteLine($"Задача {index} удалена.");
        }

        private static void ViewTodos()
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

		private static void ExpandArrays()
		{
			var newSize = todos.Length * 2;
			Array.Resize(ref todos, newSize);
			Array.Resize(ref statuses, newSize);
			Array.Resize(ref dates, newSize);
		}
	}
}