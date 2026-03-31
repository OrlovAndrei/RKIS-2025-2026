using System.Collections.Generic;
using System.Linq;

namespace TodoList
{
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
    if (string.IsNullOrWhiteSpace(args))
    {
        throw new ArgumentException("Недостаточно параметров для команды update. Используйте: update <индекс> \"новый текст\"");
    }

    var parts = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    
    if (parts.Length < 2)
    {
        throw new ArgumentException("Недостаточно параметров для команды update. Используйте: update <индекс> \"новый текст\"");
    }
    
    // Парсим индекс
    if (!int.TryParse(parts[0], out int index) || index < 1)
    {
        throw new ArgumentException("Неверный индекс для команды update.");
    }
    
    // Собираем текст (может быть в кавычках или без)
    string newText;
    
    if (parts.Length == 2 && parts[1].StartsWith("\"") && parts[1].EndsWith("\""))
    {
        // Текст в кавычках
        newText = parts[1].Trim('"');
    }
    else if (parts.Length > 2)
    {
        // Текст без кавычек или с пробелами
        newText = string.Join(" ", parts.Skip(1)).Trim('"');
    }
    else
    {
        newText = parts[1].Trim('"');
    }
    
    if (string.IsNullOrWhiteSpace(newText))
    {
        throw new ArgumentException("Текст задачи не может быть пустым.");
    }
    
    return new UpdateCommand(index, newText);
}
        private static ICommand ParseSearch(string args)
        {
            string searchText = "";
            TodoStatus? statusFilter = null;
            DateTime? fromDate = null;
            DateTime? toDate = null;
            string sortBy = "";
            bool sortDescending = false;
            int? top = null;
            bool caseSensitive = false;
            TextMatchType matchType = TextMatchType.Contains;

            // Если нет аргументов, показываем справку
            if (string.IsNullOrWhiteSpace(args))
            {
                Console.WriteLine("Использование: search [флаги]");
                Console.WriteLine("  --contains <текст>   - задачи, содержащие указанный текст");
                Console.WriteLine("  --starts-with <текст> - задачи, начинающиеся с указанного текста");
                Console.WriteLine("  --ends-with <текст>   - задачи, заканчивающиеся указанным текстом");
                Console.WriteLine("  --status <статус>     - фильтр по статусу");
                Console.WriteLine("  --from <дата>         - задачи с датой не раньше (yyyy-MM-dd)");
                Console.WriteLine("  --to <дата>           - задачи с датой не позже (yyyy-MM-dd)");
                Console.WriteLine("  --sort text|date      - сортировка по тексту или дате");
                Console.WriteLine("  --desc                - сортировка по убыванию");
                Console.WriteLine("  --top <n>             - показать только первые n задач");
                Console.WriteLine("  --case-sensitive      - регистрозависимый поиск");
                Console.WriteLine("\nПримеры:");
                Console.WriteLine("  search --contains \"купить\"");
                Console.WriteLine("  search --starts-with \"Важно\" --status InProgress");
                Console.WriteLine("  search --from 2024-01-01 --to 2024-12-31 --sort date --desc");
                throw new ArgumentException("Не указаны параметры поиска.");
            }

            var parts = args.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            
            int i = 0;
            while (i < parts.Length)
            {
                string flag = parts[i].ToLower();
                
                switch (flag)
                {
                    case "--contains":
                        if (i + 1 >= parts.Length)
                            throw new ArgumentException("Не указан текст для --contains");
                        searchText = UnquoteString(parts[i + 1]);
                        matchType = TextMatchType.Contains;
                        i += 2;
                        break;
                        
                    case "--starts-with":
                        if (i + 1 >= parts.Length)
                            throw new ArgumentException("Не указан текст для --starts-with");
                        searchText = UnquoteString(parts[i + 1]);
                        matchType = TextMatchType.StartsWith;
                        i += 2;
                        break;
                        
                    case "--ends-with":
                        if (i + 1 >= parts.Length)
                            throw new ArgumentException("Не указан текст для --ends-with");
                        searchText = UnquoteString(parts[i + 1]);
                        matchType = TextMatchType.EndsWith;
                        i += 2;
                        break;
                        
                    case "--status":
                        if (i + 1 >= parts.Length)
                            throw new ArgumentException("Не указан статус для --status");
                        if (!Enum.TryParse<TodoStatus>(parts[i + 1], true, out var status))
                            throw new ArgumentException("Неверный статус. Доступные: NotStarted, InProgress, Completed, Postponed, Failed");
                        statusFilter = status;
                        i += 2;
                        break;
                        
                    case "--from":
                        if (i + 1 >= parts.Length)
                            throw new ArgumentException("Не указана дата для --from");
                        if (!DateTime.TryParse(parts[i + 1], out var from))
                            throw new ArgumentException("Неверный формат даты для --from. Используйте: yyyy-MM-dd");
                        fromDate = from.Date;
                        i += 2;
                        break;
                        
                    case "--to":
                        if (i + 1 >= parts.Length)
                            throw new ArgumentException("Не указана дата для --to");
                        if (!DateTime.TryParse(parts[i + 1], out var to))
                            throw new ArgumentException("Неверный формат даты для --to. Используйте: yyyy-MM-dd");
                        toDate = to.Date;
                        i += 2;
                        break;
                        
                    case "--sort":
                        if (i + 1 >= parts.Length)
                            throw new ArgumentException("Не указан параметр сортировки для --sort");
                        string sortValue = parts[i + 1].ToLower();
                        if (sortValue != "text" && sortValue != "date")
                            throw new ArgumentException("Неверный параметр сортировки. Доступные: text, date");
                        sortBy = sortValue;
                        i += 2;
                        break;
                        
                    case "--desc":
                        sortDescending = true;
                        i++;
                        break;
                        
                    case "--top":
                        if (i + 1 >= parts.Length)
                            throw new ArgumentException("Не указано количество для --top");
                        if (!int.TryParse(parts[i + 1], out int topValue))
                            throw new ArgumentException("Неверный формат числа для --top");
                        if (topValue <= 0)
                            throw new ArgumentException("Количество для --top должно быть положительным числом");
                        top = topValue;
                        i += 2;
                        break;
                        
                    case "--case-sensitive":
                        caseSensitive = true;
                        i++;
                        break;
                        
                    default:
                        throw new ArgumentException($"Неизвестный флаг: {flag}");
                }
            }
            
            // Проверяем, что указан хотя бы один критерий поиска
            if (string.IsNullOrEmpty(searchText) && 
                !statusFilter.HasValue && 
                !fromDate.HasValue && 
                !toDate.HasValue)
            {
                throw new ArgumentException("Не указан ни один критерий поиска. Используйте хотя бы один флаг: --contains, --starts-with, --ends-with, --status, --from, --to");
            }
            
            // Проверяем корректность диапазона дат
            if (fromDate.HasValue && toDate.HasValue && fromDate.Value > toDate.Value)
            {
                throw new ArgumentException("Дата 'от' не может быть позже даты 'до'");
            }

            return new SearchCommand(searchText, statusFilter, fromDate, toDate, sortBy, sortDescending, top, caseSensitive, matchType);
        }

        private static string UnquoteString(string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;
            
            if (s.StartsWith("\"") && s.EndsWith("\""))
                return s.Substring(1, s.Length - 2);
            
            return s;
        }
    }
}