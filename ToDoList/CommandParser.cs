using System;
using TodoList.Commands;
using ToDoList;

namespace TodoList
{
	static class CommandParser
	{
		public static ICommand Parse(string inputString, TodoList todoList, Profile profile)
		{
			if (string.IsNullOrEmpty(inputString))
				return null;

			string command = inputString.Trim().ToLower();

			if (command == "help")
				return new HelpCommand();

			if (command == "undo")
				return new UndoCommand();

			if (command == "redo")
				return new RedoCommand();

			if (command == "profile")
			return new ProfileCommand { Profile = profile };

			if (command == "view")
				return new ViewCommand { TodoList = AppInfo.Todos };

			if (command.StartsWith("view "))
			{
				var viewCommand = new ViewCommand { TodoList = AppInfo.Todos };
				ParseViewFlags(inputString, viewCommand);
				return viewCommand;
			}

			if (command.StartsWith("add"))
			{
				var addCommand = new AddCommand { TodoList = AppInfo.Todos };
				ParseAddCommand(inputString, addCommand);
				return addCommand;
			}

			if (command.StartsWith("delete "))
			{
				var deleteCommand = new DeleteCommand { TodoList = AppInfo.Todos };
				if (ParseTaskIndex(inputString, out int index))
					deleteCommand.TaskIndex = index;
				return deleteCommand;
			}

			if (command.StartsWith("update "))
			{
				var updateCommand = new UpdateCommand { TodoList = AppInfo.Todos };
				if (ParseUpdateCommand(inputString, updateCommand))
					return updateCommand;
			}

			if (command.StartsWith("read "))
			{
				var readCommand = new ReadCommand { TodoList = AppInfo.Todos };
				if (ParseTaskIndex(inputString, out int index))
					readCommand.TaskIndex = index;
				return readCommand;
			}

			if (command == "status")
			{
				return new StatusCommand { TodoList = AppInfo.Todos };
			}

			if (command.StartsWith("status "))
			{
				var statusCommand = new StatusCommand { TodoList = AppInfo.Todos };
				if (ParseStatusCommand(inputString, statusCommand))
					return statusCommand;
			}
			if (command.StartsWith("search"))
			{
				var searchCommand = new SearchCommand { TodoList = AppInfo.Todos };
				ParseSearchCommand(inputString, searchCommand);
				return searchCommand;
			}
			Console.WriteLine("Неизвестная команда. Введите help для справки.");
			return null;
		}

		private static void ParseViewFlags(string input, ViewCommand command)
		{
			command.ShowIndex = input.Contains("--index") || input.Contains("-i");
			command.ShowStatus = input.Contains("--status") || input.Contains("-s");
			command.ShowDate = input.Contains("--update-date") || input.Contains("-d");

			if (input.Contains("--all") || input.Contains("-a"))
			{
				command.ShowIndex = true;
				command.ShowStatus = true;
				command.ShowDate = true;
			}
		}

		private static void ParseAddCommand(string input, AddCommand command)
		{
			command.IsMultiline = input.Contains("--multiline") || input.Contains("-m");
			command.IsUrgent = input.Contains("--urgent") || input.Contains("-u");

			if (!command.IsMultiline)
			{
				string[] parts = input.Split('"');
				if (parts.Length >= 2)
				{
					command.TaskText = parts[1];
				}
			}

			if (!command.IsMultiline && string.IsNullOrEmpty(command.TaskText))
			{
				string taskText = input.Substring(3).Trim();
				if (!string.IsNullOrEmpty(taskText))
				{
					command.TaskText = taskText.Trim('"');
				}
			}
		}

		private static bool ParseTaskIndex(string input, out int index)
		{
			index = -1;
			string[] parts = input.Split(' ');
			if (parts.Length >= 2 && int.TryParse(parts[1], out int parsedIndex))
			{
				index = parsedIndex - 1;
				return true;
			}
			return false;
		}

		private static bool ParseUpdateCommand(string input, UpdateCommand command)
		{
			string[] parts = input.Split(' ', 3);
			if (parts.Length >= 3 && int.TryParse(parts[1], out int index))
			{
				command.TaskIndex = index - 1;
				command.NewText = parts[2].Trim('"');
				return true;
			}
			return false;
		}

		private static bool ParseStatusCommand(string input, StatusCommand command)
		{
			string[] parts = input.Split(' ');
			if (parts.Length >= 3 && int.TryParse(parts[1], out int index))
			{
				command.TaskIndex = index - 1;

				string statusStr = parts[2].ToLower();
				command.NewStatus = statusStr switch
				{
					"not_started" => TodoStatus.NotStarted,
					"in_progress" => TodoStatus.InProgress,
					"completed" => TodoStatus.Completed,
					"postponed" => TodoStatus.Postponed,
					"failed" => TodoStatus.Failed,
				};
				return true;
			}
			return false;
		}
		private static bool ParseSearchCommand(string input, SearchCommand command)
		{
			var parts = input.Split(' ');

			for (int i = 1; i < parts.Length; i++)
			{
				string arg = parts[i];

				if (arg == "--contains" && i + 1 < parts.Length)
				{
					command.ContainsText = parts[++i].Trim('"');
				}
				else if (arg == "--starts-with" && i + 1 < parts.Length)
				{
					command.StartsWithText = parts[++i].Trim('"');
				}
				else if (arg == "--ends-with" && i + 1 < parts.Length)
				{
					command.EndsWithText = parts[++i].Trim('"');
				}
				else if (arg == "--status" && i + 1 < parts.Length)
				{
					command.Status = parts[++i].ToLower();
				}
				else if (arg == "--from" && i + 1 < parts.Length)
				{
					if (DateTime.TryParseExact(parts[++i], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var date))
						command.FromDate = date;
				}
				else if (arg == "--to" && i + 1 < parts.Length)
				{
					if (DateTime.TryParseExact(parts[++i], "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var date))
						command.ToDate = date;
				}
				else if (arg == "--sort" && i + 1 < parts.Length)
				{
					command.SortBy = parts[++i];
				}
				else if (arg == "--desc")
				{
					command.Descending = true;
				}
				else if (arg == "--top" && i + 1 < parts.Length)
				{
					if (int.TryParse(parts[++i], out int top))
						command.TopCount = top;
				}
			}

			return true;
		}
	}
}