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
            _commandHandlers["search"] = ParseSearch; // Добавляем обработчик search
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

        // Новый метод для парсинга команды search
        private static ICommand ParseSearch(string args)
        {
            if (string.IsNullOrWhiteSpace(args))
            {
                throw new ArgumentException("Не указаны параметры поиска. Используйте: search [флаги]");
            }

            var criteria = new SearchCriteria();
            var parts = ParseSearchArguments(args);
            
            // Обработка текстовых флагов
            if (parts.TryGetValue("contains", out var containsText))
            {
                criteria.TextFilter = UnquoteString(containsText);
                criteria.TextMatchType = TextMatchType.Contains;
            }
            else if (parts.TryGetValue("starts-with", out var startsWithText))
            {
                criteria.TextFilter = UnquoteString(startsWithText);
                criteria.TextMatchType = TextMatchType.StartsWith;
            }
            else if (parts.TryGetValue("ends-with", out var endsWithText))
            {
                criteria.TextFilter = UnquoteString(endsWithText);
                criteria.TextMatchType = TextMatchType.EndsWith;
            }
            
            // Обработка статуса
            if (parts.TryGetValue("status", out var statusStr))
            {
                if (!Enum.TryParse<TodoStatus>(statusStr, true, out var status))
                    throw new ArgumentException("Неверный статус. Доступные: NotStarted, InProgress, Completed, Postponed, Failed");
                criteria.Status = status;
            }
            
            // Обработка дат
            if (parts.TryGetValue("from", out var fromDateStr))
            {
                if (!DateTime.TryParse(fromDateStr, out var fromDate))
                    throw new ArgumentException("Неверный формат даты для --from. Используйте: yyyy-MM-dd");
                criteria.FromDate = fromDate.Date;
            }
            
            if (parts.TryGetValue("to", out var toDateStr))
            {
                if (!DateTime.TryParse(toDateStr, out var toDate))
                    throw new ArgumentException("Неверный формат даты для --to. Используйте: yyyy-MM-dd");
                criteria.ToDate = toDate.Date;
            }
            
            // Обработка сортировки
            if (parts.TryGetValue("sort", out var sortBy))
            {
                if (sortBy != "text" && sortBy != "date")
                    throw new ArgumentException("Неверный параметр сортировки. Доступные: text, date");
                criteria.SortBy = sortBy;
            }
            
            // Обработка флага desc
            if (parts.ContainsKey("desc"))
            {
                criteria.SortDescending = true;
            }
            
            // Обработка top
            if (parts.TryGetValue("top", out var topStr))
            {
                if (!int.TryParse(topStr, out int top) || top <= 0)
                    throw new ArgumentException("Неверное количество для --top. Должно быть положительное число");
                criteria.Top = top;
            }
            
            // Обработка case-sensitive
            if (parts.ContainsKey("case-sensitive"))
            {
                criteria.CaseSensitive = true;
            }
            
            // Проверяем, что указан хотя бы один критерий поиска
            if (string.IsNullOrEmpty(criteria.TextFilter) && 
                !criteria.Status.HasValue && 
                !criteria.FromDate.HasValue && 
                !criteria.ToDate.HasValue)
            {
                throw new ArgumentException("Не указан ни один критерий поиска. Используйте хотя бы один флаг: --contains, --starts-with, --ends-with, --status, --from, --to");
            }
            
            return new SearchCommand(criteria);
        }
        
        // Вспомогательный метод для парсинга аргументов search
        private static Dictionary<string, string> ParseSearchArguments(string args)
        {
            var result = new Dictionary<string, string>();
            var parts = args.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            
            int i = 0;
            while (i < parts.Length)
            {
                string flag = parts[i].ToLower();
                
                if (flag.StartsWith("--"))
                {
                    string flagName = flag.Substring(2);
                    
                    // Флаги без значения
                    if (flagName == "desc" || flagName == "case-sensitive")
                    {
                        result[flagName] = "";
                        i++;
                        continue;
                    }
                    
                    // Флаги со значением
                    if (i + 1 >= parts.Length)
                        throw new ArgumentException($"Не указано значение для флага {flag}");
                    
                    string value = parts[i + 1];
                    result[flagName] = value;
                    i += 2;
                }
                else
                {
                    // Если встретили не флаг - игнорируем или выбрасываем ошибку
                    throw new ArgumentException($"Неожиданный аргумент: {flag}. Все параметры должны передаваться через флаги.");
                }
            }
            
            return result;
        }
        
        // Вспомогательный метод для удаления кавычек из строки
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