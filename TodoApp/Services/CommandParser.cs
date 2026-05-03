using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TodoApp.Commands;
using TodoApp.Exceptions;
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
                ["load"] = ParseLoadCommand,
                ["sync"] = ParseSyncCommand,
                ["undo"] = args => new UndoCommand(),
                ["redo"] = args => new RedoCommand(),
            };
        }

        public static ICommand Parse(string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
            {
                throw new InvalidCommandException("Пустая команда.");
            }

            var parts = SplitCommand(inputString);
            if (parts.Length == 0)
            {
                throw new InvalidCommandException("Пустая команда.");
            }

            string command = parts[0].ToLower();
            string args = inputString.Length > parts[0].Length
                ? inputString.Substring(parts[0].Length).TrimStart()
                : string.Empty;

            if (!_commandHandlers.ContainsKey(command))
            {
                throw new InvalidCommandException($"Команда '{command}' не зарегистрирована.");
            }

            return _commandHandlers[command](args);
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

            string text = string.Join(" ", args).Trim('"');
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new InvalidArgumentException("Текст задачи не может быть пустым.");
            }

            return new AddCommand(text, false);
        }

        private static ICommand ParseViewCommand(string input)
        {
            var args = SplitCommand(input);
            bool showIndex = args.Any(a => a == "-i" || a == "--index");
            bool showStatus = args.Any(a => a == "-s" || a == "--status");
            bool showDate = args.Any(a => a == "-d" || a == "--update-date");
            bool showAll = args.Any(a => a == "-a" || a == "--all");

            return showAll
                ? new ViewCommand(true, true, true)
                : new ViewCommand(showIndex, showStatus, showDate);
        }

        private static ICommand ParseReadCommand(string input)
        {
            var args = SplitCommand(input);
            if (args.Length == 0 || !int.TryParse(args[0], out int index))
            {
                throw new InvalidArgumentException("Используйте: read <индекс>. Индекс должен быть числом.");
            }

            return new ReadCommand(index);
        }

        private static ICommand ParseStatusCommand(string input)
        {
            var args = SplitCommand(input);
            if (args.Length < 2)
            {
                throw new InvalidArgumentException("Используйте: status <индекс> <статус>.");
            }

            if (!int.TryParse(args[0], out int index))
            {
                throw new InvalidArgumentException("Индекс задачи должен быть числом.");
            }

            if (Enum.TryParse<TodoStatus>(args[1], ignoreCase: true, out var status))
            {
                return new StatusCommand(index, status);
            }

            throw new InvalidArgumentException("Неизвестный статус. Доступные: NotStarted, InProgress, Completed, Postponed, Failed.");
        }

        private static ICommand ParseUpdateCommand(string input)
        {
            var args = SplitCommand(input);
            if (args.Length < 2)
            {
                throw new InvalidArgumentException("Используйте: update <индекс> \"новый текст\".");
            }

            if (!int.TryParse(args[0], out int index))
            {
                throw new InvalidArgumentException("Индекс задачи должен быть числом.");
            }

            string newText = string.Join(" ", args.Skip(1)).Trim('"');
            if (string.IsNullOrWhiteSpace(newText))
            {
                throw new InvalidArgumentException("Новый текст задачи не может быть пустым.");
            }

            return new UpdateCommand(index, newText);
        }

        private static ICommand ParseDeleteCommand(string input)
        {
            var args = SplitCommand(input);
            if (args.Length == 0 || !int.TryParse(args[0], out int index))
            {
                throw new InvalidArgumentException("Используйте: delete <индекс>. Индекс должен быть числом.");
            }

            return new DeleteCommand(index);
        }

        private static ICommand ParseSearchCommand(string input)
        {
            return new SearchCommand(SplitCommand(input));
        }

        private static ICommand ParseLoadCommand(string input)
        {
            var args = SplitCommand(input);
            if (args.Length < 2)
            {
                throw new InvalidArgumentException("Используйте: load <количество_скачиваний> <размер_скачиваний>.");
            }

            if (!int.TryParse(args[0], out int downloadsCount))
            {
                throw new InvalidArgumentException("Количество скачиваний должно быть числом.");
            }

            if (!int.TryParse(args[1], out int downloadSize))
            {
                throw new InvalidArgumentException("Размер скачиваний должен быть числом.");
            }

            if (downloadsCount <= 0)
            {
                throw new InvalidArgumentException("Количество скачиваний должно быть больше 0.");
            }

            if (downloadSize <= 0)
            {
                throw new InvalidArgumentException("Размер скачиваний должен быть больше 0.");
            }

            return new LoadCommand(downloadsCount, downloadSize);
        }

        private static ICommand ParseSyncCommand(string input)
        {
            var args = SplitCommand(input);
            bool pull = args.Any(arg => arg == "--pull");
            bool push = args.Any(arg => arg == "--push");

            var unknownFlag = args.FirstOrDefault(arg => arg != "--pull" && arg != "--push");
            if (unknownFlag != null)
            {
                throw new InvalidCommandException($"Неизвестный флаг sync: {unknownFlag}");
            }

            if (pull == push)
            {
                throw new InvalidArgumentException("Используйте ровно один флаг: sync --pull или sync --push.");
            }

            return new SyncCommand(pull, push);
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
