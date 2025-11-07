using System;

namespace Todolist
{
	public static class CommandParser
	{
		public static ICommand Parse(string inputString, TodoList todoList, Profile profile)
		{
			if (string.IsNullOrWhiteSpace(inputString))
				return null;

			string[] parts = inputString.Split(' ');
			string commandName = parts[0].ToLower();

			switch (commandName)
			{
				case "help":
					return new HelpCommand();

				case "profile":
					return new ProfileCommand { UserProfile = profile };

				case "add":
					return CreateAddCommand(parts, todoList);

				case "view":
					return CreateViewCommand(parts, todoList);

				case "read":
					return CreateReadCommand(parts, todoList);

				case "done":
					return CreateDoneCommand(parts, todoList);

				case "delete":
					return CreateDeleteCommand(parts, todoList);

				case "update":
					return CreateUpdateCommand(parts, todoList);

				case "exit":
					return new ExitCommand();

				default:
					return null;
			}
		}

		private static AddCommand CreateAddCommand(string[] parts, TodoList todoList)
		{
			var command = new AddCommand { TodoList = todoList };

			// Проверяем флаги многострочного режима
			for (int i = 1; i < parts.Length; i++)
			{
				if (!string.IsNullOrEmpty(parts[i]) &&
					(parts[i] == "--multiline" || parts[i] == "-m"))
				{
					command.MultilineMode = true;
					return command;
				}
			}

			// Если не многострочный режим, извлекаем текст задачи
			if (parts.Length < 2)
			{
				Console.WriteLine("Ошибка: не указана задача");
				return null;
			}

			command.TaskText = string.Join(" ", parts, 1, parts.Length - 1);
			return command;
		}

		private static ViewCommand CreateViewCommand(string[] parts, TodoList todoList)
		{
			var command = new ViewCommand { TodoList = todoList };

			// Обрабатываем флаги отображения
			for (int i = 1; i < parts.Length; i++)
			{
				string flag = parts[i];
				if (flag == "--all" || flag == "-a") command.ShowAll = true;
				else if (flag == "--index" || flag == "-i") command.ShowIndex = true;
				else if (flag == "--status" || flag == "-s") command.ShowStatus = true;
				else if (flag == "--update-date" || flag == "-d") command.ShowDate = true;
				else if (flag.StartsWith("-") && flag.Length > 1 && !flag.StartsWith("--"))
				{
					// Обработка комбинированных флагов (-is, -isd и т.д.)
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

		private static ReadCommand CreateReadCommand(string[] parts, TodoList todoList)
		{
			if (parts.Length < 2)
			{
				Console.WriteLine("Ошибка: не указан номер задачи");
				return null;
			}

			if (int.TryParse(parts[1], out int taskNumber))
			{
				return new ReadCommand { TodoList = todoList, TaskNumber = taskNumber };
			}
			else
			{
				Console.WriteLine("Ошибка: неверный номер задачи");
				return null;
			}
		}

		private static DoneCommand CreateDoneCommand(string[] parts, TodoList todoList)
		{
			if (parts.Length < 2)
			{
				Console.WriteLine("Ошибка: не указан номер задачи");
				return null;
			}

			if (int.TryParse(parts[1], out int taskNumber))
			{
				return new DoneCommand { TodoList = todoList, TaskNumber = taskNumber };
			}
			else
			{
				Console.WriteLine("Ошибка: неверный номер задачи");
				return null;
			}
		}

		private static DeleteCommand CreateDeleteCommand(string[] parts, TodoList todoList)
		{
			if (parts.Length < 2)
			{
				Console.WriteLine("Ошибка: не указан номер задачи");
				return null;
			}

			if (int.TryParse(parts[1], out int taskNumber))
			{
				return new DeleteCommand { TodoList = todoList, TaskNumber = taskNumber };
			}
			else
			{
				Console.WriteLine("Ошибка: неверный номер задачи");
				return null;
			}
		}

		private static UpdateCommand CreateUpdateCommand(string[] parts, TodoList todoList)
		{
			if (parts.Length < 3)
			{
				Console.WriteLine("Ошибка: не указан номер задачи или новый текст");
				return null;
			}

			if (int.TryParse(parts[1], out int taskNumber))
			{
				string newText = string.Join(" ", parts, 2, parts.Length - 2);
				return new UpdateCommand
				{
					TodoList = todoList,
					TaskNumber = taskNumber,
					NewText = newText
				};
			}
			else
			{
				Console.WriteLine("Ошибка: неверный номер задачи");
				return null;
			}
		}
	}
}