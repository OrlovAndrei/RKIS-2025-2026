using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Commands
{
    public class SearchCommand : ICommand
    {
        private readonly string[] _args;
        private readonly List<string> _errors = new();

        private string? _contains;
        private string? _startsWith;
        private string? _endsWith;
        private DateTime? _from;
        private DateTime? _to;
        private TodoStatus? _status;
        private string? _sort;
        private bool _desc;
        private int? _top;

        public SearchCommand(string[] args)
        {
            _args = args;
            ParseArgs();
        }

        public void Execute()
        {
            if (_errors.Count > 0)
            {
                Console.WriteLine(string.Join(Environment.NewLine, _errors));
                return;
            }

            var todos = CommandParser.Todos;
            if (todos == null || todos.Count == 0)
            {
                Console.WriteLine("Ничего не найдено");
                return;
            }

            var query = todos.GetAll()
                .Select((todo, index) => new SearchResult(index, todo))
                .AsEnumerable();

            query = ApplyTextFilters(query);
            query = ApplyDateFilters(query);
            query = ApplyStatusFilter(query);
            query = ApplySorting(query);
            query = ApplyTop(query);

            var results = query.ToList();
            if (results.Count == 0)
            {
                Console.WriteLine("Ничего не найдено");
                return;
            }

            Console.WriteLine(BuildTable(results));
        }

        private IEnumerable<SearchResult> ApplyTextFilters(IEnumerable<SearchResult> query)
        {
            if (!string.IsNullOrEmpty(_contains))
            {
                query = query.Where(result => result.Todo.Text.Contains(_contains, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(_startsWith))
            {
                query = query.Where(result => result.Todo.Text.StartsWith(_startsWith, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(_endsWith))
            {
                query = query.Where(result => result.Todo.Text.EndsWith(_endsWith, StringComparison.OrdinalIgnoreCase));
            }

            return query;
        }

        private IEnumerable<SearchResult> ApplyDateFilters(IEnumerable<SearchResult> query)
        {
            if (_from.HasValue)
            {
                query = query.Where(result => result.Todo.LastUpdate.Date >= _from.Value.Date);
            }

            if (_to.HasValue)
            {
                query = query.Where(result => result.Todo.LastUpdate.Date <= _to.Value.Date);
            }

            return query;
        }

        private IEnumerable<SearchResult> ApplyStatusFilter(IEnumerable<SearchResult> query)
        {
            if (_status.HasValue)
            {
                query = query.Where(result => result.Todo.Status == _status.Value);
            }

            return query;
        }

        private IEnumerable<SearchResult> ApplySorting(IEnumerable<SearchResult> query)
        {
            return _sort switch
            {
                "text" when _desc => query
                    .OrderByDescending(result => result.Todo.Text)
                    .ThenBy(result => result.Index),
                "text" => query
                    .OrderBy(result => result.Todo.Text)
                    .ThenBy(result => result.Index),
                "date" when _desc => query
                    .OrderByDescending(result => result.Todo.LastUpdate)
                    .ThenBy(result => result.Index),
                "date" => query
                    .OrderBy(result => result.Todo.LastUpdate)
                    .ThenBy(result => result.Index),
                _ => query
                    .OrderBy(result => result.Index)
                    .ThenBy(result => result.Todo.Text),
            };
        }

        private IEnumerable<SearchResult> ApplyTop(IEnumerable<SearchResult> query)
        {
            return _top.HasValue
                ? query.Take(_top.Value)
                : query;
        }

        private void ParseArgs()
        {
            for (int i = 0; i < _args.Length; i++)
            {
                string arg = _args[i];

                switch (arg)
                {
                    case "--contains":
                        _contains = ReadValue(++i, arg);
                        break;
                    case "--starts-with":
                        _startsWith = ReadValue(++i, arg);
                        break;
                    case "--ends-with":
                        _endsWith = ReadValue(++i, arg);
                        break;
                    case "--from":
                        ParseDate(ReadValue(++i, arg), "--from", value => _from = value);
                        break;
                    case "--to":
                        ParseDate(ReadValue(++i, arg), "--to", value => _to = value);
                        break;
                    case "--status":
                        ParseStatus(ReadValue(++i, arg));
                        break;
                    case "--sort":
                        ParseSort(ReadValue(++i, arg));
                        break;
                    case "--desc":
                        _desc = true;
                        break;
                    case "--top":
                        ParseTop(ReadValue(++i, arg));
                        break;
                    default:
                        _errors.Add($"Неизвестный флаг search: {arg}");
                        break;
                }
            }
        }

        private string? ReadValue(int index, string flag)
        {
            if (index >= _args.Length || _args[index].StartsWith("--"))
            {
                _errors.Add($"Для {flag} нужно указать значение.");
                return null;
            }

            return _args[index];
        }

        private void ParseDate(string? value, string flag, Action<DateTime> setDate)
        {
            if (value == null)
            {
                return;
            }

            bool hasExpectedFormat = value.Length == "yyyy-MM-dd".Length
                && value[4] == '-'
                && value[7] == '-';

            if (hasExpectedFormat
                && DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                setDate(date);
                return;
            }

            _errors.Add($"Некорректная дата для {flag}. Используйте формат yyyy-MM-dd.");
        }

        private void ParseStatus(string? value)
        {
            if (value == null)
            {
                return;
            }

            if (TryParseStatus(value, out TodoStatus status))
            {
                _status = status;
                return;
            }

            _errors.Add("Неизвестный статус. Доступные: notstarted, inprogress, completed, postponed, failed.");
        }

        private void ParseSort(string? value)
        {
            if (value == null)
            {
                return;
            }

            string sort = value.ToLowerInvariant();
            if (sort == "text" || sort == "date")
            {
                _sort = sort;
                return;
            }

            _errors.Add("Некорректная сортировка. Используйте --sort text или --sort date.");
        }

        private void ParseTop(string? value)
        {
            if (value == null)
            {
                return;
            }

            if (int.TryParse(value, out int top) && top > 0)
            {
                _top = top;
                return;
            }

            _errors.Add("Параметр --top должен быть положительным числом.");
        }

        private bool TryParseStatus(string value, out TodoStatus status)
        {
            string normalized = value.Replace("-", "").Replace("_", "");

            var match = ((TodoStatus[])Enum.GetValues(typeof(TodoStatus)))
                .Where(item => item.ToString().Equals(normalized, StringComparison.OrdinalIgnoreCase))
                .Select(item => (TodoStatus?)item)
                .FirstOrDefault();

            status = match.GetValueOrDefault();
            return match.HasValue;
        }

        private string BuildTable(List<SearchResult> results)
        {
            const int indexWidth = 5;
            const int textWidth = 35;
            const int statusWidth = 15;
            const int dateWidth = 20;

            string separator = $"+-{new string('-', indexWidth)}-+-{new string('-', textWidth)}-+-{new string('-', statusWidth)}-+-{new string('-', dateWidth)}-+";
            string header = $"| {"Index".PadRight(indexWidth)} | {"Text".PadRight(textWidth)} | {"Status".PadRight(statusWidth)} | {"LastUpdate".PadRight(dateWidth)} |";

            var rows = results.Select(result =>
                $"| {result.Index.ToString().PadRight(indexWidth)} | {Shorten(result.Todo.Text).PadRight(textWidth)} | {result.Todo.Status.ToString().PadRight(statusWidth)} | {result.Todo.LastUpdate.ToString("yyyy-MM-dd HH:mm").PadRight(dateWidth)} |");

            return string.Join(Environment.NewLine, new[] { separator, header, separator }
                .Concat(rows)
                .Concat(new[] { separator }));
        }

        private string Shorten(string text)
        {
            string normalized = text.Replace("\r", " ").Replace("\n", " ");
            return normalized.Length > 30
                ? normalized.Substring(0, 30) + "..."
                : normalized;
        }

        private class SearchResult
        {
            public SearchResult(int index, TodoItem todo)
            {
                Index = index;
                Todo = todo;
            }

            public int Index { get; }
            public TodoItem Todo { get; }
        }
    }
}
