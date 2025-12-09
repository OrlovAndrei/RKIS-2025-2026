namespace TodoList
{
	class Program
	{
		static string name, surname;
		static int age;
		
		static string[] todos = new string[2];
		static bool[] statuses = new bool[2];
		static DateTime[] dates = new DateTime[2];
		static int taskCount = 0;
		
		static bool isRunning = true;

		public static void Main()
		{
			Console.WriteLine("Работу выполнили Николаенко Крошняк");
            
			Console.Write("Введите ваше имя: ");
			name = Console.ReadLine();
			Console.Write("Введите вашу фамилию: ");
			surname = Console.ReadLine();

			Console.Write("Введите ваш год рождения: ");
			int year = int.Parse(Console.ReadLine());
			age = DateTime.Now.Year - year;
            
			Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");
			

			while (isRunning)
			{
				Console.Write("\nВведите команду: ");
				string command = Console.ReadLine();
				
				if (command == "help")
				{
					ShowHelp();
				}
				else if (command == "profile")
				{
					ShowProfile();
				}
				else if (command.StartsWith("add "))
				{
					AddTask(command);
				}
				else if (command.StartsWith("done "))
				{
					CompleteTask(command);
				}
				else if (command.StartsWith("delete "))
				{
					DeleteTask(command);
				}
				else if (command.StartsWith("update "))
				{
					UpdateTask(command);
				}
				else if (command == "view")
				{
					ViewTasks();
				}
				else if (command == "exit")
				{
					ExitProgram();
				}
				else
				{
					UnknownCommand();
				}
			}
		}
		
		private static void ShowHelp()
        {
            Console.WriteLine("""
            Доступные команды:
            help — список команд
            profile — выводит данные 
            add "текст" — добавляет задачу
            view — просмотр всех задач
            done "индекс" - отметить выполненным
            delete "индекс" - удалить задачу
            update "индекс" "текст" - изменить текст
            exit — завершить программу
            """);
        }

        private static void ShowProfile()
        {
            Console.WriteLine($"{name} {surname} {age}");
        }

        private static void AddTask(string command)
        {
            string task = command.Split(" ", 2)[1];
            if (taskCount == todos.Length)
                ExpandArrays();

            todos[taskCount] = task;
            taskCount++;
            Console.WriteLine($"Задача добавлена: {task}");
        }

        private static void CompleteTask(string command)
        {
            var parts = command.Split(' ', 2);
            var index = int.Parse(parts[1]) - 1;

            statuses[index] = true;
            dates[index] = DateTime.Now;

            Console.WriteLine($"Задача {index + 1} выполнена.");
        }

        private static void DeleteTask(string command)
        {
            var parts = command.Split(' ', 2);
            var index = int.Parse(parts[1]) - 1;

            for (var i = index; i < taskCount - 1; i++)
            {
                todos[i] = todos[i + 1];
                statuses[i] = statuses[i + 1];
                dates[i] = dates[i + 1];
            }

            taskCount--;
            Console.WriteLine($"Задача {index + 1} удалена.");
        }

        private static void UpdateTask(string command)
        {
            var parts = command.Split(' ', 3);
            var index = int.Parse(parts[1]);
            var task = parts[2];

            todos[index] = task;
            dates[index] = DateTime.Now;

            Console.WriteLine("Задача обновлена");
        }

        private static void ViewTasks()
        {
            Console.WriteLine("Задачи:");
            for (var i = 0; i < taskCount; i++)
            {
                Console.WriteLine($"{i + 1}) {todos[i]} статус:{statuses[i]} {dates[i]}");
            }
        }

        private static void ExitProgram()
        {
            Console.WriteLine("Программа завершена.");
            isRunning = false;
        }

        private static void UnknownCommand()
        {
            Console.WriteLine("Неизвестная команда");
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