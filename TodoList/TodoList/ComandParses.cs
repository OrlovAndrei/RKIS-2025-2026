using System;
using System.Collections.Generic;
using TodoList.Commands;
namespace TodoList
{
	public static class CommandParser
	{
		private static Dictionary<string, Func<string, ICommand>> _commandHandlers = new Dictionary<string, Func<string, ICommand>>();
		static CommandParser()
		{
			_commandHandlers["add"] = ParseAdd;
			_commandHandlers["delete"] = ParseDelete;
			_commandHandlers["update"] = ParseUpdate;
			_commandHandlers["status"] = ParseStatus;
			_commandHandlers["view"] = ParseView;
			_commandHandlers["profile"] = ParseProfile;
			_commandHandlers["search"] = ParseSearch;
			_commandHandlers["help"] = (args) => new CommandHelp();
			_commandHandlers["undo"] = (args) => new UndoCommand();
			_commandHandlers["redo"] = (args) => new RedoCommand();
		}
		public static ICommand Parse(string inputString)
		{
			if (string.IsNullOrWhiteSpace(inputString)) return null;
			var parts = inputString.Trim().Split(new char[] { ' ' }, 2);
			var commandName = parts[0].ToLower();
			var args = parts.Length > 1 ? parts[1] : "";
			if (_commandHandlers.TryGetValue(commandName, out var handler))
			{
				return handler(args);
			}
			Console.WriteLine($"Неизвестная команда: {commandName}. Введите 'help'.");
			return null;
		}
		private static ICommand ParseSearch(string args) => new SearchCommand(args.Trim());
		private static ICommand ParseAdd(string args)
		{
			if (args.StartsWith("-m") || args.StartsWith("--multiline"))
			{
				Console.WriteLine("Введите задачу (!end для завершения):");
				string text = "";
				while (true)
				{
					Console.Write("> ");
					string line = Console.ReadLine();
					if (line == "!end") break;
					if (!string.IsNullOrEmpty(text)) text += "\n";
					text += line;
				}
				if (string.IsNullOrWhiteSpace(text)) { Console.WriteLine("Пустая задача не добавлена."); return null; }
				return new AddCommand(text);
			}
			if (string.IsNullOrWhiteSpace(args))
			{
				Console.WriteLine("Ошибка: Введите текст задачи. Пример: add Купить молоко");
				return null;
			}
			return new AddCommand(args.Trim());
		}
		private static ICommand ParseDelete(string args)
		{
			if (int.TryParse(args, out int idx))
				return new DeleteCommand(idx);

			Console.WriteLine("Ошибка: Укажите числовой номер задачи. Пример: delete 1");
			return null;
		}
		private static ICommand ParseUpdate(string args)
		{
			string[] parts = args.Split(new char[] { ' ' }, 2);
			if (parts.Length == 2 && int.TryParse(parts[0], out int idx))
			{
				if (string.IsNullOrWhiteSpace(parts[1]))
				{
					Console.WriteLine("Ошибка: Текст задачи не может быть пустым.");
					return null;
				}
				return new UpdateCommand(idx, parts[1]);
			}
			Console.WriteLine("Ошибка: Неверный формат. Пример: update 1 Новое название");
			return null;
		}
		private static ICommand ParseStatus(string args)
		{
			string[] parts = args.Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length == 2 && int.TryParse(parts[0], out int idx))
			{
				if (Enum.TryParse<TodoStatus>(parts[1], true, out TodoStatus newStatus))
				{
					return new StatusCommand(idx, newStatus);
				}
				else
				{
					Console.WriteLine($"Ошибка: Статус '{parts[1]}' не найден.");
					Console.WriteLine("Доступные: NotStarted, InProgress, Completed, Postponed, Failed");
					return null;
				}
			}
			Console.WriteLine("Ошибка: Неверный формат. Пример: status 1 Completed");
			return null;
		}
		private static ICommand ParseView(string args)
		{
			ViewCommand command = new ViewCommand();
			string[] parts = args.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var part in parts)
			{
				switch (part.ToLower())
				{
					case "-i": case "--index": command.ShowIndex = true; break;
					case "-s": case "--status": command.ShowDone = true; break;
					case "-d": case "--update-date": command.ShowDate = true; break;
					case "-a": case "--all": command.ShowAll = true; break;
					default: Console.WriteLine($"Предупреждение: Флаг '{part}' не распознан."); break;
				}
			}
			return command;
		}
		private static ICommand ParseProfile(string args)
		{
			return new ProfileCommand("profile " + args);
		}
	}
}