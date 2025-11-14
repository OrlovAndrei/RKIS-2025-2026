using System;
using TodoList;

namespace Todolist
{
	public static class CommandParser
	{
		public static ICommand Parse(string inputString, TodoList todoList, Profile profile, string todoFilePath, string profileFilePath)
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
					return new ProfileCommand
					{
						UserProfile = profile,
						ProfileFilePath = profileFilePath
					};

				case "add":
					return CreateAddCommand(parts, todoList, todoFilePath);

				case "view":
					return CreateViewCommand(parts, todoList);

				case "read":
					return CreateReadCommand(parts, todoList);

				case "done":
					return CreateDoneCommand(parts, todoList, todoFilePath);

				case "delete":
					return CreateDeleteCommand(parts, todoList, todoFilePath);

				case "update":
					return CreateUpdateCommand(parts, todoList, todoFilePath);
				case "status":
					return CreateStatusCommand(parts, todoList, todoFilePath);
				case "exit":
					return new ExitCommand();

				default:
					return null;
			}
		}

		private static AddCommand CreateAddCommand(string[] parts, TodoList todoList, string todoFilePath)
		{
			var command = new AddCommand
			{
				TodoList = todoList,
				TodoFilePath = todoFilePath
			};

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

		private static ViewCommand CreateViewCommand(string[] parts, TodoList todoList)
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

		private static DoneCommand CreateDoneCommand(string[] parts, TodoList todoList, string todoFilePath)
		{
			if (parts.Length < 2)
			{
				Console.WriteLine("Ошибка: не указан номер задачи");
				return null;
			}

			if (int.TryParse(parts[1], out int taskNumber))
			{
				return new DoneCommand
				{
					TodoList = todoList,
					TaskNumber = taskNumber,
					TodoFilePath = todoFilePath
				};
			}
			else
			{
				Console.WriteLine("Ошибка: неверный номер задачи");
				return null;
			}
		}

		private static DeleteCommand CreateDeleteCommand(string[] parts, TodoList todoList, string todoFilePath)
		{
			if (parts.Length < 2)
			{
				Console.WriteLine("Ошибка: не указан номер задачи");
				return null;
			}

			if (int.TryParse(parts[1], out int taskNumber))
			{
				return new DeleteCommand
				{
					TodoList = todoList,
					TaskNumber = taskNumber,
					TodoFilePath = todoFilePath
				};
			}
			else
			{
				Console.WriteLine("Ошибка: неверный номер задачи");
				return null;
			}
		}

		private static UpdateCommand CreateUpdateCommand(string[] parts, TodoList todoList, string todoFilePath)
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
					NewText = newText,
					TodoFilePath = todoFilePath
				};
			}
			else
			{
				Console.WriteLine("Ошибка: неверный номер задачи");
				return null;
			}
		}
		private static StatusCommand CreateStatusCommand(string[] parts, TodoList todoList, string todoFilePath)
		{
			if (parts.Length < 3)
			{
				Console.WriteLine("Ошибка: синтаксис: status <номер> <статус>");
				return null;
			}

			if (!int.TryParse(parts[1], out int taskNumber))
			{
				Console.WriteLine("Ошибка: неверный номер задачи");
				return null;
			}

			string statusString = parts[2];
			// Собираем оставшиеся части, если статус содержит дефис/underscore или пробелы (на всякий пожарный)
			if (parts.Length > 3)
				statusString = string.Join(" ", parts, 2, parts.Length - 2);

			if (!TryParseStatus(statusString, out TodoStatus newStatus))
			{
				Console.WriteLine($"Ошибка: неизвестный статус '{statusString}'");
				return null;
			}

			return new StatusCommand
			{
				TodoList = todoList,
				TaskNumber = taskNumber,
				NewStatus = newStatus,
				TodoFilePath = todoFilePath
			};
		}
		private static bool TryParseStatus(string input, out TodoStatus status)
		{
			status = TodoStatus.NotStarted;
			if (string.IsNullOrWhiteSpace(input)) return false;

			string s = input.Trim().ToLower().Replace("-", "").Replace("_", "").Replace(" ", "");

			// Простые соответствия и распространённые опечатки
			switch (s)
			{
				case "notstarted":
				case "not":
				case "notstart":
					status = TodoStatus.NotStarted; return true;

				case "inprogress":
				case "in":
				case "progress":
					status = TodoStatus.InProgress; return true;

				case "completed":
				case "complete":
				case "done":
				case "complited": // возможная опечатка
				case "compl":
					status = TodoStatus.Completed; return true;

				case "postponed":
				case "postpone":
				case "later":
					status = TodoStatus.Postponed; return true;

				case "failed":
				case "fail":
					status = TodoStatus.Failed; return true;
			}

			// Попробовать стандартный парсинг enum по имени
			if (Enum.TryParse<TodoStatus>(input, true, out var parsed))
			{
				status = parsed;
				return true;
			}

			return false;
		}
	}
}