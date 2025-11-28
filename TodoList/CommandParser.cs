using System;
using System.Linq;

namespace Todolist
{
    public static class CommandParser
    {
        private static string _profileFilePath;

        public static void SetFilePaths(string profileFilePath)
        {
            _profileFilePath = profileFilePath;
        }

        public static ICommand Parse(string input, Todolist todoList, Profile user)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string commandName = parts[0];

            try
            {
                switch (commandName.ToLower())
                {
                    case "help":
                        return new HelpCommand();
                    case "profile":
                        return new ProfileCommand(user);
                    case "exit":
                        return new ExitCommand();
                    case "view":
                        return ParseViewCommand(parts, todoList);
                    case "add":
                        return ParseAddCommand(parts, todoList);
                    case "read":
                        return ParseReadCommand(parts, todoList);
                    case "delete":
                        return ParseDeleteCommand(parts, todoList);
                    case "update":
                        return ParseUpdateCommand(parts, todoList);
                    case "status":
                        return ParseStatusCommand(parts, todoList);
                    case "undo":
                        return new UndoCommand();
                    case "redo":
                        return new RedoCommand();
                    default:
                        Console.WriteLine($"Неизвестная команда: {commandName}");
                        return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при разборе команды: {ex.Message}");
                return null;
            }
        }

        private static ICommand ParseViewCommand(string[] parts, Todolist todoList)
        {
            bool showIndex = false, showStatus = false, showDate = false, showAll = false;

            for (int i = 1; i < parts.Length; i++)
            {
                switch (parts[i].ToLower())
                {
                    case "-i":
                    case "--index":
                        showIndex = true;
                        break;
                    case "-s":
                    case "--status":
                        showStatus = true;
                        break;
                    case "-d":
                    case "--update-date":
                        showDate = true;
                        break;
                    case "-a":
                    case "--all":
                        showAll = true;
                        break;
                    default:
                        Console.WriteLine($"Неизвестный флаг для команды view: {parts[i]}");
                        break;
                }
            }

            return new ViewCommand(todoList, showIndex, showStatus, showDate, showAll);
        }

        private static ICommand ParseAddCommand(string[] parts, Todolist todoList)
        {
            if (parts.Length < 2)
                throw new ArgumentException("Неверный формат команды add");

            bool isMultiline = false;
            string taskText;

            if (parts[1] == "-m" || parts[1] == "--multiline")
            {
                isMultiline = true;
                taskText = "";
            }
            else
            {
                taskText = string.Join(" ", parts, 1, parts.Length - 1);
            }

            return new AddCommand(todoList, taskText, isMultiline);
        }

        private static ICommand ParseReadCommand(string[] parts, Todolist todoList)
        {
            if (parts.Length < 2 || !int.TryParse(parts[1], out int taskNumber))
                throw new ArgumentException("Неверный формат команды read");

            return new ReadCommand(todoList, taskNumber);
        }

        private static ICommand ParseDeleteCommand(string[] parts, Todolist todoList)
        {
            if (parts.Length < 2 || !int.TryParse(parts[1], out int taskNumber))
                throw new ArgumentException("Неверный формат команды delete");

            return new DeleteCommand(todoList, taskNumber);
        }

        private static ICommand ParseUpdateCommand(string[] parts, Todolist todoList)
        {
            if (parts.Length < 3 || !int.TryParse(parts[1], out int taskNumber))
                throw new ArgumentException("Неверный формат команды update");

            string newText = string.Join(" ", parts, 2, parts.Length - 2);
            return new UpdateCommand(todoList, taskNumber, newText);
        }

        private static ICommand ParseStatusCommand(string[] parts, Todolist todoList)
        {
            if (parts.Length < 3 || !int.TryParse(parts[1], out int taskNumber))
                throw new ArgumentException("Неверный формат команды status");

            if (!Enum.TryParse<TodoStatus>(parts[2], true, out TodoStatus status))
                throw new ArgumentException("Неверный статус задачи");

            return new StatusCommand(todoList, taskNumber, status);
        }
    }
}