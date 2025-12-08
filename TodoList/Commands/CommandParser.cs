using System;
using System.Collections.Generic;

namespace TodoList.Commands
{
	public static class CommandParser
	{
		private static readonly Dictionary<string, Func<string, string[], ICommand>> _commandHandlers;

		static CommandParser()
		{
			_commandHandlers = new Dictionary<string, Func<string, string[], ICommand>>
			{
				["add"] = ParseAddCommand,
				["view"] = ParseViewCommand,
				["delete"] = ParseDeleteCommand,
				["update"] = ParseUpdateCommand,
				["read"] = ParseReadCommand,
				["status"] = ParseStatusCommand,
				["profile"] = (arg, flags) => new ProfileCommand(),
				["help"] = (arg, flags) => new HelpCommand(),
				["undo"] = (arg, flags) => new UndoCommand(),
				["redo"] = (arg, flags) => new RedoCommand()
			};
		}

		public static ICommand? Parse(string inputString)
		{
			if (string.IsNullOrWhiteSpace(inputString))
				return null;

			var parts = inputString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length == 0)
				return null;

			string commandName = parts[0].ToLowerInvariant();
			var flags = new List<string>();
			int i = 1;

			for (; i < parts.Length; i++)
			{
				string token = parts[i];
				if (token.StartsWith("--"))
					flags.Add(token.Substring(2));
				else if (token.StartsWith("-") && token.Length > 1)
				{
					foreach (char c in token.Substring(1))
					{
						switch (c)
						{
							case 'm': flags.Add("multiline"); break;
							case 'a': flags.Add("all"); break;
							case 'i': flags.Add("index"); break;
							case 's': flags.Add("status"); break;
							case 'd': flags.Add("update-date"); break;
						}
					}
				}
				else break;
			}

			string arg = i < parts.Length ? string.Join(' ', parts, i, parts.Length - i) : string.Empty;

			if (_commandHandlers.TryGetValue(commandName, out var handler))
			{
				return handler(arg, flags.ToArray());
			}

			return null;
		}

		private static ICommand ParseAddCommand(string arg, string[] flags)
		{
			return new AddCommand
			{
				Text = arg,
				IsMultiline = flags.Contains("multiline"),
				Flags = flags.ToArray()
			};
		}

		private static ICommand ParseViewCommand(string arg, string[] flags)
		{
			return new ViewCommand
			{
				Flags = flags.ToArray()
			};
		}

		private static ICommand ParseDeleteCommand(string arg, string[] flags)
		{
			return new DeleteCommand
			{
				Arg = arg
			};
		}

		private static ICommand ParseUpdateCommand(string arg, string[] flags)
		{
			return new UpdateCommand
			{
				Arg = arg
			};
		}

		private static ICommand ParseReadCommand(string arg, string[] flags)
		{
			return new ReadCommand
			{
				Arg = arg
			};
		}

		private static ICommand ParseStatusCommand(string arg, string[] flags)
		{
			return new StatusCommand
			{
				Arg = arg
			};
		}
	}
}