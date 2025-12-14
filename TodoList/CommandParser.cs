using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace TodoList
{
	public class ProfileCommand : ICommand
	{
		private readonly Profile _profile;

		public ProfileCommand(Profile profile)
		{
			_profile = profile;
		}

		public void Execute()
		{
			Console.WriteLine(_profile.GetInfo());
		}
	}

	public class HelpCommand : ICommand
	{
		public void Execute()
		{
			Console.WriteLine("""
            Доступные команды:
            help — список команд
            profile — выводит данные профиля
            add "текст" [multiline|m] — добавляет задачу.
            done <idx> — отмечает задачу как выполненную.
            update <idx> "новый текст" — изменяет текст задачи.
            view [флаги] — просмотр всех задач.
                index, i — показать индекс задачи
                status, s — показать статус задачи (сделано/не сделано)
                update-date, d — показать дату последнего изменения
                all, a — показать все данные
            read <idx> — просмотр полного текста задачи.
            exit — завершить программу
            """);
		}
	}

	public class ExitCommand : ICommand
	{
		public void Execute()
		{
			Console.WriteLine("Программа завершена.");
		}
	}

	public class ReadCommand : IndexedCommandBase
	{
		public ReadCommand(TodoList todoList, string todoFilePath) : base(todoList, todoFilePath) { }

		public override void Execute()
		{
			TodoItem item = GetTaskOrShowError();
			if (item == null) return;

			Console.WriteLine(item.GetFullInfo());
		}
	}

	public class ViewCommand : ICommand
	{
		private readonly TodoList _todoList;
		public bool ShowIndex { get; set; }
		public bool ShowStatus { get; set; }
		public bool ShowDate { get; set; }
		public bool ShowAll { get; set; }

		public ViewCommand(TodoList todoList)
		{
			_todoList = todoList;
		}

		public void Execute()
		{
			bool showIndex = ShowIndex || ShowAll;
			bool showStatus = ShowStatus || ShowAll;
			bool showDate = ShowDate || ShowAll;

			_todoList.View(showIndex, showStatus, showDate);
		}
	}

	public static class CommandParser
	{
		private const string CommandHelp = "help";
		private const string CommandProfile = "profile";
		private const string CommandAdd = "add";
		private const string CommandDone = "done";
		private const string CommandUpdate = "update";
		private const string CommandView = "view";
		private const string CommandRead = "read";
		private const string CommandExit = "exit";

		public static ICommand Parse(string inputString, TodoList todoList, Profile profile, string todoFilePath)
		{
			if (string.IsNullOrEmpty(inputString))
			{
				return null;
			}

			string[] parts = inputString.Trim().Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
			string commandName = parts[0].ToLowerInvariant();
			string args = parts.Length > 1 ? parts[1].Trim() : string.Empty;

			try
			{
				switch (commandName)
				{
					case CommandHelp:
						return new HelpCommand();

					case CommandProfile:
						return new ProfileCommand(profile);

					case CommandExit:
						return new ExitCommand();

					case CommandAdd:
						return ParseAddCommand(args, todoList, todoFilePath);

					case CommandView:
						return ParseViewCommand(args, todoList);

					case CommandDone:
						return ParseSimpleIndexCommand<DoneCommand>(args, todoList, todoFilePath);

					case CommandRead:
						return ParseSimpleIndexCommand<ReadCommand>(args, todoList, todoFilePath);

					case CommandUpdate:
						return ParseUpdateCommand(args, todoList, todoFilePath);

					default:
						Console.WriteLine($"Неизвестная команда: {commandName}. Введите help для списка команд.");
						return null;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при разборе команды '{commandName}': {ex.Message}");
				return null;
			}
		}

		private static ICommand ParseAddCommand(string args, TodoList todoList, string todoFilePath)
		{
			var command = new AddCommand(todoList, todoFilePath);
			string taskText = args;

			if (args.EndsWith(CommandFlags.FlagMultiline) || args.EndsWith(CommandFlags.FlagShortMultiline))
			{
				command.IsMultiline = true;

				if (args.EndsWith(CommandFlags.FlagMultiline))
				{
					taskText = args.Substring(0, args.Length - CommandFlags.FlagMultiline.Length).Trim();
				}
				else
				{
					taskText = args.Substring(0, args.Length - CommandFlags.FlagShortMultiline.Length).Trim();
				}
			}

			if (command.IsMultiline)
			{
				taskText = ReadMultilineInput(taskText);
			}

			command.TaskText = taskText;
			return command;
		}

		private static string ReadMultilineInput(string initialText)
		{
			Console.WriteLine("Введите задачу (для завершения введите !end):");
			System.Text.StringBuilder taskBuilder = new System.Text.StringBuilder();

			if (!string.IsNullOrEmpty(initialText))
			{
				taskBuilder.AppendLine(initialText);
			}

			while (true)
			{
				Console.Write("> ");
				string line = Console.ReadLine();
				if (line?.Trim().Equals("!end", StringComparison.OrdinalIgnoreCase) == true)
				{
					break;
				}
				taskBuilder.AppendLine(line);
			}
			return taskBuilder.ToString().TrimEnd();
		}

		private static ICommand ParseViewCommand(string args, TodoList todoList)
		{
			var command = new ViewCommand(todoList);

			command.ShowIndex = args.Contains(CommandFlags.FlagIndex) || args.Contains(CommandFlags.FlagShortIndex);
			command.ShowStatus = args.Contains(CommandFlags.FlagStatus) || args.Contains(CommandFlags.FlagShortStatus);
			command.ShowDate = args.Contains(CommandFlags.FlagDate) || args.Contains(CommandFlags.FlagShortDate);
			command.ShowAll = args.Contains(CommandFlags.FlagAll) || args.Contains(CommandFlags.FlagShortAll);

			return command;
		}

		private static ICommand ParseSimpleIndexCommand<T>(string args, TodoList todoList, string todoFilePath) where T : IndexedCommandBase
		{
			if (string.IsNullOrEmpty(args) || !int.TryParse(args, out int index))
			{
				Console.WriteLine($"Неверный формат. Требуется индекс: {typeof(T).Name.Replace("Command", "").ToLowerInvariant()} <idx>");
				return null;
			}

			T command = (T)Activator.CreateInstance(typeof(T), todoList, todoFilePath);
			command.Index = index;
			return command;
		}

		private static ICommand ParseUpdateCommand(string args, TodoList todoList, string todoFilePath)
		{
			string[] parts = args.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);

			if (parts.Length != 2 || !int.TryParse(parts[0], out int index) || string.IsNullOrEmpty(parts[1]))
			{
				Console.WriteLine("Неверный формат команды. Используйте: update <idx> \"новый текст\"");
				return null;
			}

			var command = new UpdateCommand(todoList, todoFilePath)
			{
				Index = index,
				NewText = parts[1]
			};

			return command;
		}
	}
}