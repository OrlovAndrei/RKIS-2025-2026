namespace TodoList
{
	class Program
	{
		public static void Main()
		{
			Console.WriteLine("Выполнил Герасимов Егор");
			Console.Write("Введите имя: "); 
			string firstName = Console.ReadLine();
			Console.Write("Введите фамилию: ");
			string lastName = Console.ReadLine();

			Console.Write("Введите год рождения: ");
			int year = int.Parse(Console.ReadLine());
			int age = (DateTime.Now.Year - year);
			
			string output = "Добавлен пользователь " + firstName + " " + lastName + ", возраст - " + age;
			Console.WriteLine(output);
			
			string[] todos = new string[2];
			int index = 0;
			
			while (true)
            {
                Console.Write("Введите команду: ");
                string command = Console.ReadLine();

                if (command == "profile")
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
            }
		}
	}
}