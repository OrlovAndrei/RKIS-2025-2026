using System.Collections.Generic;
using System.Linq;

namespace TodoList
{
    public class SearchCommand : ICommand
    {
        private readonly string _containsText;
        private readonly string _startsWithText;
        private readonly string _endsWithText;
        private readonly DateTime? _fromDate;
        private readonly DateTime? _toDate;
        private readonly TodoStatus? _status;
        private readonly string _sortBy;
        private readonly bool _descending;
        private readonly int? _topCount;

        public SearchCommand(
            string containsText = null,
            string startsWithText = null,
            string endsWithText = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            TodoStatus? status = null,
            string sortBy = null,
            bool descending = false,
            int? topCount = null)
        {
            _containsText = containsText;
            _startsWithText = startsWithText;
            _endsWithText = endsWithText;
            _fromDate = fromDate;
            _toDate = toDate;
            _status = status;
            _sortBy = sortBy;
            _descending = descending;
            _topCount = topCount;
        }

        public void Execute()
        {
            if (AppInfo.CurrentTodos == null)
            {
                Console.WriteLine("Ошибка: нет активного профиля.");
                return;
            }

            var query = AppInfo.CurrentTodos.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(_containsText))
            {
                query = query.Where(t => t.Text.Contains(_containsText, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(_startsWithText))
            {
                query = query.Where(t => t.Text.StartsWith(_startsWithText, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(_endsWithText))
            {
                query = query.Where(t => t.Text.EndsWith(_endsWithText, StringComparison.OrdinalIgnoreCase));
            }

            if (_fromDate.HasValue)
            {
                query = query.Where(t => t.LastUpdate >= _fromDate.Value.Date);
            }

            if (_toDate.HasValue)
            {
                query = query.Where(t => t.LastUpdate <= _toDate.Value.Date.AddDays(1).AddSeconds(-1));
            }

            if (_status.HasValue)
            {
                query = query.Where(t => t.Status == _status.Value);
            }

            if (!string.IsNullOrWhiteSpace(_sortBy))
            {
                if (_sortBy.Equals("text", StringComparison.OrdinalIgnoreCase))
                {
                    query = _descending 
                        ? query.OrderByDescending(t => t.Text)
                        : query.OrderBy(t => t.Text);
                }
                else if (_sortBy.Equals("date", StringComparison.OrdinalIgnoreCase))
                {
                    query = _descending
                        ? query.OrderByDescending(t => t.LastUpdate)
                        : query.OrderBy(t => t.LastUpdate);
                }
            }

            if (_topCount.HasValue && _topCount.Value > 0)
            {
                query = query.Take(_topCount.Value);
            }

            var results = query.ToList();

            if (results.Count == 0)
            {
                Console.WriteLine("Ничего не найдено");
                return;
            }

            PrintTable(results);
        }

        private void PrintTable(List<TodoItem> items)
        {
            Console.WriteLine("Index | Text | Status | LastUpdate");
            Console.WriteLine(new string('-', 80));
            
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var shortText = item.Text.Length > 30 
                    ? item.Text.Substring(0, 27) + "..." 
                    : item.Text;
                
                var originalIndex = GetOriginalIndex(item);
                
                Console.WriteLine($"{originalIndex,5} | {shortText,-30} | {item.Status,-12} | {item.LastUpdate:yyyy-MM-dd HH:mm}");
            }
        }

        private int GetOriginalIndex(TodoItem item)
        {
            if (AppInfo.CurrentTodos == null)
                return -1;
                
            for (int i = 0; i < AppInfo.CurrentTodos.Count; i++)
            {
                if (AppInfo.CurrentTodos[i] == item)
                    return i + 1;
            }
            return -1;
        }

        public void Unexecute()
        {
        }
    }
}