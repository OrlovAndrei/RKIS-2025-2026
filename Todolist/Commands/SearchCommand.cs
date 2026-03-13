using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Todolist.Exceptions;

namespace Todolist.Commands
{
    internal class SearchCommand : ICommand
    {
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

        public SearchCommand(string args)
        {
            var tokens = Tokenize(args ?? string.Empty);
            ParseTokens(tokens);
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

        private void ParseTokens(List<string> tokens)
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
                    if (!TodoStatusHelper.TryParse(tokens[i + 1], out TodoStatus s))
                        throw new InvalidArgumentException("Неизвестный статус. Возможные: NotStarted, InProgress, Completed, Postponed, Failed");
                    _statusFilter = s;
                    i += 2;
                    continue;
                }
                if (tLower == "--from" && i + 1 < tokens.Count)
                {
                    if (!DateTime.TryParseExact(tokens[i + 1], DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fromDate))
                        throw new InvalidArgumentException($"Некорректный формат даты (ожидается {DateFormat}): {tokens[i + 1]}");
                    _dateFrom = fromDate;
                    i += 2;
                    continue;
                }
                if (tLower == "--to" && i + 1 < tokens.Count)
                {
                    if (!DateTime.TryParseExact(tokens[i + 1], DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime toDate))
                        throw new InvalidArgumentException($"Некорректный формат даты (ожидается {DateFormat}): {tokens[i + 1]}");
                    _dateTo = toDate;
                    i += 2;
                    continue;
                }
                if (tLower == "--sort" && i + 1 < tokens.Count)
                {
                    string sortVal = tokens[i + 1].ToLowerInvariant();
                    if (sortVal != "text" && sortVal != "date")
                        throw new InvalidArgumentException("Флаг --sort допускает только 'text' или 'date'.");
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
                        throw new InvalidArgumentException("Флаг --top требует положительное целое число.");
                    _topN = n;
                    i += 2;
                    continue;
                }
                if (t.StartsWith("--"))
                    throw new InvalidArgumentException($"Неизвестный флаг: {t}");
                i++;
            }
        }

        public void Execute()
        {
            if (AppInfo.CurrentProfileId == Guid.Empty)
                throw new AuthenticationException("Необходимо войти в профиль для работы с задачами.");

            int count = AppInfo.Todos.Count;
            if (count == 0)
            {
                Console.WriteLine("Ничего не найдено");
                return;
            }

            IEnumerable<(int Index, TodoItem Item)> source = Enumerable
                .Range(1, count)
                .Select(i => (Index: i, Item: AppInfo.Todos.GetItem(i)));

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

            if (_sortBy == "text")
            {
                source = _sortDesc
                    ? source.OrderByDescending(p => p.Item.Text ?? string.Empty).ThenByDescending(p => p.Index)
                    : source.OrderBy(p => p.Item.Text ?? string.Empty).ThenBy(p => p.Index);
            }
            else if (_sortBy == "date")
            {
                source = _sortDesc
                    ? source.OrderByDescending(p => p.Item.LastUpdate).ThenByDescending(p => p.Index)
                    : source.OrderBy(p => p.Item.LastUpdate).ThenBy(p => p.Index);
            }

            if (_topN.HasValue)
                source = source.Take(_topN.Value);

            List<TodoItem> results = source.Select(p => p.Item).ToList();
            if (results.Count == 0)
            {
                Console.WriteLine("Ничего не найдено");
                return;
            }

            new TodoList(results).View(true, true, true);
        }
    }
}
