using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TodoApp.Commands;
using TodoApp.Models;

namespace TodoApp.Services
{
    public static class CommandParser
    {
        private static Dictionary<string, Func<string, ICommand>> _commandHandlers = new();

        public static TodoList? Todos => AppInfo.GetCurrentTodoList();

        static CommandParser()
        {
            InitializeHandlers();
        }

        private static void InitializeHandlers()
        {
            _commandHandlers = new Dictionary<string, Func<string, ICommand>>
            {
                ["help"] = args => new HelpCommand(),
                ["profile"] = ParseProfileCommand,
                ["add"] = ParseAddCommand,
                ["view"] = ParseViewCommand,
                ["read"] = ParseReadCommand,
                ["status"] = ParseStatusCommand,
                ["update"] = ParseUpdateCommand,
                ["delete"] = ParseDeleteCommand,
                ["search"] = ParseSearchCommand,
                ["undo"] = args => new UndoCommand(),
                ["redo"] = args => new RedoCommand(),
            };
        }

        public static ICommand Parse(string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
            {
                return new HelpCommand();
            }

            var parts = SplitCommand(inputString);
            if (parts.Length == 0)
                return new HelpCommand();

            string command = parts[0].ToLower();
            string args = inputString.Length > parts[0].Length
                ? inputString.Substring(parts[0].Length).TrimStart()
                : string.Empty;

            if (_commandHandlers.ContainsKey(command))
            {
                try
                {
                    return _commandHandlers[command](args);
                }
                catch
                {
                    Console.WriteLine($"Ошибка при выполнении команды '{command}'");
                    return new HelpCommand();
                }
            }

            Console.WriteLine($"Неизвестная команда: '{command}'. Введите 'help' для справки.");
            return new HelpCommand();
        }

        private static ICommand ParseProfileCommand(string input)
        {
            var args = SplitCommand(input);
            bool logout = args.Any(a => a == "-o" || a == "--out");
            return new ProfileCommand(logout);
        }

        private static ICommand ParseAddCommand(string input)
        {
            var args = SplitCommand(input);
            bool isMultiline = args.Any(a => a == "-m" || a == "--multiline");

            if (isMultiline)
            {
                return new AddCommand("", true);
            }

            string text = string.Join(" ", args);
            text = text.Trim('"');

            return new AddCommand(text, false);
        }

        private static ICommand ParseViewCommand(string input)
        {
            var args = SplitCommand(input);
            bool showIndex = args.Any(a => a == "-i" || a == "--index");
            bool showStatus = args.Any(a => a == "-s" || a == "--status");
            bool showDate = args.Any(a => a == "-d" || a == "--update-date");
            bool showAll = args.Any(a => a == "-a" || a == "--all");

            if (showAll)
                return new ViewCommand(true, true, true);

            return new ViewCommand(showIndex, showStatus, showDate);
        }

        private static ICommand ParseReadCommand(string input)
        {
            var args = SplitCommand(input);
            if (args.Length > 0 && int.TryParse(args[0], out int index))
            {
                return new ReadCommand(index);
            }

            Console.WriteLine("Используйте: read <индекс>");
            return new HelpCommand();
        }

        private static ICommand ParseStatusCommand(string input)
        {
            var args = SplitCommand(input);
            if (args.Length < 2)
            {
                Console.WriteLine("Используйте: status <индекс> <статус>");
                return new HelpCommand();
            }

            if (!int.TryParse(args[0], out int index))
            {
                Console.WriteLine("Индекс должен быть числом.");
                return new HelpCommand();
            }

            string statusStr = args[1].ToLower();
            if (Enum.TryParse<TodoStatus>(statusStr, ignoreCase: true, out var status))
            {
                return new StatusCommand(index, status);
            }

            Console.WriteLine("Неизвестный статус. Доступные: NotStarted, InProgress, Completed, Postponed, Failed");
            return new HelpCommand();
        }

        private static ICommand ParseUpdateCommand(string input)
        {
            var args = SplitCommand(input);
            if (args.Length < 2)
            {
                Console.WriteLine("Используйте: update <индекс> \"новый текст\"");
                return new HelpCommand();
            }

            if (!int.TryParse(args[0], out int index))
            {
                Console.WriteLine("Индекс должен быть числом.");
                return new HelpCommand();
            }

            string newText = string.Join(" ", args.Skip(1)).Trim('"');
            return new UpdateCommand(index, newText);
        }

        private static ICommand ParseDeleteCommand(string input)
        {
            var args = SplitCommand(input);
            if (args.Length == 0 || !int.TryParse(args[0], out int index))
            {
                Console.WriteLine("Используйте: delete <индекс>");
                return new HelpCommand();
            }

            return new DeleteCommand(index);
        }

        private static ICommand ParseSearchCommand(string input)
        {
            return new SearchCommand(SplitCommand(input));
        }

        private static string[] SplitCommand(string input)
        {
            var result = new List<string>();
            var regex = new Regex(@"[^\s""]+|""([^""]*)""");
            var matches = regex.Matches(input);

            foreach (Match match in matches)
            {
                if (match.Groups[1].Success)
                {
                    result.Add(match.Groups[1].Value);
                }
                else
                {
                    result.Add(match.Value);
                }
            }

            return result.ToArray();
        }
    }
}
