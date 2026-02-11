using System;
using System.Collections.Generic;
using System.Linq;

namespace Todolist
{
    public static class CommandParser
    {
        public static ICommand Parse(string input)
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
                        return ParseProfileCommand(parts);
                    case "exit":
                        return new ExitCommand();
                    case "view":
                        return ParseViewCommand(parts);
                    case "add":
                        return ParseAddCommand(parts);
                    case "read":
                        return ParseReadCommand(parts);
                    case "delete":
                        return ParseDeleteCommand(parts);
                    case "update":
                        return ParseUpdateCommand(parts);
                    case "status":
                        return ParseStatusCommand(parts);
                    case "undo":
                        return new UndoCommand();
                    case "redo":
                        return new RedoCommand();
                    case "search":
                        return ParseSearchCommand(parts);
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

        private static ICommand ParseProfileCommand(string[] parts)
        {
            bool isLogout = false;
            
            if (parts.Length > 1)
            {
                for (int i = 1; i < parts.Length; i++)
                {
                    if (parts[i] == "-o" || parts[i] == "--out")
                    {
                        isLogout = true;
                        break;
                    }
                }
            }
            
            return new ProfileCommand(isLogout);
        }

        private static ICommand ParseViewCommand(string[] parts)
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

            return new ViewCommand(showIndex, showStatus, showDate, showAll);
        }

        private static ICommand ParseAddCommand(string[] parts)
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

            return new AddCommand(taskText, isMultiline);
        }

        private static ICommand ParseReadCommand(string[] parts)
        {
            if (parts.Length < 2 || !int.TryParse(parts[1], out int taskNumber))
                throw new ArgumentException("Неверный формат команды read");

            return new ReadCommand(taskNumber);
        }

        private static ICommand ParseDeleteCommand(string[] parts)
        {
            if (parts.Length < 2 || !int.TryParse(parts[1], out int taskNumber))
                throw new ArgumentException("Неверный формат команды delete");

            return new DeleteCommand(taskNumber);
        }

        private static ICommand ParseUpdateCommand(string[] parts)
        {
            if (parts.Length < 3 || !int.TryParse(parts[1], out int taskNumber))
                throw new ArgumentException("Неверный формат команды update");

            string newText = string.Join(" ", parts, 2, parts.Length - 2);
            return new UpdateCommand(taskNumber, newText);
        }

        private static ICommand ParseStatusCommand(string[] parts)
        {
            if (parts.Length < 3 || !int.TryParse(parts[1], out int taskNumber))
                throw new ArgumentException("Неверный формат команды status");

            if (!Enum.TryParse<TodoStatus>(parts[2], true, out TodoStatus status))
                throw new ArgumentException("Неверный статус задачи");

            return new StatusCommand(taskNumber, status);
        }

        private static ICommand ParseSearchCommand(string[] parts)
        {
            Dictionary<string, string> flags = new Dictionary<string, string>();
            
            for (int i = 1; i < parts.Length; i++)
            {
                switch (parts[i].ToLower())
                {
                    case "--contains":
                    case "--starts-with":
                    case "--ends-with":
                    case "--from":
                    case "--to":
                    case "--status":
                    case "--sort":
                    case "--top":
                        if (i + 1 < parts.Length && !parts[i + 1].StartsWith("--"))
                        {
                            flags[parts[i].ToLower()] = parts[i + 1];
                            i++;
                        }
                        else
                        {
                            Console.WriteLine($"Ошибка: для флага {parts[i]} не указано значение");
                            return null;
                        }
                        break;
                        
                    case "--desc":
                        flags[parts[i].ToLower()] = "true";
                        break;
                        
                    default:
                        Console.WriteLine($"Неизвестный флаг: {parts[i]}");
                        return null;
                }
            }

            return new SearchCommand(flags);
        }
    }
}