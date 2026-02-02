using System;
using TodoApp.Commands;
namespace TodoApp.Commands
{
	public static class CommandParser
    {
        private static readonly Dictionary<string, Func<string, TodoList, Guid?, BaseCommand>> _commandHandlers = 
            new Dictionary<string, Func<string, TodoList, Guid?, BaseCommand>>();
        static CommandParser()
        {
            InitializeCommandHandlers();
        }
		public static BaseCommand Parse(string input, Guid? currentProfileId, TodoList todoList)
		{
			if (string.IsNullOrWhiteSpace(input))
				return new ErrorCommand("Пустая команда.");

			string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length == 0)
				return new ErrorCommand("Неверная команда.");

			string commandName = parts[0].ToLower();

			if (_commandHandlers.TryGetValue(commandName, out var handler))
			{
				return handler(input, todoList, currentProfileId);
			}
			else
			{
				return new ErrorCommand($"Неизвестная команда: {commandName}");
			}
		}
		private static void InitializeCommandHandlers()
        {
            _commandHandlers["add"] = (input, todoList, currentProfileId) => 
                ParseAddCommand(input, todoList, currentProfileId);
            
            _commandHandlers["delete"] = (input, todoList, currentProfileId) => 
                ParseDeleteCommand(input, todoList, currentProfileId);
            
            _commandHandlers["read"] = (input, todoList, currentProfileId) =>
                ParseReadCommand(input, todoList, currentProfileId);

            _commandHandlers["status"] = (input, todoList, currentProfileId) => 
                ParseStatusCommand(input, todoList, currentProfileId);
            
            _commandHandlers["view"] = (input, todoList, currentProfileId) => 
                ParseViewCommand(input, todoList, currentProfileId);
            
            _commandHandlers["profile"] = (input, todoList, currentProfileId) => 
                ParseProfileCommand(input);
            
            _commandHandlers["undo"] = (input, todoList, currentProfileId) => 
                new UndoCommand();
            
            _commandHandlers["update"] = (input, todoList, currentProfileId) => 
                ParseUpdateCommand(input, todoList, currentProfileId);
            
            _commandHandlers["redo"] = (input, todoList, currentProfileId) => 
                new RedoCommand();
            
            _commandHandlers["exit"] = (input, todoList, currentProfileId) => 
                new ExitCommand();
            
            _commandHandlers["help"] = (input, todoList, currentProfileId) => 
                new HelpCommand();
        }
        private static BaseCommand ParseDeleteCommand(string input, TodoList todoList, Guid? currentProfileId)
        {
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 1 && int.TryParse(parts[1], out int indexDelete))
                return new DeleteCommand(todoList, indexDelete - 1, currentProfileId);
            else
                return new ErrorCommand("Неверный номер задачи.");
        }
        private static BaseCommand ParseReadCommand(string input, TodoList todoList, Guid? currentProfileId)
        {
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 1 && int.TryParse(parts[1], out int index))
            {
                return new ReadCommand(index - 1);
            }
            else
            {
                return new ErrorCommand("Неверный номер задачи для чтения.");
            }
        }
        private static BaseCommand ParseStatusCommand(string input, TodoList todoList, Guid? currentProfileId)
        {
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 3 && int.TryParse(parts[1], out int indexStatus))
            {
                var parsedStatus = ParseStatus(parts[2]);
                if (parsedStatus.HasValue)
                    return new StatusCommand(todoList, indexStatus - 1, parsedStatus.Value, currentProfileId);
                else
                    return new ErrorCommand("Неверный статус задачи.");
            }
            else
            {
                return new ErrorCommand("Неверный формат команды status.");
            }
        }
        private static BaseCommand ParseProfileCommand(string input)
        {
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 1 && parts[1] == "--out")
            {
                var command = new ProfileCommand();
                command.SaveToFile = true;
                return command;
            }
            else
            {
                return new ProfileCommand();
            }
        }
        private static BaseCommand ParseUpdateCommand(string input, TodoList todoList, Guid? currentProfileId)
        {
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 3 && int.TryParse(parts[1], out int index))
            {
                int startIndex = input.IndexOf("update") + 6;
                string remainingText = input.Substring(startIndex).Trim();
                string[] remainingParts = remainingText.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                if (remainingParts.Length >= 2)
                {
                    string newText = remainingParts[1].Trim(' ', '"');
                    return new UpdateCommand(index - 1, newText);
                }
            }
            return new ErrorCommand("Неверный формат команды update. Используйте: update <номер> <новый текст>");
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
        public static void RegisterCommand(string commandName, Func<string, TodoList, Guid?, BaseCommand> handler)
        {
            if (!_commandHandlers.ContainsKey(commandName.ToLower()))
            {
                _commandHandlers[commandName.ToLower()] = handler;
            }
        }
    }
}