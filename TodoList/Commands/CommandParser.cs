using System;
using System.Collections.Generic;
using TodoList.Exceptions;

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
				["search"] = ParseSearchCommand,
				["load"] = ParseLoadCommand
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
			var unknownFlags = new List<string>();
			
			int i = 1;
			while (i < parts.Length)
			{
				string token = parts[i];
				
				if (token.StartsWith("--"))
				{
					string flagName = token.Substring(2);
					
					if (!IsValidFlagForCommand(commandName, flagName))
					{
						unknownFlags.Add(flagName);
					}
					
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
						string shortFlag = c.ToString();
						string longFlag = shortFlag switch
						{
							"m" => "multiline",
							"a" => "all",
							"i" => "index",
							"s" => "status",
							"d" => "update-date",
							"o" => "out",
							_ => shortFlag
						};
						
						if (!IsValidFlagForCommand(commandName, longFlag))
						{
							unknownFlags.Add($"-{c}");
						}
						
						switch (c)
						{
							case 'm': flags.Add("multiline"); break;
							case 'a': flags.Add("all"); break;
							case 'i': flags.Add("index"); break;
							case 's': flags.Add("status"); break;
							case 'd': flags.Add("update-date"); break;
							case 'o': flags.Add("out"); break;
							default: flags.Add(shortFlag); break;
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

			if (unknownFlags.Count > 0)
			{
				throw new InvalidCommandException($"Неизвестные флаги: {string.Join(", ", unknownFlags)}");
			}

			string arg = args.Count > 0 ? string.Join(' ', args) : string.Empty;

			if (_commandHandlers.TryGetValue(commandName, out var handler))
			{
				return handler(arg, flags.ToArray());
			}

			throw new InvalidCommandException($"Неизвестная команда '{commandName}'. Введите 'help' для списка команд.");
		}

		private static bool IsValidFlagForCommand(string commandName, string flag)
		{
			return commandName switch
			{
				"add" => flag == "multiline" || flag == "m",
				"view" => flag == "index" || flag == "i" || flag == "status" || flag == "s" || 
						 flag == "update-date" || flag == "d" || flag == "all" || flag == "a",
				"profile" => flag == "out" || flag == "o" || flag == "logout",
				"search" => flag == "contains" || flag == "starts-with" || flag == "ends-with" || 
						   flag == "from" || flag == "to" || flag == "status" || flag == "sort" || 
						   flag == "desc" || flag == "top",
				"load" => false,
				_ => true
			};
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

		private static ICommand ParseLoadCommand(string arg, string[] flags)
		{
			return new LoadCommand
			{
				Arg = arg,
				Flags = flags
			};
		}
	}
}