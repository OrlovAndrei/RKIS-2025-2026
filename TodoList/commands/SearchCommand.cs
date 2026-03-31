using System;
using System.Collections.Generic;

namespace TodoList
{
    public enum TextMatchType
    {
        Contains,
        StartsWith,
        EndsWith
    }

    public class SearchCriteria
    {
        public string TextFilter { get; set; } = "";
        public TextMatchType TextMatchType { get; set; } = TextMatchType.Contains;
        public TodoStatus? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string SortBy { get; set; } = "";
        public bool SortDescending { get; set; }
        public int? Top { get; set; }
        public bool CaseSensitive { get; set; }
    }

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

            // Собираем результаты
            List<TodoItem> results = new List<TodoItem>();
            
            for (int i = 0; i < AppInfo.CurrentTodos.Count; i++)
            {
                TodoItem todo = AppInfo.CurrentTodos[i];
                bool include = true;
                
                // Проверка текста
                if (_criteria.TextFilter.Length > 0)
                {
                    string textToCheck = todo.Text;
                    string filter = _criteria.TextFilter;
                    
                    if (!_criteria.CaseSensitive)
                    {
                        textToCheck = textToCheck.ToLower();
                        filter = filter.ToLower();
                    }
                    
                    if (_criteria.TextMatchType == TextMatchType.Contains)
                    {
                        if (!textToCheck.Contains(filter))
                            include = false;
                    }
                    else if (_criteria.TextMatchType == TextMatchType.StartsWith)
                    {
                        if (!textToCheck.StartsWith(filter))
                            include = false;
                    }
                    else if (_criteria.TextMatchType == TextMatchType.EndsWith)
                    {
                        if (!textToCheck.EndsWith(filter))
                            include = false;
                    }
                }
                
                // Проверка статуса
                if (include && _criteria.Status.HasValue)
                {
                    if (todo.Status != _criteria.Status.Value)
                        include = false;
                }
                
                // Проверка даты "от"
                if (include && _criteria.FromDate.HasValue)
                {
                    if (todo.LastUpdate.Date < _criteria.FromDate.Value)
                        include = false;
                }
                
                // Проверка даты "до"
                if (include && _criteria.ToDate.HasValue)
                {
                    if (todo.LastUpdate.Date > _criteria.ToDate.Value)
                        include = false;
                }
                
                if (include)
                {
                    results.Add(todo);
                }
            }
            
            // Сортировка
            if (_criteria.SortBy.Length > 0)
            {
                if (_criteria.SortBy.ToLower() == "text")
                {
                    if (_criteria.SortDescending)
                        results.Sort((a, b) => b.Text.CompareTo(a.Text));
                    else
                        results.Sort((a, b) => a.Text.CompareTo(b.Text));
                }
                else if (_criteria.SortBy.ToLower() == "date")
                {
                    if (_criteria.SortDescending)
                        results.Sort((a, b) => b.LastUpdate.CompareTo(a.LastUpdate));
                    else
                        results.Sort((a, b) => a.LastUpdate.CompareTo(b.LastUpdate));
                }
            }
            
            // Ограничение
            if (_criteria.Top.HasValue && _criteria.Top.Value > 0 && results.Count > _criteria.Top.Value)
            {
                List<TodoItem> limited = new List<TodoItem>();
                for (int i = 0; i < _criteria.Top.Value; i++)
                {
                    limited.Add(results[i]);
                }
                results = limited;
            }
            
            // Вывод результатов
            if (results.Count == 0)
            {
                Console.WriteLine("Задачи, соответствующие критериям поиска, не найдены.");
                return;
            }
            
            Console.WriteLine($"\nНайдено задач: {results.Count}");
            
            // Вывод критериев
            List<string> criteriaList = new List<string>();
            if (_criteria.TextFilter.Length > 0)
            {
                string matchType = "";
                if (_criteria.TextMatchType == TextMatchType.Contains)
                    matchType = "содержит";
                else if (_criteria.TextMatchType == TextMatchType.StartsWith)
                    matchType = "начинается с";
                else if (_criteria.TextMatchType == TextMatchType.EndsWith)
                    matchType = "заканчивается на";
                    
                string caseInfo = _criteria.CaseSensitive ? " (с учетом регистра)" : "";
                criteriaList.Add($"текст {matchType} \"{_criteria.TextFilter}\"{caseInfo}");
            }
            
            if (_criteria.Status.HasValue)
            {
                criteriaList.Add($"статус = {_criteria.Status.Value}");
            }
            
            if (_criteria.FromDate.HasValue)
            {
                criteriaList.Add($"дата >= {_criteria.FromDate.Value:dd.MM.yyyy}");
            }
            
            if (_criteria.ToDate.HasValue)
            {
                criteriaList.Add($"дата <= {_criteria.ToDate.Value:dd.MM.yyyy}");
            }
            
            if (criteriaList.Count > 0)
            {
                Console.WriteLine($"Критерии: {string.Join(", ", criteriaList)}");
            }
            
            if (_criteria.SortBy.Length > 0)
            {
                string sortOrder = _criteria.SortDescending ? "убывание" : "возрастание";
                string sortField = _criteria.SortBy == "text" ? "тексту" : "дате";
                Console.WriteLine($"Сортировка: по {sortField} ({sortOrder})");
            }
            
            Console.WriteLine();
            Console.WriteLine("Индекс | Статус | Текст");
            Console.WriteLine(new string('-', 60));
            
            // Выводим результаты
            for (int i = 0; i < results.Count; i++)
            {
                TodoItem todo = results[i];
                
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
                
                string statusSymbol = "";
                if (todo.Status == TodoStatus.Completed)
                    statusSymbol = "✓";
                else if (todo.Status == TodoStatus.InProgress)
                    statusSymbol = "▶";
                else if (todo.Status == TodoStatus.Postponed)
                    statusSymbol = "⏸";
                else if (todo.Status == TodoStatus.Failed)
                    statusSymbol = "✗";
                else
                    statusSymbol = "○";
                
                string preview = todo.Text;
                if (todo.Text.Length > 50)
                {
                    preview = todo.Text.Substring(0, 47) + "...";
                }
                
                Console.WriteLine($"{realIndex,6} | {statusSymbol} {todo.Status,-12} | {preview}");
            }
            
            Console.WriteLine("\nДля просмотра полной информации используйте read <индекс>");
        }

        public void Unexecute() { }
    }
}