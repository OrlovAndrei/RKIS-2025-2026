using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoList
{
	public static class CommandParser
	{
		public static ICommand Parse(string input)
		{
			string[] inputParts = input.Trim().Split(new[] { ' ' }, 2);
			string commandName = inputParts[0].ToLower();
			string args = inputParts.Length > 1 ? inputParts[1] : string.Empty;

			return commandName switch
			{
				"help" => new HelpCommand(),
				"profile" => new ProfileCommand(),
				"exit" => new ExitCommand(),
				"add" => ParseAddCommand(args),
				"view" => ParseViewCommand(args),
				"status" => ParseStatusCommand(args),
				"delete" => ParseDeleteCommand(args),
				_ => HandleUnknownCommand(commandName)
			};
		}

		private static ICommand ParseAddCommand(string args)
		{
			if (string.IsNullOrWhiteSpace(args))
			{
				Console.WriteLine("Ошибка: Команда add требует текст задачи.");
				return null;
			}
			return new AddCommand { TaskText = args.Trim('"') };
		}

		private static ICommand ParseStatusCommand(string args)
		{
			string[] parts = args.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length == 2 && int.TryParse(parts[0], out int index))
			{
				if (Enum.TryParse<TodoStatus>(parts[1], true, out TodoStatus status))
				{
					return new StatusCommand(index, status);
				}
			}
			Console.WriteLine("Ошибка: Использование: status <индекс> <notstarted|inprogress|completed|postponed|failed>");
			return null;
		}

		private static ICommand ParseViewCommand(string args)
		{
			var command = new ViewCommand();
			if (string.IsNullOrWhiteSpace(args)) return command;

			string[] flags = args.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var flag in flags)
			{
				switch (flag.ToLower().TrimStart('-'))
				{
					case "i": case "index": command.ShowIndex = true; break;
					case "s": case "status": command.ShowStatus = true; break;
					case "d": case "date": command.ShowDate = true; break;
					case "a": case "all": command.ShowAll = true; break;
				}
			}
			return command;
		}

		private static ICommand ParseDeleteCommand(string args)
		{
			if (int.TryParse(args, out int index))
			{
				return null;
			}
			return null;
		}

		private static ICommand HandleUnknownCommand(string name)
		{
			Console.WriteLine($"Ошибка: Неизвестная команда '{name}'. Введите 'help' для списка команд.");
			return null;
		}
	}

	public class HelpCommand : ICommand
	{
		public void Execute()
		{
			Console.WriteLine("""
            Доступные команды:
            help — список команд
            profile — данные профиля
            add "текст" — добавить задачу
            status <idx> <status> — изменить статус задачи
            view [i, s, d, a] — просмотр списка (flags: index, status, date, all)
            exit — выход из программы
            """);
		}
	}

	public class ExitCommand : ICommand
	{
		public void Execute() => Console.WriteLine("Завершение работы...");
	}

	public class ProfileCommand : ICommand
	{
		public void Execute()
		{
			if (AppInfo.CurrentProfile != null)
				Console.WriteLine(AppInfo.CurrentProfile.GetInfo());
		}
	}
}