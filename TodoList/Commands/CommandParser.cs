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
				["profile"] = (arg, flags) => new ProfileCommand { Flags = flags },
				["help"] = (arg, flags) => new HelpCommand(),
				["undo"] = (arg, flags) => new UndoCommand(),
				["redo"] = (arg, flags) => new RedoCommand(),
				["search"] = ParseSearchCommand
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
			var args = new List<string>();
			
			int i = 1;
			while (i < parts.Length)
			{
				string token = parts[i];
				
				if (token.StartsWith("--"))
				{
					string flagName = token.Substring(2);
					flags.Add(flagName);
					
					if (i + 1 < parts.Length && !parts[i + 1].StartsWith("-"))
					{
						flags.Add(parts[i + 1]);
						i += 2;
					}
					else
					{
						i++;
					}
				}
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
							case 'o': flags.Add("out"); break;
							default: Console.WriteLine($"Неизвестный флаг: -{c}"); break;
						}
					}
					i++;
				}
				else
				{
					args.Add(token);
					i++;
				}
			}

			string arg = args.Count > 0 ? string.Join(' ', args) : string.Empty;

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
				Flags = flags
			};
		}

		private static ICommand ParseViewCommand(string arg, string[] flags)
		{
			return new ViewCommand
			{
				Flags = flags
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

		private static ICommand ParseSearchCommand(string arg, string[] flags)
		{
			return new SearchCommand
			{
				Arg = arg,
				Flags = flags
			};
		}
	}
}