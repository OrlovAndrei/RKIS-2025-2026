using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoList
{
    public class SearchCommand : ICommand
    {
        private readonly SearchCriteria _criteria;

        public SearchCommand(SearchCriteria criteria)
        {
            _criteria = criteria;
        }

        public void Execute()
        {
            if (AppInfo.CurrentTodos == null)
            {
                Console.WriteLine("Ошибка: нет активного профиля.");
                return;
            }

            var results = PerformSearch();

            if (!results.Any())
            {
                Console.WriteLine("Задачи, соответствующие критериям поиска, не найдены.");
                return;
            }

            DisplayResults(results);
        }

        private List<(int Index, TodoItem Item)> PerformSearch()
        {
            // Получаем все задачи с индексами
            var todosWithIndexes = AppInfo.CurrentTodos
                .Select((item, index) => (Index: index + 1, Item: item));
            
            // Применяем фильтрацию через LINQ Where
            var filtered = todosWithIndexes
                .Where(x => MatchesTextFilter(x.Item.Text))
                .Where(x => MatchesStatusFilter(x.Item.Status))
                .Where(x => MatchesFromDateFilter(x.Item.LastUpdate))
                .Where(x => MatchesToDateFilter(x.Item.LastUpdate));
            
            // Применяем сортировку через LINQ OrderBy/OrderByDescending/ThenBy
            var sorted = ApplySorting(filtered);
            
            // Применяем ограничение через LINQ Take
            var limited = ApplyTopLimit(sorted);
            
            return limited.ToList();
        }

        private bool MatchesTextFilter(string text)
        {
            if (string.IsNullOrEmpty(_criteria.TextFilter))
                return true;

            var comparison = _criteria.CaseSensitive ? 
                StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            return _criteria.TextMatchType switch
            {
                TextMatchType.Contains => text.Contains(_criteria.TextFilter, comparison),
                TextMatchType.StartsWith => text.StartsWith(_criteria.TextFilter, comparison),
                TextMatchType.EndsWith => text.EndsWith(_criteria.TextFilter, comparison),
                _ => true
            };
        }

        private bool MatchesStatusFilter(TodoStatus status)
        {
            if (!_criteria.Status.HasValue)
                return true;
            
            return status == _criteria.Status.Value;
        }

        private bool MatchesFromDateFilter(DateTime lastUpdate)
        {
            if (!_criteria.FromDate.HasValue)
                return true;
            
            return lastUpdate.Date >= _criteria.FromDate.Value;
        }

        private bool MatchesToDateFilter(DateTime lastUpdate)
        {
            if (!_criteria.ToDate.HasValue)
                return true;
            
            return lastUpdate.Date <= _criteria.ToDate.Value;
        }

        private IOrderedEnumerable<(int Index, TodoItem Item)> ApplySorting(
            IEnumerable<(int Index, TodoItem Item)> query)
        {
            if (string.IsNullOrEmpty(_criteria.SortBy))
            {
                // Если сортировка не указана, возвращаем как есть, но нужно привести к IOrderedEnumerable
                // Используем OrderBy с постоянным значением для создания IOrderedEnumerable
                return query.OrderBy(x => 0);
            }

            return _criteria.SortBy.ToLower() switch
            {
                "text" => _criteria.SortDescending
                    ? query.OrderByDescending(x => x.Item.Text)
                    : query.OrderBy(x => x.Item.Text),
                    
                "date" => _criteria.SortDescending
                    ? query.OrderByDescending(x => x.Item.LastUpdate)
                    : query.OrderBy(x => x.Item.LastUpdate),
                    
                _ => query.OrderBy(x => 0)
            };
        }

        private IEnumerable<(int Index, TodoItem Item)> ApplyTopLimit(
            IOrderedEnumerable<(int Index, TodoItem Item)> query)
        {
            if (_criteria.Top.HasValue && _criteria.Top.Value > 0)
            {
                return query.Take(_criteria.Top.Value);
            }
            
            return query;
        }

        private void DisplayResults(List<(int Index, TodoItem Item)> results)
        {
            Console.WriteLine($"\nНайдено задач: {results.Count}");
            if (_criteria.Top.HasValue && results.Count == _criteria.Top.Value)
            {
                Console.WriteLine($"(Показано первых {_criteria.Top.Value})");
            }
            Console.WriteLine(new string('-', 80));

            foreach (var (index, item) in results)
            {
                // Статус с символом
                string statusSymbol = item.Status switch
                {
                    TodoStatus.Completed => "✓",
                    TodoStatus.InProgress => "▶",
                    TodoStatus.Postponed => "⏸",
                    TodoStatus.Failed => "✗",
                    _ => "○"
                };
                
                // Предпросмотр текста
                string preview = item.Text.Length > 50 
                    ? item.Text.Substring(0, 47) + "..." 
                    : item.Text;
                
                // Вывод с отступами
                Console.WriteLine($"[{index,3}] {statusSymbol} {preview}");
                Console.WriteLine($"      Статус: {item.Status,12} | Обновлено: {item.LastUpdate:dd.MM.yyyy HH:mm}");
                Console.WriteLine();
            }
            
            Console.WriteLine(new string('-', 80));
            Console.WriteLine("Для просмотра полной информации используйте read <индекс>");
        }

        public void Unexecute() { }
    }

    // Модель для критериев поиска
    public class SearchCriteria
    {
        public string? TextFilter { get; set; }
        public TextMatchType TextMatchType { get; set; } = TextMatchType.Contains;
        public TodoStatus? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }
        public int? Top { get; set; }
        public bool CaseSensitive { get; set; }
    }

    public enum TextMatchType
    {
        Contains,
        StartsWith,
        EndsWith
    }
}