using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoList
{
    public enum TextMatchType
    {
        Contains,
        StartsWith,
        EndsWith
    }

    public class SearchCommand : ICommand
    {
        private readonly string _searchText;
        private readonly TodoStatus? _statusFilter;
        private readonly DateTime? _fromDate;
        private readonly DateTime? _toDate;
        private readonly string _sortBy;
        private readonly bool _sortDescending;
        private readonly int? _top;
        private readonly bool _caseSensitive;
        private readonly TextMatchType _matchType;

        public SearchCommand(
            string searchText = "",
            TodoStatus? statusFilter = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            string sortBy = "",
            bool sortDescending = false,
            int? top = null,
            bool caseSensitive = false,
            TextMatchType matchType = TextMatchType.Contains)
        {
            _searchText = searchText ?? "";
            _statusFilter = statusFilter;
            _fromDate = fromDate;
            _toDate = toDate;
            _sortBy = sortBy ?? "";
            _sortDescending = sortDescending;
            _top = top;
            _caseSensitive = caseSensitive;
            _matchType = matchType;
        }

        public void Execute()
        {
            if (AppInfo.CurrentTodos == null)
            {
                Console.WriteLine("Ошибка: нет активного профиля.");
                return;
            }

            // Выполняем поиск через LINQ
            var results = PerformSearch();

            // Проверка на пустой результат
            if (!results.Any())
            {
                Console.WriteLine("Ничего не найдено");
                return;
            }

            // Выводим результаты
            DisplayResults(results);
        }

        private List<TodoItem> PerformSearch()
        {
            var query = AppInfo.CurrentTodos.AsEnumerable();

            // Фильтр по тексту
            if (!string.IsNullOrEmpty(_searchText))
            {
                var comparison = _caseSensitive ? 
                    StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

                query = _matchType switch
                {
                    TextMatchType.Contains => query.Where(t => t.Text.Contains(_searchText, comparison)),
                    TextMatchType.StartsWith => query.Where(t => t.Text.StartsWith(_searchText, comparison)),
                    TextMatchType.EndsWith => query.Where(t => t.Text.EndsWith(_searchText, comparison)),
                    _ => query.Where(t => t.Text.Contains(_searchText, comparison))
                };
            }

            // Фильтр по статусу
            if (_statusFilter.HasValue)
            {
                var status = _statusFilter.Value;
                query = query.Where(t => t.Status == status);
            }

            // Фильтр по дате "от"
            if (_fromDate.HasValue)
            {
                var from = _fromDate.Value;
                query = query.Where(t => t.LastUpdate.Date >= from);
            }

            // Фильтр по дате "до"
            if (_toDate.HasValue)
            {
                var to = _toDate.Value;
                query = query.Where(t => t.LastUpdate.Date <= to);
            }

            // Сортировка
            if (!string.IsNullOrEmpty(_sortBy))
            {
                if (_sortBy.ToLower() == "text")
                {
                    query = _sortDescending 
                        ? query.OrderByDescending(t => t.Text)
                        : query.OrderBy(t => t.Text);
                }
                else if (_sortBy.ToLower() == "date")
                {
                    query = _sortDescending 
                        ? query.OrderByDescending(t => t.LastUpdate)
                        : query.OrderBy(t => t.LastUpdate);
                }
            }

            // Ограничение
            if (_top.HasValue && _top.Value > 0)
            {
                query = query.Take(_top.Value);
            }

            return query.ToList();
        }

        private void DisplayResults(List<TodoItem> results)
        {
            Console.WriteLine($"\nРезультаты поиска: найдено {results.Count} задач");
            
            // Выводим критерии поиска
            var criteria = new List<string>();
            
            if (!string.IsNullOrEmpty(_searchText))
            {
                string matchType = _matchType switch
                {
                    TextMatchType.Contains => "содержит",
                    TextMatchType.StartsWith => "начинается с",
                    TextMatchType.EndsWith => "заканчивается на",
                    _ => "содержит"
                };
                string caseInfo = _caseSensitive ? " (с учетом регистра)" : "";
                criteria.Add($"текст {matchType} \"{_searchText}\"{caseInfo}");
            }
            
            if (_statusFilter.HasValue)
            {
                criteria.Add($"статус = {_statusFilter.Value}");
            }
            
            if (_fromDate.HasValue)
            {
                criteria.Add($"дата >= {_fromDate.Value:dd.MM.yyyy}");
            }
            
            if (_toDate.HasValue)
            {
                criteria.Add($"дата <= {_toDate.Value:dd.MM.yyyy}");
            }
            
            if (criteria.Any())
            {
                Console.WriteLine($"Критерии: {string.Join(", ", criteria)}");
            }
            
            if (!string.IsNullOrEmpty(_sortBy))
            {
                string sortOrder = _sortDescending ? "убывание" : "возрастание";
                string sortField = _sortBy == "text" ? "тексту" : "дате";
                Console.WriteLine($"Сортировка: по {sortField} ({sortOrder})");
            }
            
            if (_top.HasValue && results.Count == _top.Value)
            {
                Console.WriteLine($"(Показано первых {_top.Value})");
            }
            
            Console.WriteLine();
            
            // Выводим таблицу
            Console.WriteLine("Индекс | Статус | Текст | Дата изменения");
            Console.WriteLine(new string('-', 80));
            
            for (int i = 0; i < results.Count; i++)
            {
                var todo = results[i];
                
                // Находим реальный индекс
                int realIndex = -1;
                for (int j = 0; j < AppInfo.CurrentTodos.Count; j++)
                {
                    if (AppInfo.CurrentTodos[j] == todo)
                    {
                        realIndex = j + 1;
                        break;
                    }
                }
                
                string statusSymbol = todo.Status switch
                {
                    TodoStatus.Completed => "✓",
                    TodoStatus.InProgress => "▶",
                    TodoStatus.Postponed => "⏸",
                    TodoStatus.Failed => "✗",
                    _ => "○"
                };
                
                string preview = todo.Text.Length > 40 
                    ? todo.Text.Substring(0, 37) + "..." 
                    : todo.Text;
                
                string formattedDate = todo.LastUpdate.ToString("dd.MM.yyyy HH:mm");
                
                Console.WriteLine($"{realIndex,6} | {statusSymbol} {todo.Status,-12} | {preview,-40} | {formattedDate}");
            }
            
            Console.WriteLine(new string('-', 80));
            Console.WriteLine("\nДля просмотра полной информации используйте read <индекс>");
        }

        public void Unexecute() { }
    }
}