using System.Collections.Generic;
using System.Linq;

namespace TodoList
{
    public class SearchFlags
    {
        public string? ContainsText { get; set; }
        public string? StartsWithText { get; set; }
        public string? EndsWithText { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public TodoStatus? Status { get; set; }
        public string? SortBy { get; set; }
        public bool Descending { get; set; }
        public int? TopCount { get; set; }
    }

    public static class CommandParser
    {
        private static readonly Dictionary<string, Func<string, ICommand>> _commandHandlers = new();

        static CommandParser()
        {
            InitializeCommandHandlers();
        }

        private static void InitializeCommandHandlers()
        {
            _commandHandlers["help"] = ParseHelp;
            _commandHandlers["profile"] = ParseProfile;
            _commandHandlers["exit"] = ParseExit;
            _commandHandlers["undo"] = ParseUndo;
            _commandHandlers["redo"] = ParseRedo;
            _commandHandlers["add"] = ParseAdd;
            _commandHandlers["view"] = ParseView;
            _commandHandlers["read"] = ParseRead;
            _commandHandlers["status"] = ParseStatus;
            _commandHandlers["delete"] = ParseDelete;
            _commandHandlers["update"] = ParseUpdate;
            _commandHandlers["search"] = ParseSearch;
        }

        public static ICommand Parse(string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
            {
                throw new ArgumentException("Введена пустая строка.");
            }

            var parts = inputString.Trim().Split(' ', 2);
            var commandName = parts[0].ToLower();
            var args = parts.Length > 1 ? parts[1] : string.Empty;

            if (_commandHandlers.TryGetValue(commandName, out var handler))
            {
                return handler(args);
            }

            throw new ArgumentException("Неизвестная команда.");
        }

        private static ICommand ParseHelp(string args) => new HelpCommand();
        private static ICommand ParseExit(string args) => new ExitCommand();
        private static ICommand ParseUndo(string args) => new UndoCommand();
        private static ICommand ParseRedo(string args) => new RedoCommand();

        private static ICommand ParseProfile(string args)
        {
            if (!string.IsNullOrEmpty(args))
            {
                var parts = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 0 && (parts[0] == "--out" || parts[0] == "-o"))
                {
                    return new ProfileCommand(logout: true);
                }
            }
            return new ProfileCommand(logout: false);
        }

        private static ICommand ParseAdd(string args)
        {
            if (string.IsNullOrEmpty(args))
            {
                throw new ArgumentException("Недостаточно параметров для команды add.");
            }

            var parts = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0 && (parts[0] == "--multiline" || parts[0] == "-m"))
            {
                return new AddCommand("", true);
            }

            return new AddCommand(args.Trim('"'), false);
        }

        private static ICommand ParseView(string args)
        {
            bool showIndex = false;
            bool showStatus = false;
            bool showDate = false;

            if (!string.IsNullOrEmpty(args))
            {
                var parts = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var part in parts)
                {
                    switch (part)
                    {
                        case "--index":
                        case "-i":
                            showIndex = true;
                            break;
                        case "--status":
                        case "-s":
                            showStatus = true;
                            break;
                        case "--update-date":
                        case "-d":
                            showDate = true;
                            break;
                        case "--all":
                        case "-a":
                            showIndex = true;
                            showStatus = true;
                            showDate = true;
                            break;
                    }
                }
            }

            return new ViewCommand(showIndex, showStatus, showDate);
        }

        private static ICommand ParseRead(string args)
        {
            if (!int.TryParse(args, out int index) || index < 1)
            {
                throw new ArgumentException("Неверный индекс для команды read.");
            }

            return new ReadCommand(index);
        }

        private static ICommand ParseStatus(string args)
        {
            var parts = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2 || !int.TryParse(parts[0], out int index) || index < 1)
            {
                throw new ArgumentException("Неверный индекс или статус для команды status. Пример: status 1 completed");
            }

            if (!Enum.TryParse<TodoStatus>(parts[1], true, out TodoStatus status))
            {
                throw new ArgumentException("Неверный статус. Доступные: NotStarted, InProgress, Completed, Postponed, Failed");
            }

            return new StatusCommand(index, status);
        }

        private static ICommand ParseDelete(string args)
        {
            if (!int.TryParse(args, out int index) || index < 1)
            {
                throw new ArgumentException("Неверный индекс для команды delete.");
            }

            return new DeleteCommand(index);
        }

        private static ICommand ParseUpdate(string args)
        {
            var parts = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2 || !int.TryParse(parts[0], out int index) || index < 1)
            {
                throw new ArgumentException("Неверный индекс для команды update.");
            }

            if (parts.Length < 3)
            {
                throw new ArgumentException("Недостаточно параметров для команды update.");
            }

            if (parts[1] == "--multiline" || parts[1] == "-m")
            {
                return new UpdateCommand(index, "", true);
            }

            var updateText = string.Join(" ", parts.Skip(1));
            return new UpdateCommand(index, updateText, false);
        }

        private static ICommand ParseSearch(string args)
        {
            var flags = new SearchFlags();

            if (string.IsNullOrEmpty(args))
            {
                return new SearchCommand(flags);
            }

            var parts = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < parts.Length; i++)
            {
                switch (parts[i])
                {
                    case "--contains":
                        if (i + 1 < parts.Length)
                        {
                            flags.ContainsText = parts[++i].Trim('"');
                        }
                        else
                        {
                            throw new ArgumentException("Не указан текст для поиска с флагом --contains");
                        }
                        break;

                    case "--starts-with":
                        if (i + 1 < parts.Length)
                        {
                            flags.StartsWithText = parts[++i].Trim('"');
                        }
                        else
                        {
                            throw new ArgumentException("Не указан текст для поиска с флагом --starts-with");
                        }
                        break;

                    case "--ends-with":
                        if (i + 1 < parts.Length)
                        {
                            flags.EndsWithText = parts[++i].Trim('"');
                        }
                        else
                        {
                            throw new ArgumentException("Не указан текст для поиска с флагом --ends-with");
                        }
                        break;

                    case "--from":
                        if (i + 1 < parts.Length)
                        {
                            if (!DateTime.TryParseExact(parts[++i], "yyyy-MM-dd",
                                System.Globalization.CultureInfo.InvariantCulture,
                                System.Globalization.DateTimeStyles.None, out DateTime from))
                            {
                                throw new ArgumentException($"Неверный формат даты: {parts[i]}. Используйте формат yyyy-MM-dd");
                            }
                            flags.FromDate = from;
                        }
                        else
                        {
                            throw new ArgumentException("Не указана дата для флага --from");
                        }
                        break;

                    case "--to":
                        if (i + 1 < parts.Length)
                        {
                            if (!DateTime.TryParseExact(parts[++i], "yyyy-MM-dd",
                                System.Globalization.CultureInfo.InvariantCulture,
                                System.Globalization.DateTimeStyles.None, out DateTime to))
                            {
                                throw new ArgumentException($"Неверный формат даты: {parts[i]}. Используйте формат yyyy-MM-dd");
                            }
                            flags.ToDate = to;
                        }
                        else
                        {
                            throw new ArgumentException("Не указана дата для флага --to");
                        }
                        break;

                    case "--status":
                        if (i + 1 < parts.Length)
                        {
                            if (!Enum.TryParse<TodoStatus>(parts[++i], true, out TodoStatus parsedStatus))
                            {
                                throw new ArgumentException($"Неверный статус: {parts[i]}. Доступные значения: NotStarted, InProgress, Completed, Postponed, Failed");
                            }
                            flags.Status = parsedStatus;
                        }
                        else
                        {
                            throw new ArgumentException("Не указан статус для флага --status");
                        }
                        break;

                    case "--sort":
                        if (i + 1 < parts.Length)
                        {
                            flags.SortBy = parts[++i].ToLower();
                            if (flags.SortBy != "text" && flags.SortBy != "date")
                            {
                                throw new ArgumentException($"Неверное значение для сортировки: {flags.SortBy}. Допустимые значения: text, date");
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Не указан критерий сортировки для флага --sort");
                        }
                        break;

                    case "--desc":
                        flags.Descending = true;
                        break;

                    case "--top":
                        if (i + 1 < parts.Length)
                        {
                            if (!int.TryParse(parts[++i], out int top) || top <= 0)
                            {
                                throw new ArgumentException($"Неверное количество задач для флага --top: {parts[i]}. Укажите положительное число");
                            }
                            flags.TopCount = top;
                        }
                        else
                        {
                            throw new ArgumentException("Не указано количество задач для флага --top");
                        }
                        break;

                    default:
                        throw new ArgumentException($"Неизвестный флаг: {parts[i]}");
                }
            }

            return new SearchCommand(flags);
        }
    }
}