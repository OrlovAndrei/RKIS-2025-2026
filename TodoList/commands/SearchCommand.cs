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
            var results = new List<(int Index, TodoItem Item)>();

            for (int i = 0; i < AppInfo.CurrentTodos.Count; i++)
            {
                var item = AppInfo.CurrentTodos[i];
                
                // Фильтр по тексту
                if (!MatchesTextFilter(item.Text))
                    continue;

                // Фильтр по статусу
                if (_criteria.Status.HasValue && item.Status != _criteria.Status.Value)
                    continue;

                // Фильтр по дате "от"
                if (_criteria.FromDate.HasValue && item.LastUpdate.Date < _criteria.FromDate.Value)
                    continue;

                // Фильтр по дате "до"
                if (_criteria.ToDate.HasValue && item.LastUpdate.Date > _criteria.ToDate.Value)
                    continue;

                results.Add((i + 1, item));
            }

            // Применяем сортировку
            var sorted = ApplySorting(results);
            
            // Применяем ограничение Top N
            if (_criteria.Top.HasValue && _criteria.Top.Value > 0)
            {
                sorted = sorted.Take(_criteria.Top.Value).ToList();
            }

            return sorted;
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

        private List<(int Index, TodoItem Item)> ApplySorting(List<(int Index, TodoItem Item)> results)
        {
            if (string.IsNullOrEmpty(_criteria.SortBy))
                return results;

            var sorted = _criteria.SortBy.ToLower() switch
            {
                "text" => _criteria.SortDescending
                    ? results.OrderByDescending(x => x.Item.Text).ToList()
                    : results.OrderBy(x => x.Item.Text).ToList(),
                "date" => _criteria.SortDescending
                    ? results.OrderByDescending(x => x.Item.LastUpdate).ToList()
                    : results.OrderBy(x => x.Item.LastUpdate).ToList(),
                _ => results
            };

            return sorted;
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