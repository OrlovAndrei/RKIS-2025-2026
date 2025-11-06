using System;
using System.Collections.Generic;
using TodoList1.Commands;

namespace TodoList1;
public static class CommandParser
{
	public static BaseCommand Parse(string inputString, TodoList todoList, Profile profile)
	{
		if (string.IsNullOrWhiteSpace(inputString))
			return new HelpCommand();

		string[] parts = inputString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length == 0)
			return new HelpCommand();

		string commandName = parts[0].ToLower();

		switch (commandName)
		{
			case "exit":
				return new ExitCommand();

			case "help":
				return new HelpCommand();

			case "profile":
				return new ProfileCommand { Profile = profile };

			case "add":
				return ParseAddCommand(inputString, todoList);

			case "view":
				return ParseViewCommand(inputString, todoList);

			case "read":
				return ParseReadCommand(inputString, todoList);

			case "done":
				return ParseDoneCommand(inputString, todoList);

			case "delete":
				return ParseDeleteCommand(inputString, todoList);

			case "update":
				return ParseUpdateCommand(inputString, todoList);

			default:
				Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
				return new HelpCommand();
		}
	}
	private static BaseCommand ParseAddCommand(string input, TodoList todoList)
	{
		var command = new AddCommand { TodoList = todoList };

		if (input.Contains("--multiline") || input.Contains("-m"))
		{
			command.Multiline = true;
		}
		else
		{
			int startIndex = input.IndexOf("add") + 3;
			if (startIndex < input.Length)
				command.TaskText = input.Substring(startIndex).Trim(' ', '"');
		}

		return command;
	}
	private static BaseCommand ParseViewCommand(string input, TodoList todoList)
	{
		var command = new ViewCommand { TodoList = todoList, };

		command.ShowIndex = input.Contains("--index") || input.Contains("-i");
		command.ShowStatus = input.Contains("--status") || input.Contains("-s");
		command.ShowDate = input.Contains("--date") || input.Contains("-d");
		command.ShowAll = input.Contains("--all") || input.Contains("-a");

		return command;
	}
	private static BaseCommand ParseReadCommand(string input, TodoList todoList)
	{
		var command = new ReadCommand { TodoList = todoList };
		command.Index = GetIndexFromCommand(input);
		return command;
	}

	private static BaseCommand ParseDoneCommand(string input, TodoList todoList)
	{
		var command = new DoneCommand { TodoList = todoList };
		command.Index = GetIndexFromCommand(input);
		return command;
	}

	private static BaseCommand ParseDeleteCommand(string input, TodoList todoList)
	{
		var command = new DeleteCommand { TodoList = todoList };
		command.Index = GetIndexFromCommand(input);
		return command;
	}

	private static BaseCommand ParseUpdateCommand(string input, TodoList todoList)
	{
		var command = new UpdateCommand { TodoList = todoList };

		string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length >= 2 && int.TryParse(parts[1], out int index))
		{
			command.Index = index - 1;

			int firstSpaceIndex = input.IndexOf(' ');
			if (firstSpaceIndex != -1)
			{
				int secondSpaceIndex = input.IndexOf(' ', firstSpaceIndex + 1);
				if (secondSpaceIndex != -1)
				{
					command.NewText = input.Substring(secondSpaceIndex + 1).Trim(' ', '"');
				}
			}
		}
		else
		{
			Console.WriteLine("Ошибка: неверный формат команды update. Используйте: update \"номер\" \"новый текст\"");
		}
		return command;
	}
	public static int GetIndexFromCommand(string input)
	{
		string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length > 1 && int.TryParse(parts[1], out int index))
			return index - 1;
		else
		{
			Console.WriteLine("Ошибка: укажите номер задачи.");
			return - 1;
		}
	}
}


