namespace TodoList
{
    public class SearchCommand : ICommand
    {
        private readonly string _searchText;
        private readonly TodoStatus? _statusFilter;
        private readonly bool _caseSensitive;
        private readonly bool _useRegex;

        public SearchCommand(string searchText, TodoStatus? statusFilter = null, 
                            bool caseSensitive = false, bool useRegex = false)
        {
            _searchText = searchText;
            _statusFilter = statusFilter;
            _caseSensitive = caseSensitive;
            _useRegex = useRegex;
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
            var comparison = _caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

            for (int i = 0; i < AppInfo.CurrentTodos.Count; i++)
            {
                var item = AppInfo.CurrentTodos[i];
                
                // Фильтр по статусу
                if (_statusFilter.HasValue && item.Status != _statusFilter.Value)
                    continue;

                // Поиск по тексту
                if (!string.IsNullOrEmpty(_searchText))
                {
                    bool matches = false;
                    
                    if (_useRegex)
                    {
                        try
                        {
                            var regex = new System.Text.RegularExpressions.Regex(_searchText, 
                                _caseSensitive ? System.Text.RegularExpressions.RegexOptions.None : 
                                                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                            matches = regex.IsMatch(item.Text);
                        }
                        catch
                        {
                            matches = item.Text.Contains(_searchText, comparison);
                        }
                    }
                    else
                    {
                        matches = item.Text.Contains(_searchText, comparison);
                    }
                    
                    if (!matches)
                        continue;
                }

                results.Add((i + 1, item));
            }

            return results;
        }

        private void DisplayResults(List<(int Index, TodoItem Item)> results)
        {
            Console.WriteLine($"\nНайдено задач: {results.Count}");
            Console.WriteLine(new string('-', 80));

            foreach (var (index, item) in results)
            {
                // Краткий вывод с индексом и статусом
                string statusSymbol = item.Status switch
                {
                    TodoStatus.Completed => "✓",
                    TodoStatus.InProgress => "▶",
                    TodoStatus.Postponed => "⏸",
                    TodoStatus.Failed => "✗",
                    _ => "○"
                };
                
                string preview = item.Text.Length > 50 
                    ? item.Text.Substring(0, 47) + "..." 
                    : item.Text;
                
                Console.WriteLine($"[{index,3}] {statusSymbol} {preview}");
                Console.WriteLine($"      Статус: {item.Status} | Обновлено: {item.LastUpdate:dd.MM.yyyy HH:mm}");
                Console.WriteLine();
            }
            
            Console.WriteLine(new string('-', 80));
            Console.WriteLine("Для просмотра полной информации используйте read <индекс>");
        }

        public void Unexecute() { }
    }
}