using System;
using System.Collections.Generic;

namespace TodoList.Commands
{
	public static class CommandParser
	{
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
							case 'o': flags.Add("out"); break;
						}
					}
				}
				else break;
			}

			string arg = i < parts.Length ? string.Join(' ', parts, i, parts.Length - i) : string.Empty;

			return commandName switch
			{
				"add" => new AddCommand
				{
					Text = arg,
					IsMultiline = flags.Contains("multiline"),
					Flags = flags.ToArray()
				},
				"view" => new ViewCommand
				{
					Flags = flags.ToArray()
				},
				"delete" => new DeleteCommand
				{
					Arg = arg
				},
				"update" => new UpdateCommand
				{
					Arg = arg
				},
				"read" => new ReadCommand
				{
					Arg = arg
				},
				"status" => new StatusCommand
				{
					Arg = arg
				},
				"profile" => new ProfileCommand
				{
					Flags = flags.ToArray()
				},
				"help" => new HelpCommand(),
				"undo" => new UndoCommand(),
				"redo" => new RedoCommand(),
				_ => null
			};
		}
	}
}