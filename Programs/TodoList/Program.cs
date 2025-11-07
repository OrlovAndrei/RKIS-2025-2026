using System;

namespace Todolist
{
	class Program
	{
		static TodoList todoList = new TodoList();
		static Profile userProfile = new Profile();

		static void Main()
		{

			Console.WriteLine("Добро пожаловать в программу");
			Console.WriteLine("Введите 'help' для списка команд");

			while (true)
			{
				Console.WriteLine("=-=-=-=-=-=-=-=");
				string input = Console.ReadLine();

				if (string.IsNullOrWhiteSpace(input))
					continue;

				ICommand command = ParseCommand(input);
				if (command != null)
				{
					command.Execute();
				}
				else
				{
					Console.WriteLine($"Неизвестная команда: {input.Split(' ')[0]}");
				}
			}
		}

		static ICommand ParseCommand(string input)
		{
			string[] parts = input.Split(' ');
			string commandName = parts[0].ToLower();

			switch (commandName)
			{
				case "help":
					return new HelpCommand();

				case "profile":
					return new ProfileCommand { UserProfile = userProfile };

				case "add":
					return CreateAddCommand(parts);

				case "view":
					return CreateViewCommand(parts);

				case "read":
					if (parts.Length < 2)
					{
						Console.WriteLine("Ошибка: не указан номер задачи");
						return null;
					}
					return new ReadCommand { TodoList = todoList, TaskNumber = int.Parse(parts[1]) };

				case "done":
					if (parts.Length < 2)
					{
						Console.WriteLine("Ошибка: не указан номер задачи");
						return null;
					}
					return new DoneCommand { TodoList = todoList, TaskNumber = int.Parse(parts[1]) };

				case "delete":
					if (parts.Length < 2)
					{
						Console.WriteLine("Ошибка: не указан номер задачи");
						return null;
					}
					return new DeleteCommand { TodoList = todoList, TaskNumber = int.Parse(parts[1]) };

				case "update":
					if (parts.Length < 3)
					{
						Console.WriteLine("Ошибка: не указан номер задачи или новый текст");
						return null;
					}
					string newText = string.Join(" ", parts, 2, parts.Length - 2);
					return new UpdateCommand
					{
						TodoList = todoList,
						TaskNumber = int.Parse(parts[1]),
						NewText = newText
					};

				case "exit":
					return new ExitCommand();

				default:
					return null;
			}
		}

		static AddCommand CreateAddCommand(string[] parts)
		{
			var command = new AddCommand { TodoList = todoList };

			for (int i = 1; i < parts.Length; i++)
			{
				if (!string.IsNullOrEmpty(parts[i]) &&
					(parts[i] == "--multiline" || parts[i] == "-m"))
				{
					command.MultilineMode = true;
					return command;
				}
			}

			if (parts.Length < 2)
			{
				Console.WriteLine("Ошибка: не указана задача");
				return null;
			}

			command.TaskText = string.Join(" ", parts, 1, parts.Length - 1);
			return command;
		}

		static ViewCommand CreateViewCommand(string[] parts)
		{
			var command = new ViewCommand { TodoList = todoList };

			for (int i = 1; i < parts.Length; i++)
			{
				string flag = parts[i];
				if (flag == "--all" || flag == "-a") command.ShowAll = true;
				else if (flag == "--index" || flag == "-i") command.ShowIndex = true;
				else if (flag == "--status" || flag == "-s") command.ShowStatus = true;
				else if (flag == "--update-date" || flag == "-d") command.ShowDate = true;
				else if (flag.StartsWith("-") && flag.Length > 1 && !flag.StartsWith("--"))
				{
					foreach (char c in flag.Substring(1))
					{
						switch (c)
						{
							case 'i': command.ShowIndex = true; break;
							case 's': command.ShowStatus = true; break;
							case 'd': command.ShowDate = true; break;
							case 'a': command.ShowAll = true; break;
						}
					}
				}
			}
			return command;
		}
	}
}