using System.Collections.Generic;
using System.Linq;
using TodoList.Exceptions;  // добавлен using

namespace TodoList
{
    public class SearchCommand : ICommand
    {
        private readonly SearchFlags _flags;
        public SearchCommand(SearchFlags searchFlags)
        {
            _flags = searchFlags ?? new SearchFlags(); 
        }

        public void Execute()
        {
            if (AppInfo.CurrentTodos == null)
                throw new AuthenticationException("Необходимо войти в профиль.");

            var query = AppInfo.CurrentTodos.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(_flags.ContainsText))
            {
                query = query.Where(t => t.Text.Contains(_flags.ContainsText, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(_flags.StartsWithText))
            {
                query = query.Where(t => t.Text.StartsWith(_flags.StartsWithText, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(_flags.EndsWithText))
            {
                query = query.Where(t => t.Text.EndsWith(_flags.EndsWithText, StringComparison.OrdinalIgnoreCase));
            }

            if (_flags.FromDate.HasValue)
            {
                query = query.Where(t => t.LastUpdate >= _flags.FromDate.Value.Date);
            }

            if (_flags.ToDate.HasValue)
            {
                query = query.Where(t => t.LastUpdate <= _flags.ToDate.Value.Date.AddDays(1).AddSeconds(-1));
            }

            if (_flags.Status.HasValue)
            {
                query = query.Where(t => t.Status == _flags.Status.Value);
            }

            if (!string.IsNullOrWhiteSpace(_flags.SortBy))
            {
                if (_flags.SortBy.Equals("text", StringComparison.OrdinalIgnoreCase))
                {
                    query = _flags.Descending
                        ? query.OrderByDescending(t => t.Text)
                        : query.OrderBy(t => t.Text);
                }
                else if (_flags.SortBy.Equals("date", StringComparison.OrdinalIgnoreCase))
                {
                    query = _flags.Descending
                        ? query.OrderByDescending(t => t.LastUpdate)
                        : query.OrderBy(t => t.LastUpdate);
                }
            }

            if (_flags.TopCount.HasValue && _flags.TopCount.Value > 0)
            {
                query = query.Take(_flags.TopCount.Value);
            }

            var results = new TodoList(query.ToList());

            if (results.Count == 0)
            {
                Console.WriteLine("Ничего не найдено");
                return;
            }

            results.View(true, true, true);
        }

        public void Unexecute() { }
    }
}