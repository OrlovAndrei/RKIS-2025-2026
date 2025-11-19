namespace TodoList
{
	class Program
	{
		public static void Main()
		{
			Console.WriteLine("Работу  выполнили Лютов и Легатов 3832");
			Console.Write("Введите ваше имя: "); 
			string firstName = Console.ReadLine();
			Console.Write("Введите вашу фамилию: ");
			string lastName = Console.ReadLine();

			Console.Write("Введите ваш год рождения: ");
			int year = int.Parse(Console.ReadLine());
			int age = DateTime.Now.Year - year;
			
			string text = "Добавлен пользователь " + firstName + " " + lastName + ", возраст - " + age;
			Console.WriteLine(text);
			
			string[] todos = new string[2];
			int index = 0;
			
            while (true)
            {
                Console.Write("Введите команду: ");
                string command = Console.ReadLine();

                if (command == "help")
                {
	                Console.WriteLine("Команды:");
	                Console.WriteLine("help — выводит список всех доступных команд с кратким описанием");
	                Console.WriteLine("profile — выводит данные пользователя");
	                Console.WriteLine("add text — добавляет новую задачу");
	                Console.WriteLine("view — выводит все задачи");
	                Console.WriteLine("exit — выход из программы");
                }
                else if (command == "profile")
                {
                    Console.WriteLine(firstName + " " + lastName + ", - " + age);
                }
                else if (command.StartsWith("add "))
                {
	                string task = command.Split("add ")[1];
	                if (index == todos.Length)
	                {
		                string[] newTodos = new string[todos.Length*2];
		                for (int i = 0; i < todos.Length; i++)
		                {
			                newTodos[i] = todos[i];
		                }

		                todos = newTodos;
	                }

	                todos[index] = task;
	                index++;

	                Console.WriteLine("Добавлена задача: " + task);
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
	                Console.WriteLine("Выход из программы.");
	                break;
                }
                else
                {
	                Console.WriteLine("Неизвестная команда.");
                }
            }
		}
	}
}