using TodoApp.Exceptions;
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
				 throw new InvalidCommandException("Пустая команда.");

			string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length == 0)
				throw new InvalidCommandException("Неверная команда.");

			string commandName = parts[0].ToLower();

			if (_commandHandlers.TryGetValue(commandName, out var handler))
			{
				return handler(input, todoList, currentProfileId);
			}
			else
			{
				throw new InvalidCommandException($"Неизвестная команда: {commandName}");
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

            _commandHandlers["search"] = (input, todoList, currentProfileId) => 
                ParseSearchCommand(input, todoList, currentProfileId);

			_commandHandlers["load"] = (input, todoList, currentProfileId) =>
			{
				string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

				if (parts.Length != 3)
				{
					throw new LoadCommandException("Неверный формат команды load. Используйте: load <количество_скачиваний> <размер_скачиваний>");
				}

				if (!int.TryParse(parts[1], out int downloadsCount))
				{
					throw new LoadCommandException("Количество скачиваний должно быть числом.");
				}
				if (!int.TryParse(parts[2], out int downloadSize))
				{
					throw new LoadCommandException("Размер скачивания должен быть числом.");
				}

				return new LoadCommand(downloadsCount, downloadSize);
			};
		}
        private static BaseCommand ParseDeleteCommand(string input, TodoList todoList, Guid? currentProfileId)
        {
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 1 && int.TryParse(parts[1], out int indexDelete))
            {
                if (indexDelete < 1 || indexDelete > todoList.Count)
                {
                    throw new InvalidArgumentException($"Неверный номер задачи: {indexDelete}. Допустимый диапазон: 1-{todoList.Count}");
                }
                return new DeleteCommand(todoList, indexDelete - 1, currentProfileId);
            }
            else
            {
                throw new InvalidCommandException("Неверный формат команды delete. Используйте: delete <номер>");
            }
        }
        private static BaseCommand ParseReadCommand(string input, TodoList todoList, Guid? currentProfileId)
        {
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 1 && int.TryParse(parts[1], out int index))
            {
                if (index < 1 || index > todoList.Count)
                {
                    throw new InvalidArgumentException($"Неверный номер задачи: {index}. Допустимый диапазон: 1-{todoList.Count}");
                }
                return new ReadCommand(index - 1);
            }
            else
            {
                throw new InvalidCommandException("Неверный формат команды read. Используйте: read <номер>");
            }
        }
        private static BaseCommand ParseStatusCommand(string input, TodoList todoList, Guid? currentProfileId)
        {
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 3 && int.TryParse(parts[1], out int indexStatus))
            {
                if (indexStatus < 1 || indexStatus > todoList.Count)
                {
                    throw new InvalidArgumentException($"Неверный номер задачи: {indexStatus}. Допустимый диапазон: 1-{todoList.Count}");
                }
                var parsedStatus = ParseStatus(parts[2]);
                if (parsedStatus.HasValue)
                {
                    return new StatusCommand(todoList, indexStatus - 1, parsedStatus.Value, currentProfileId);
                }
                else
                {
                    throw new InvalidArgumentException($"Неверный статус задачи: {parts[2]}. Допустимые значения: notstarted, inprogress, completed, postponed, failed");
                }
            }
            else
            {
                throw new InvalidCommandException("Неверный формат команды status. Используйте: status <номер> <статус>");
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
            string[] validFlags = { "--index", "-i", "--status", "-s", "--date", "-d", "--all", "-a" };
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < parts.Length; i++)
            {
                if (parts[i].StartsWith("--") || parts[i].StartsWith("-"))
                {
                    if (!validFlags.Contains(parts[i].ToLower()))
                    {
                        throw new InvalidArgumentException($"Неизвестный флаг: {parts[i]}");
                    }
                }
            }
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
        private static BaseCommand ParseSearchCommand(string input, TodoList todoList, Guid? currentProfileId)
		{
			try
			{
				var command = new SearchCommand(todoList, currentProfileId);

				string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				string[] validFlags = { 
                    "--contains", "--starts-with", "--ends-with", "--from", "--to", 
                    "--status", "--sort", "--desc", "--top" 
                };
                for (int i = 0; i < parts.Length; i++)
                {
                    string part = parts[i];
                    if (part.StartsWith("--") && !validFlags.Contains(part.ToLower()))
                    {
                        throw new InvalidArgumentException($"Неизвестный флаг в команде search: {part}");
                    }
                    switch (part.ToLower())
                    {
                        case "--contains":
                            if (i + 1 < parts.Length)
                                command.ContainsText = ExtractTextArgument(parts, ref i);
                            break;
                            
                        case "--starts-with":
                            if (i + 1 < parts.Length)
                                command.StartsWithText = ExtractTextArgument(parts, ref i);
                            break;
                            
                        case "--ends-with":
                            if (i + 1 < parts.Length)
                                command.EndsWithText = ExtractTextArgument(parts, ref i);
                            break;
                            
                        case "--from":
                            if (i + 1 < parts.Length)
                            {
                                if (!DateTime.TryParse(parts[i + 1], out DateTime fromDate))
                                {
                                    throw new InvalidArgumentException($"Некорректный формат даты: {parts[i + 1]}. Используйте формат: ГГГГ-ММ-ДД");
                                }
                                command.FromDate = fromDate;
                                i++;
                            }
                            break;
                            
                        case "--to":
                            if (i + 1 < parts.Length)
                            {
                                if (!DateTime.TryParse(parts[i + 1], out DateTime toDate))
                                {
                                    throw new InvalidArgumentException($"Некорректный формат даты: {parts[i + 1]}. Используйте формат: ГГГГ-ММ-ДД");
                                }
                                command.ToDate = toDate;
                                i++;
                            }
                            break;
                            
                        case "--status":
                            if (i + 1 < parts.Length)
                            {
                                var status = ParseStatus(parts[i + 1]);
                                if (!status.HasValue)
                                {
                                    throw new InvalidArgumentException($"Неверный статус: {parts[i + 1]}. Допустимые значения: notstarted, inprogress, completed, postponed, failed");
                                }
                                command.StatusFilter = status.Value;
                                i++;
                            }
                            break;

                        case "--sort":
                            if (i + 1 < parts.Length)
                            {
                                command.SortBy = parts[i + 1];
                                i++;
                            }
                            break;
                            
                        case "--desc":
                            command.SortDescending = true;
                            break;
                            
                        case "--top":
                            if (i + 1 < parts.Length)
                            {
                                if (!int.TryParse(parts[i + 1], out int top) || top <= 0)
                                {
                                    throw new InvalidArgumentException($"Некорректное значение top: {parts[i + 1]}. Должно быть положительное число.");
                                }
                                command.TopCount = top;
                                i++;
                            }
                            break;
                    }
                }
                return command;
            }
            catch (InvalidArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при парсинге команды search: {ex.Message}");
                return new ErrorCommand("Ошибка при обработке команды search");
            }
        }

private static string ExtractTextArgument(string[] parts, ref int index)
{
    string arg = parts[index + 1];
    
    if (arg.StartsWith("\""))
    {
        var textParts = new List<string>();
        textParts.Add(arg.Trim('"'));
        
        int j = index + 2;
        while (j < parts.Length && !parts[j].EndsWith("\""))
        {
            textParts.Add(parts[j]);
            j++;
        }
        
        if (j < parts.Length)
        {
            textParts.Add(parts[j].Trim('"'));
            index = j; 
        }
        
        return string.Join(" ", textParts);
    }
    
    index++; 
    return arg.Trim('"');
}
    }
}