using System.Diagnostics;
using TodoList.Commands;

namespace TodoList;
internal class CommandParser
{
	//==========================================================================
	//	COMMANDS STRINGS
	//==========================================================================
	private const string CommandAddName = "add";
	private const string CommandProfileName = "profile";
	private const string CommandViewName = "view";
	private const string CommandExitName = "exit";
	private const string CommandHelpName = "help";
	private const string CommandDeleteName = "delete";
	private const string CommandUpdateName = "update";
	private const string CommandReadName = "read";
	private const string CommandStatusName = "status";
	private const string CommandUndoName = "undo";
	private const string CommandRedoName = "redo";
	private const string CommandSearchName = "search";

	private static string[] CommandAddMultilineFlags = new string[]
	{
		"--multiline",
		"-m"
	};

	private static string[] CommandViewIndexFlags = new string[]
	{
		"--index", "-i"
	};
	private static string[] CommandViewStatusFlags = new string[]
	{
		"--status", "-s"
	};
	private static string[] CommandViewUpdateFlags = new string[]
	{
		"--update-date", "-d"
	};
	private static string[] CommandViewAllFlags = new string[]
	{
		"--all", "-a"
	};
	private static string[] CommandProfileOutFlags = new string[]
	{
		"--out", "-o"
	};

	private static string[] CommandSearchFlags = new string[]
	{
		"--contains", "--start-with", "--end-with",
		"--from", "-to",
		"--status", "--sort text", "--sort date", "--desc", "--top"
	};
	//==========================================================================

	private static Dictionary<string, Func<string, ICommand>> _commandHandlers = new Dictionary<string, Func<string, ICommand>>();
	static CommandParser()
	{
		_commandHandlers["add"] = ParseAdd;
		_commandHandlers["delete"] = ParseDelete;
		_commandHandlers["help"] = ParseHelp;
		_commandHandlers["exit"] = ParseExit;
		_commandHandlers["profile"] = ParseProfile;
		_commandHandlers["undo"] = ParseUndo;
		_commandHandlers["redo"] = ParseRedo;
		_commandHandlers["none"] = ParseNone;
		_commandHandlers["uncorrect"] = ParseIncorrect;
		_commandHandlers["index"] = ParseIndex;
		_commandHandlers["read"] = ParseRead;
		_commandHandlers["status"] = ParseStatus;
		_commandHandlers["update"] = ParseUpdate;
		_commandHandlers["view"] = ParseView;
		_commandHandlers["search"] = ParseSearch;
	}

	public static ICommand Parse(string inputString)
	{
		if (string.IsNullOrWhiteSpace(inputString))
			return new NoneCommand();

		var parts = inputString.Trim().Split(' ', 2);
		string commandName = parts[0].ToLower();
		string arguments = parts.Length > 1 ? parts[1] : "";
		// Ищем обработчик в словаре
		if (_commandHandlers.TryGetValue(commandName, out var handler))
		{
			return handler(arguments);
		}
		// Если команда не найдена - парс как команду с индексом
		return ParseUnknownCommand(inputString);
	}
	private static ICommand ParseAdd(string arguments)
	{
		bool multiline = false;
		var userEnteredTask = arguments.Split(' ', 2);

		foreach (var i in CommandAddMultilineFlags)
		{
			if (arguments.Contains(i))
			{
				multiline = true;
				break;
			}
		}
		if (!multiline)
		{
			if (userEnteredTask.Length > 0 && IsTaskValidToAdd(userEnteredTask[0]))
			{
				return new AddCommand(multiline, userEnteredTask[0]);
			}
			else return GetUncorrect();
		}
		else
		{
			return new AddCommand(multiline, "");
		}
	}
	private static ICommand ParseDelete(string arguments)
	{
		if (int.TryParse(arguments.Trim(), out int index))
			return new DeleteCommand(index);
		return GetUncorrect();
	}

	private static ICommand ParseHelp(string arguments) => new HelpCommand();
	private static ICommand ParseExit(string arguments) => new ExitCommand();
	private static ICommand ParseProfile(string arguments) => new ProfileCommand(LineFlagsFounded("profile " + arguments, CommandProfileOutFlags));
	private static ICommand ParseUndo(string arguments) => new UndoCommand();
	private static ICommand ParseRedo(string arguments) => new RedoCommand();
	private static ICommand ParseNone(string arguments) => new NoneCommand();
	private static ICommand ParseIncorrect(string arguments) => new UncorrectCommand();
	private static ICommand ParseIndex(string arguments) => GetUncorrect(); // Заглушка
	private static ICommand ParseRead(string arguments) => new ReadCommand(ReadIndexFromCommand("read", "read " + arguments, false));
	private static ICommand ParseStatus(string arguments) => GetStatusCommand("status " + arguments);
	private static ICommand ParseUpdate(string arguments) => GetUpdateCommand("update " + arguments);
	private static ICommand ParseView(string arguments) => GetViewCommand("view " + arguments);
	private static ICommand ParseSearch(string arguments) => GetSearchCommand("search " + arguments);

	private static ICommand ParseUnknownCommand(string inputString)
	{
		if (CompareCommand(inputString, CommandExitName)) return new ExitCommand();
		if (CompareCommand(inputString, CommandUndoName)) return new UndoCommand();
		if (CompareCommand(inputString, CommandRedoName)) return new RedoCommand();
		else if (CompareCommand(inputString, CommandHelpName)) return new HelpCommand();
		else if (CompareCommand(inputString, CommandAddName)) return GetAddCommand(inputString);
		else if (CompareCommand(inputString, CommandStatusName)) return GetStatusCommand(inputString);
		else if (CompareCommand(inputString, CommandProfileName)) return GetProfileCommand(inputString);
		else if (CompareCommand(inputString, CommandViewName)) return GetViewCommand(inputString);
		else if (CompareCommand(inputString, CommandDeleteName)) return GetDeleteCommand(inputString);
		else if (CompareCommand(inputString, CommandUpdateName)) return GetUpdateCommand(inputString);
		else if (CompareCommand(inputString, CommandReadName)) return GetReadCommand(inputString);
		else if (CompareCommand(inputString, CommandSearchName)) return GetSearchCommand(inputString);
		return new NoneCommand();
	}

	private static bool CompareCommand(string inputString, string commandName) => inputString.StartsWith(commandName);

	private static ICommand GetUncorrect() => new UncorrectCommand();

	private static bool IsTaskValidToAdd(string task)
	{
		return task.Length > 0;
	}

	private static ICommand GetAddCommand(string inputString)
	{
		bool multiline = false;
		var userEnteredTask = inputString.Split(' ', 2);

		// checking for flags
		foreach (var i in CommandAddMultilineFlags)
		{
			if (userEnteredTask.Contains(i))
			{
				multiline = true;
			}
		}

		// execution
		if (!multiline)
		{
			if (userEnteredTask.Length > 1 && IsTaskValidToAdd(userEnteredTask[1]))
			{
				return new AddCommand(multiline, userEnteredTask[1]);
			}
			else return GetUncorrect();
		}
		else
		{
			return new AddCommand(multiline, "");
		}
	}

	private static bool LineFlagsFounded(string command, string[] flags)
	{
		bool flag = false;

		foreach (var i in flags)
		{
			if (command.Contains(i))
				flag = true;
		}

		return flag;
	}

	private static ICommand GetViewCommand(string inputString)
	{
		string[] userEnteredCommand = inputString.Split(' ');

		// checking for flags
		bool indexed = LineFlagsFounded(inputString, CommandViewIndexFlags);
		bool statused = LineFlagsFounded(inputString, CommandViewStatusFlags);
		bool update = LineFlagsFounded(inputString, CommandViewUpdateFlags);
		bool all = LineFlagsFounded(inputString, CommandViewAllFlags);

		// checking for multiflags
		foreach (var i in userEnteredCommand)
		{
			if (i.StartsWith("-"))
			{
				for (int j = 1; j < i.Length; j++)
				{
					if (i[j] == CommandViewIndexFlags[1][1])
						indexed = true;
					if (i[j] == CommandViewStatusFlags[1][1])
						statused = true;
					if (i[j] == CommandViewUpdateFlags[1][1])
						update = true;
					if (i[j] == CommandViewAllFlags[1][1])
						all = true;
				}
			}
		}

		if (all)
			indexed = statused = update = true;
		return new ViewCommand(indexed, statused, update);
	}

	private static int ReadIndexFromCommand(string commandName, string command, bool checkForNumOfTasks = true)
	{
		var items = command.Split(commandName);
		Debug.Assert(items.Length >= 2);

		bool indexValid = int.TryParse(items[1], out int index);
		Debug.Assert(indexValid);

		if (checkForNumOfTasks && (index < 0 || index >= AppInfo.CurrentTodoList.Length))
		{
			throw new Exception($"ReadIndexFromCommand: Uncorrected index {index}!");
		}

		return index;
	}

	private static ICommand GetDeleteCommand(string inputString) => new DeleteCommand(ReadIndexFromCommand(CommandDeleteName, inputString, false));
	private static ICommand GetReadCommand(string inputString) => new ReadCommand(ReadIndexFromCommand(CommandReadName, inputString, false));

	private static ICommand GetUpdateCommand(string inputString)
	{
		var args = inputString.Split(' ', 3);

		bool indexValid = int.TryParse(args[1], out int index);
		if (!indexValid)
		{
			Console.WriteLine($"{CommandUpdateName}: не верный индекс задачи.");
			return GetUncorrect();
		}

		if (index < 0 && index > AppInfo.CurrentTodoList.Length)
		{
			Console.WriteLine($"{CommandUpdateName}: задачи под номером {index} не существует.");
			return GetUncorrect();
		}

		if (args.Length > 2 && AppInfo.CurrentTodoList.IsValidIndex(index))
		{
			return new UpdateCommand(index, args[2]);
		}
		else
			return GetUncorrect();
	}

	// TODO: make status command
	private static ICommand GetStatusCommand(string inputString)
	{
		var args = inputString.Split(' ', 3);
		if (args.Length < 3)
		{
			Console.WriteLine("Ошибка: Команда требует индекс задачи и новый статус. Пример: status 0 InProgress");
			return GetUncorrect();
		}
		if (!int.TryParse(args[1], out int index))
		{
			Console.WriteLine($"Ошибка: '{args[1]}' не является корректным индексом задачи.");
			return GetUncorrect();
		}
		if (!Enum.TryParse(args[2], true, out TodoStatus newStatus))
		{
			Console.WriteLine($"Ошибка: '{args[2]}' не является корректным статусом. Доступные статусы: {string.Join(", ", Enum.GetNames(typeof(TodoStatus)))}");
			return GetUncorrect();
		}
		return new StatusCommand(index, newStatus);
	}

	private static ICommand GetProfileCommand(string inputString) => new ProfileCommand(LineFlagsFounded(inputString, CommandProfileOutFlags));

	private static ICommand GetSearchCommand(string inputString)
	{
		SearchCommand searchCommand = new SearchCommand();
		var args = inputString.Split(' ');

		if (args.Contains(CommandSearchFlags[0]))
		{
			int index = Array.IndexOf(args, CommandSearchFlags[0]) + 1;
			searchCommand.SetupContains(args[index]);
		}

		if (args.Contains(CommandSearchFlags[1]))
		{
			int index = Array.IndexOf(args, CommandSearchFlags[1]) + 1;
			searchCommand.SetupStartWith(args[index]);
		}

		if (args.Contains(CommandSearchFlags[2]))
		{
			int index = Array.IndexOf(args, CommandSearchFlags[2]) + 1;
			searchCommand.SetupEndWith(args[index]);
		}

		return searchCommand;
	}
}
