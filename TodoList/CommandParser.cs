using System;
using System.Collections.Generic;
using System.Linq;
using TodoList.Exceptions;

namespace TodoList
{
    public static class CommandParser
    {
        private static readonly Dictionary<string, Func<string, ICommand>> _commandHandlers = new();

        static CommandParser()
        {
            _commandHandlers["help"] = ParseHelp;
            _commandHandlers["profile"] = ParseProfile;
            _commandHandlers["add"] = ParseAdd;
            _commandHandlers["view"] = ParseView;
            _commandHandlers["status"] = ParseStatus;
            _commandHandlers["delete"] = ParseDelete;
            _commandHandlers["update"] = ParseUpdate;
            _commandHandlers["read"] = ParseRead;
            _commandHandlers["undo"] = ParseUndo;
            _commandHandlers["redo"] = ParseRedo;
            _commandHandlers["exit"] = ParseExit;
            _commandHandlers["search"] = ParseSearch;
            _commandHandlers["load"] = ParseLoad;
            _commandHandlers["sync"] = ParseSync;   
        }

        public static ICommand? Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new InvalidCommandException("Команда не может быть пустой.");

            var parts = input.Split(' ', 2);
            string cmdName = parts[0].ToLower();
            string args = parts.Length > 1 ? parts[1] : "";

            if (_commandHandlers.TryGetValue(cmdName, out var handler))
                return handler(args);

            throw new InvalidCommandException($"Неизвестная команда: '{cmdName}'.");
        }

        private static ICommand ParseHelp(string args) => new HelpCommand();
        private static ICommand ParseProfile(string args) => new ProfileCommand(args.Contains("--out") || args.Contains("-o"));

        private static ICommand ParseAdd(string args)
        {
            var flags = ParseFlagsFromArgs(args);
            bool isMultiline = flags.Contains("--multiline") || flags.Contains("-m");
            string text = ExtractTextFromArgs(args);
            if (string.IsNullOrWhiteSpace(text) && !isMultiline)
                throw new InvalidArgumentException("Текст задачи не может быть пустым.");
            return new AddCommand(text, isMultiline);
        }

        private static ICommand ParseView(string args)
        {
            var flags = ParseFlagsFromArgs(args);
            bool showAll = flags.Contains("--all") || flags.Contains("-a");
            return new ViewCommand(
                flags.Contains("--index") || flags.Contains("-i") || showAll,
                flags.Contains("--status") || flags.Contains("-s") || showAll,
                flags.Contains("--update-date") || flags.Contains("-d") || showAll
            );
        }

        private static ICommand ParseStatus(string args)
        {
            var parts = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
                throw new InvalidArgumentException("Укажите индекс и статус.");
            if (!int.TryParse(parts[0], out int idx))
                throw new InvalidArgumentException("Индекс должен быть числом.");
            if (!Enum.TryParse<TodoStatus>(parts[1], true, out var status))
                throw new InvalidArgumentException($"Недопустимый статус. Допустимые: {string.Join(", ", Enum.GetNames<TodoStatus>())}");
            return new StatusCommand(idx - 1, status);
        }

        private static ICommand ParseDelete(string args)
        {
            if (string.IsNullOrWhiteSpace(args))
                throw new InvalidArgumentException("Укажите индекс задачи.");
            if (!int.TryParse(args.Trim(), out int idx))
                throw new InvalidArgumentException("Индекс должен быть числом.");
            return new DeleteCommand(idx - 1);
        }

        private static ICommand ParseUpdate(string args)
        {
            var parts = args.Split(' ', 2);
            if (parts.Length < 2)
                throw new InvalidArgumentException("Укажите индекс и новый текст.");
            if (!int.TryParse(parts[0], out int idx))
                throw new InvalidArgumentException("Индекс должен быть числом.");
            if (string.IsNullOrWhiteSpace(parts[1]))
                throw new InvalidArgumentException("Новый текст не может быть пустым.");
            return new UpdateCommand(idx - 1, parts[1]);
        }

        private static ICommand ParseRead(string args)
        {
            if (string.IsNullOrWhiteSpace(args))
                throw new InvalidArgumentException("Укажите индекс задачи.");
            if (!int.TryParse(args.Trim(), out int idx))
                throw new InvalidArgumentException("Индекс должен быть числом.");
            return new ReadCommand(idx - 1);
        }

        private static ICommand ParseUndo(string args) => new UndoCommand();
        private static ICommand ParseRedo(string args) => new RedoCommand();
        private static ICommand ParseExit(string args) => new ExitCommand();

        private static ICommand ParseSearch(string args)
        {
            string contains = null, startsWith = null, endsWith = null;
            DateTime? from = null, to = null;
            TodoStatus? status = null;
            string sortBy = null;
            bool descending = false;
            int? top = null;

            var parts = SplitArgsRespectingQuotes(args);
            for (int i = 0; i < parts.Length; i++)
            {
                string flag = parts[i];
                switch (flag)
                {
                    case "--contains": contains = GetNext(parts, ref i); break;
                    case "--starts-with": startsWith = GetNext(parts, ref i); break;
                    case "--ends-with": endsWith = GetNext(parts, ref i); break;
                    case "--from":
                        string fromStr = GetNext(parts, ref i);
                        if (!DateTime.TryParse(fromStr, out var fd))
                            throw new InvalidArgumentException($"Некорректная дата: {fromStr}. Формат yyyy-MM-dd");
                        from = fd;
                        break;
                    case "--to":
                        string toStr = GetNext(parts, ref i);
                        if (!DateTime.TryParse(toStr, out var td))
                            throw new InvalidArgumentException($"Некорректная дата: {toStr}. Формат yyyy-MM-dd");
                        to = td;
                        break;
                    case "--status":
                        string statStr = GetNext(parts, ref i);
                        if (!Enum.TryParse<TodoStatus>(statStr, true, out var st))
                            throw new InvalidArgumentException($"Недопустимый статус: {statStr}");
                        status = st;
                        break;
                    case "--sort":
                        string sortVal = GetNext(parts, ref i);
                        if (sortVal != "text" && sortVal != "date")
                            throw new InvalidArgumentException("--sort может быть только 'text' или 'date'");
                        sortBy = sortVal;
                        break;
                    case "--desc":
                        descending = true;
                        break;
                    case "--top":
                        string topStr = GetNext(parts, ref i);
                        if (!int.TryParse(topStr, out var t) || t <= 0)
                            throw new InvalidArgumentException("--top должен быть положительным числом.");
                        top = t;
                        break;
                    default:
                        throw new InvalidArgumentException($"Неизвестный флаг: {flag}");
                }
            }
            return new SearchCommand(contains, startsWith, endsWith, from, to, status, sortBy, descending, top);
        }

        private static ICommand ParseLoad(string args)
        {
            if (string.IsNullOrWhiteSpace(args))
                throw new InvalidArgumentException("Необходимо указать количество загрузок и размер.");

            var parts = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
                throw new InvalidArgumentException("Необходимо указать два параметра: количество загрузок и размер.");

            if (!int.TryParse(parts[0], out int count) || count <= 0)
                throw new InvalidArgumentException("Количество загрузок должно быть положительным целым числом.");

            if (!int.TryParse(parts[1], out int size) || size <= 0)
                throw new InvalidArgumentException("Размер загрузки должен быть положительным целым числом.");

            return new LoadCommand(count, size);
        }

        private static ICommand ParseSync(string args)
        {
            bool pull = args.Contains("--pull");
            bool push = args.Contains("--push");
            return new SyncCommand(pull, push);
        }

        private static string GetNext(string[] arr, ref int i) =>
            i + 1 < arr.Length ? arr[++i] : throw new InvalidArgumentException($"Отсутствует значение для флага {arr[i]}");

        private static HashSet<string> ParseFlagsFromArgs(string args) =>
            new HashSet<string>(args.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Where(p => p.StartsWith("-"))
                .SelectMany(p => p.StartsWith("--") ? new[] { p } : p.Skip(1).Select(c => "-" + c)));

        private static string ExtractTextFromArgs(string args) =>
            string.Join(" ", args.Split(' ', StringSplitOptions.RemoveEmptyEntries).Where(p => !p.StartsWith("-")));

        private static string[] SplitArgsRespectingQuotes(string args)
        {
            var result = new List<string>();
            bool inQuotes = false;
            int start = 0;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == '"') inQuotes = !inQuotes;
                else if (args[i] == ' ' && !inQuotes)
                {
                    if (i > start) result.Add(args.Substring(start, i - start).Trim('"'));
                    start = i + 1;
                }
            }
            if (start < args.Length) result.Add(args.Substring(start).Trim('"'));
            return result.ToArray();
        }
    }
}