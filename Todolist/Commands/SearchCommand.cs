using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Todolist.Commands
{
    internal class SearchCommand : ICommand
    {
        private const int TaskTextMaxDisplay = 30;
        private const string DateFormat = "yyyy-MM-dd";

        private string? _containsText;
        private string? _startsWithText;
        private string? _endsWithText;
        private TodoStatus? _statusFilter;
        private DateTime? _dateFrom;
        private DateTime? _dateTo;
        private string? _sortBy; // "text" | "date"
        private bool _sortDesc;
        private int? _topN;

        private bool _parseError;

        public SearchCommand(string args)
        {
            var tokens = Tokenize(args ?? string.Empty);
            _parseError = !ParseTokens(tokens);
        }

        private static List<string> Tokenize(string args)
        {
            var tokens = new List<string>();
            var current = new StringBuilder();
            bool inQuotes = false;
            for (int i = 0; i < args.Length; i++)
            {
                char c = args[i];
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                    if (!inQuotes)
                    {
                        tokens.Add(current.ToString());
                        current.Clear();
                    }
                }
                else if ((c == ' ' || c == '\t') && !inQuotes)
                {
                    if (current.Length > 0)
                    {
                        tokens.Add(current.ToString());
                        current.Clear();
                    }
                }
                else
                {
                    current.Append(c);
                }
            }
            if (current.Length > 0)
                tokens.Add(current.ToString());
            return tokens;
        }

        private bool ParseTokens(List<string> tokens)
        {
            int i = 0;
            while (i < tokens.Count)
            {
                string t = tokens[i];
                string tLower = t.ToLowerInvariant();

                if (tLower == "--contains" && i + 1 < tokens.Count)
                {
                    _containsText = tokens[i + 1];
                    i += 2;
                    continue;
                }
                if (tLower == "--starts-with" && i + 1 < tokens.Count)
                {
                    _startsWithText = tokens[i + 1];
                    i += 2;
                    continue;
                }
                if (tLower == "--ends-with" && i + 1 < tokens.Count)
                {
                    _endsWithText = tokens[i + 1];
                    i += 2;
                    continue;
                }
                if (tLower == "--status" && i + 1 < tokens.Count)
                {
                    if (!TryParseStatus(tokens[i + 1], out TodoStatus s))
                    {
                        Console.WriteLine("Ошибка: неизвестный статус. Возможные: NotStarted, InProgress, Completed, Postponed, Failed");
                        return false;
                    }
                    _statusFilter = s;
                    i += 2;
                    continue;
                }
                if (tLower == "--from" && i + 1 < tokens.Count)
                {
                    if (!DateTime.TryParseExact(tokens[i + 1], DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fromDate))
                    {
                        Console.WriteLine($"Ошибка: неверный формат даты (ожидается {DateFormat}): {tokens[i + 1]}");
                        return false;
                    }
                    _dateFrom = fromDate;
                    i += 2;
                    continue;
                }
                if (tLower == "--to" && i + 1 < tokens.Count)
                {
                    if (!DateTime.TryParseExact(tokens[i + 1], DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime toDate))
                    {
                        Console.WriteLine($"Ошибка: неверный формат даты (ожидается {DateFormat}): {tokens[i + 1]}");
                        return false;
                    }
                    _dateTo = toDate;
                    i += 2;
                    continue;
                }
                if (tLower == "--sort" && i + 1 < tokens.Count)
                {
                    string sortVal = tokens[i + 1].ToLowerInvariant();
                    if (sortVal != "text" && sortVal != "date")
                    {
                        Console.WriteLine("Ошибка: --sort допускает только 'text' или 'date'.");
                        return false;
                    }
                    _sortBy = sortVal;
                    i += 2;
                    continue;
                }
                if (tLower == "--desc")
                {
                    _sortDesc = true;
                    i += 1;
                    continue;
                }
                if (tLower == "--top" && i + 1 < tokens.Count)
                {
                    if (!int.TryParse(tokens[i + 1], NumberStyles.Integer, CultureInfo.InvariantCulture, out int n) || n <= 0)
                    {
                        Console.WriteLine("Ошибка: --top требует положительное число.");
                        return false;
                    }
                    _topN = n;
                    i += 2;
                    continue;
                }
                i++;
            }
            return true;
        }

        private static bool TryParseStatus(string statusStr, out TodoStatus status)
        {
            status = TodoStatus.NotStarted;
            if (Enum.TryParse<TodoStatus>(statusStr, true, out TodoStatus parsed))
            {
                status = parsed;
                return true;
            }
            switch (statusStr.ToLowerInvariant())
            {
                case "notstarted":
                case "неначата":
                    status = TodoStatus.NotStarted;
                    return true;
                case "inprogress":
                case "вработе":
                    status = TodoStatus.InProgress;
                    return true;
                case "completed":
                case "done":
                case "завершена":
                    status = TodoStatus.Completed;
                    return true;
                case "postponed":
                case "отложена":
                    status = TodoStatus.Postponed;
                    return true;
                case "failed":
                case "провалена":
                    status = TodoStatus.Failed;
                    return true;
                default:
                    return false;
            }
        }

        public void Execute()
        {
            if (_parseError)
                return;

            int count = AppInfo.Todos.Count;
            if (count == 0)
            {
                Console.WriteLine("Ничего не найдено");
                return;
            }

            // Источник: пары (индекс 1-based, задача) через LINQ
            IEnumerable<(int Index, TodoItem Item)> source = Enumerable
                .Range(1, count)
                .Select(i => (Index: i, Item: AppInfo.Todos.GetItem(i)));

            // Фильтрация через Where (без for/foreach)
            source = source.Where(pair =>
            {
                TodoItem item = pair.Item;
                string text = item.Text ?? string.Empty;

                if (_containsText != null && !text.Contains(_containsText, StringComparison.OrdinalIgnoreCase))
                    return false;
                if (_startsWithText != null && !text.StartsWith(_startsWithText, StringComparison.OrdinalIgnoreCase))
                    return false;
                if (_endsWithText != null && !text.EndsWith(_endsWithText, StringComparison.OrdinalIgnoreCase))
                    return false;
                if (_statusFilter.HasValue && item.Status != _statusFilter.Value)
                    return false;

                DateTime itemDate = item.LastUpdate == default ? DateTime.MinValue : item.LastUpdate.Date;
                if (_dateFrom.HasValue && itemDate < _dateFrom.Value.Date)
                    return false;
                if (_dateTo.HasValue && itemDate > _dateTo.Value.Date)
                    return false;

                return true;
            });

            // Сортировка через OrderBy / OrderByDescending / ThenBy
            if (_sortBy == "text")
            {
                source = _sortDesc
                    ? source.OrderByDescending(p => (p.Item.Text ?? string.Empty)).ThenByDescending(p => p.Index)
                    : source.OrderBy(p => (p.Item.Text ?? string.Empty)).ThenBy(p => p.Index);
            }
            else if (_sortBy == "date")
            {
                source = _sortDesc
                    ? source.OrderByDescending(p => p.Item.LastUpdate).ThenByDescending(p => p.Index)
                    : source.OrderBy(p => p.Item.LastUpdate).ThenBy(p => p.Index);
            }

            // Ограничение результата через Take
            if (_topN.HasValue)
                source = source.Take(_topN.Value);

            var results = source.ToList();
            if (results.Count == 0)
            {
                Console.WriteLine("Ничего не найдено");
                return;
            }

            PrintTable(results);
        }

        private static void PrintTable(List<(int Index, TodoItem Item)> results)
        {
            const int idxWidth = 5;
            const int textWidth = TaskTextMaxDisplay;
            const int statusWidth = 15;
            const int dateWidth = 20;

            string header = "Index | " + PadCenter("Text", textWidth) + " | " + PadCenter("Status", statusWidth) + " | " + PadCenter("LastUpdate", dateWidth);
            Console.WriteLine("Результаты поиска:");
            Console.WriteLine(header);
            Console.WriteLine(new string('-', header.Length));

            foreach (var (index, item) in results)
            {
                string text = TruncateWithEllipsis(item.Text ?? string.Empty, textWidth);
                string statusStr = GetStatusString(item.Status);
                string dateStr = item.LastUpdate == default ? "-" : item.LastUpdate.ToString("yyyy-MM-dd HH:mm");
                Console.WriteLine($"{index.ToString().PadRight(idxWidth)} | {text.PadRight(textWidth)} | {statusStr.PadRight(statusWidth)} | {dateStr.PadRight(dateWidth)}");
            }
        }

        private static string TruncateWithEllipsis(string s, int max)
        {
            if (s.Length <= max) return s;
            if (max <= 3) return s.Substring(0, max);
            return s.Substring(0, max - 3) + "...";
        }

        private static string PadCenter(string text, int width)
        {
            if (width <= 0) return string.Empty;
            if (text.Length >= width) return text.Substring(0, width);
            int left = (width - text.Length) / 2;
            int right = width - text.Length - left;
            return new string(' ', left) + text + new string(' ', right);
        }

        private static string GetStatusString(TodoStatus status)
        {
            return status switch
            {
                TodoStatus.NotStarted => "Не начата",
                TodoStatus.InProgress => "В работе",
                TodoStatus.Completed => "Завершена",
                TodoStatus.Postponed => "Отложена",
                TodoStatus.Failed => "Провалена",
                _ => "Неизвестно"
            };
        }

        public void Unexecute()
        {
            // search не изменяет данные, отменять нечего
        }
    }
}
