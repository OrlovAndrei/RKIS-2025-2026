using System;
using TodoApp.Commands;
namespace TodoApp.Commands
{
	public static class CommandParser
	{
		public static BaseCommand Parse(string inputString, Guid? currentProfileId, TodoList todoList)
		{
			if (string.IsNullOrWhiteSpace(inputString))
				return new HelpCommand();

			string[] parts = inputString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length == 0)
				return new HelpCommand();

			string commandName = parts[0].ToLower();

			switch (commandName)
			{
				case "add":
					return ParseAddCommand(inputString, todoList, currentProfileId);

				case "delete":
					if (parts.Length > 1 && int.TryParse(parts[1], out int indexDelete))
						return new DeleteCommand(todoList, indexDelete - 1, currentProfileId);
					else
						return new ErrorCommand("Неверный номер задачи.");

				case "status":
					if (parts.Length >= 3 && int.TryParse(parts[1], out int indexStatus))
					{
						var parsedStatus = ParseStatus(parts[2]);
						if (parsedStatus.HasValue)
							return new StatusCommand(todoList, indexStatus - 1, parsedStatus.Value, currentProfileId);
						else
							return new ErrorCommand("Неверный статус задачи.");
					}
					else
						return new ErrorCommand("Неверный формат команды status.");

				case "view":
					return ParseViewCommand(inputString, todoList, currentProfileId);
				case "profile":
					if (inputString.Contains("--out"))
					{
						var cmd = new ProfileCommand();
						cmd.SaveToFile = true;
						return cmd;
					}
					else
					{
						return new ProfileCommand();
					}
				case "undo":
					return new UndoCommand();
				case "update":
					if (parts.Length >= 3 && int.TryParse(parts[1], out int index))
					{
						string newText = parts[2];
						return new UpdateCommand(index - 1, newText);
					}
					else
					{
						return new ErrorCommand("Неверный формат команды update. Используйте: update <номер> <новый текст>");
					}
				case "redo":
					return new RedoCommand();

				case "exit":
					return new ExitCommand();

				case "help":
					return new HelpCommand();

				default:
					return new ErrorCommand($"Неизвестная команда: {commandName}");
			}
		}
		private static BaseCommand ParseAddCommand(string input, TodoList todoList, Guid? currentProfileId)
		{
			bool multiline = input.Contains("--multiline") || input.Contains("-m");
			string taskText = "";

			if (!multiline)
			{
				int startIndex = input.IndexOf("add") + 3;
				if (startIndex < input.Length)
					taskText = input.Substring(startIndex).Trim(' ', '"');
			}

			return new AddCommand(todoList, taskText, multiline, currentProfileId);
		}

		private static BaseCommand ParseViewCommand(string input, TodoList todoList, Guid? currentProfileId)
		{
			var command = new ViewCommand(todoList, currentProfileId);
			command.ShowIndex = input.Contains("--index") || input.Contains("-i");
			command.ShowStatus = input.Contains("--status") || input.Contains("-s");
			command.ShowDate = input.Contains("--date") || input.Contains("-d");
			command.ShowAll = input.Contains("--all") || input.Contains("-a");
			return command;
		}
		private static TodoStatus? ParseStatus(string statusString)
		{
			return statusString.ToLower() switch
			{
				"notstarted" or "not_started" or "not" => TodoStatus.NotStarted,
				"inprogress" or "in_progress" or "progress" => TodoStatus.InProgress,
				"completed" or "complete" or "done" => TodoStatus.Completed,
				"postponed" or "postpone" => TodoStatus.Postponed,
				"failed" or "fail" => TodoStatus.Failed,
				_ => null
			};
		}
	}
}