using System;
using System.Collections.Generic;
using TodoApp.Exceptions;
public static class CommandParser
{
	private delegate ICommand CommandHandler(string args);
	private static readonly Dictionary<string, CommandHandler> _commandHandlers;
	static CommandParser()
	{
		_commandHandlers = new Dictionary<string, CommandHandler>
		{
			["help"] = ParseHelp,
			["profile"] = ParseProfile,
			["out"] = ParseLogout,
			["read"] = ParseRead,
			["add"] = ParseAdd,
			["view"] = ParseView,
			["status"] = ParseStatus,
			["delete"] = ParseDelete,
			["update"] = ParseUpdate,
			["undo"] = ParseUndo,
			["redo"] = ParseRedo,
			["search"] = ParseSearch,
			["load"] = ParseLoad,
			["sync"] = ParseSync
		};
	}
	public static ICommand Parse(string inputString)
	{
		if (string.IsNullOrWhiteSpace(inputString)) return null;
		var parts = inputString.Trim().Split(' ', 2);
		var commandKey = parts[0].ToLower();
		var args = parts.Length > 1 ? parts[1] : "";
		if (_commandHandlers.TryGetValue(commandKey, out var handler))
		{
			return handler(args);
		}
		throw new InvalidCommandException($"Неизвестная команда: {commandKey}");
	}
	private static ICommand ParseLoad(string args)
	{
		if (string.IsNullOrWhiteSpace(args))
		{
			throw new InvalidArgumentException("Команда load требует аргументы. Использование: load <количество> <размер>");
		}
		var parts = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length < 2)
		{
			throw new InvalidArgumentException("Недостаточно аргументов. Необходимо указать количество загрузок и их размер.");
		}
		if (!int.TryParse(parts[0], out int count))
		{
			throw new InvalidArgumentException($"Первый аргумент '{parts[0]}' должен быть целым числом.");
		}
		if (!int.TryParse(parts[1], out int size))
		{
			throw new InvalidArgumentException($"Второй аргумент '{parts[1]}' должен быть целым числом.");
		}
		if (count <= 0)
		{
			throw new InvalidArgumentException("Количество загрузок должно быть больше нуля.");
		}
		if (size <= 0)
		{
			throw new InvalidArgumentException("Размер загрузки должен быть больше нуля.");
		}
		return new LoadCommand { Count = count, Size = size };
	}
	private static ICommand ParseAdd(string args)
	{
		var command = new AddCommand { Todos = AppInfo.CurrentUserTodos, UserId = AppInfo.CurrentProfileId, DataDir = AppInfo.DataDir };
		if (args.Contains("-m") || args.Contains("--multiline"))
		{
			command.Multiline = true;
		}
		else
		{
			string[] textParts = args.Split('\"');
			if (textParts.Length >= 2)
			{
				command.TaskText = textParts[1];
			}
			else
			{
				throw new ArgumentException("Неверный формат команды add. Используйте: add \"текст задачи\"");
			}
		}
		return command;
	}
	private static ICommand ParseDelete(string args)
	{
		if (string.IsNullOrWhiteSpace(args) || !int.TryParse(args.Trim(), out int taskId))
		{
			throw new InvalidArgumentException("Индекс задачи должен быть целым числом.");
		}
		return new DeleteCommand { Todos = AppInfo.CurrentUserTodos, TaskIndex = taskId, UserId = AppInfo.CurrentProfileId, DataDir = AppInfo.DataDir };
	}
	private static ICommand ParseHelp(string args) => new HelpCommand();
	private static ICommand ParseUndo(string args) => new UndoCommand();
	private static ICommand ParseRedo(string args) => new RedoCommand();
	private static ICommand ParseSearch(string args)
	{
		var command = new SearchCommand { Todos = AppInfo.CurrentUserTodos };
		return command;
	}
	private static ICommand ParseView(string args)
	{
		var command = new ViewCommand { Todos = AppInfo.CurrentUserTodos };
		return command;
	}
	private static ICommand ParseUpdate(string args)
	{
		string[] textParts = args.Split('\"');
		if (textParts.Length < 2)
		{
			throw new InvalidArgumentException("Ошибка: Используйте формат: update [индекс] \"новый текст\"");
		}
		if (!int.TryParse(textParts[0].Trim(), out int taskId))
		{
			throw new InvalidArgumentException("Ошибка: Некорректный индекс задачи.");
		}
		return new UpdateCommand { Todos = AppInfo.CurrentUserTodos, TaskIndex = taskId, NewText = textParts[1], UserId = AppInfo.CurrentProfileId, DataDir = AppInfo.DataDir };
	}
	private static ICommand ParseRead(string args)
	{
		if (string.IsNullOrWhiteSpace(args) || !int.TryParse(args.Trim(), out int taskId))
		{
			throw new ArgumentException("Неверный формат команды read. Используйте: read индекс");
		}
		return new ReadCommand { Todos = AppInfo.CurrentUserTodos, TaskIndex = taskId, UserId = AppInfo.CurrentProfileId };
	}
	private static ICommand ParseStatus(string args)
	{
		string[] parts = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length < 2)
		{
			throw new ArgumentException("Неверный формат команды status. Используйте: status индекс статус");
		}
		if (!int.TryParse(parts[0], out int taskId))
		{
			throw new ArgumentException("Неверный индекс задачи");
		}
		if (!Enum.TryParse<TodoStatus>(parts[1], true, out TodoStatus status))
		{
			throw new ArgumentException($"Неверный статус. Допустимые значения: {string.Join(", ", Enum.GetNames(typeof(TodoStatus)))}");
		}
		return new StatusCommand
		{
			Todos = AppInfo.CurrentUserTodos,
			TaskIndex = taskId,
			NewStatus = status,
			UserId = AppInfo.CurrentProfileId,
			DataDir = AppInfo.DataDir
		};
	}
	private static ICommand ParseSync(string args)
	{
		return new SyncCommand();
	}
	private static ICommand ParseProfile(string args)
	{
		return new ProfileCommand { UserProfile = AppInfo.CurrentProfile, LogoutFlag = false };
	}
	private static ICommand ParseLogout(string args)
	{
		return new ProfileCommand { UserProfile = AppInfo.CurrentProfile, LogoutFlag = true };
	}
}