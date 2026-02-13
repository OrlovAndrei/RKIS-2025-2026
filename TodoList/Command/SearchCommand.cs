using System;
using System.Collections.Generic;
using System.Linq;

public class SearchCommand : ICommand
{
    public string ContainsText { get; set; }
    public string StartsWithText { get; set; }
    public string EndsWithText { get; set; }
    
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    
    public TodoStatus? Status { get; set; }
    
    public string SortBy { get; set; }
    public bool SortDescending { get; set; }
    
    public int? Top { get; set; }
    
    public TodoList TodoList { get; set; }
    
    public void Execute()
    {
        if (TodoList == null || TodoList.Count() == 0)
        {
            Console.WriteLine("Список задач пуст.");
            return;
        }
        
        var query = ApplyFilters(TodoList);
        
        query = ApplySorting(query);
        
        if (Top.HasValue && Top.Value > 0)
        {
            query = query.Take(Top.Value);
        }
        
        var results = query.ToList();
        
        DisplayResults(results);
    }
    
    private IEnumerable<TodoItem> ApplyFilters(TodoList todoList)
    {
        IEnumerable<TodoItem> query = todoList;
        
        if (!string.IsNullOrEmpty(ContainsText))
        {
            query = query.Where(item => item.Text.Contains(ContainsText));
        }
        
        if (!string.IsNullOrEmpty(StartsWithText))
        {
            query = query.Where(item => item.Text.StartsWith(StartsWithText));
        }
        
        if (!string.IsNullOrEmpty(EndsWithText))
        {
            query = query.Where(item => item.Text.EndsWith(EndsWithText));
        }
        
        if (FromDate.HasValue)
        {
            query = query.Where(item => item.LastUpdate >= FromDate.Value);
        }
        
        if (ToDate.HasValue)
        {
            query = query.Where(item => item.LastUpdate <= ToDate.Value);
        }
        
        if (Status.HasValue)
        {
            query = query.Where(item => item.Status == Status.Value);
        }
        
        return query;
    }
    
    private IEnumerable<TodoItem> ApplySorting(IEnumerable<TodoItem> query)
    {
        if (string.IsNullOrEmpty(SortBy))
        {
            return query.OrderBy(item => 0);
        }
        
        if (SortBy.ToLower() == "text")
        {
            return SortDescending 
                ? query.OrderByDescending(item => item.Text) 
                : query.OrderBy(item => item.Text);
        }
        else if (SortBy.ToLower() == "date")
        {
            return SortDescending 
                ? query.OrderByDescending(item => item.LastUpdate) 
                : query.OrderBy(item => item.LastUpdate);
        }
        
        return query.OrderBy(item => 0);
    }

    if (Top.HasValue && Top.Value > 0)
    {
    query = query.Take(Top.Value);
    }

    private void DisplayResults(List<TodoItem> results)
    {
        if (results.Count == 0)
        {
            Console.WriteLine("Ничего не найдено");
            return;
        }
        
        Console.WriteLine($"\nНайдено задач: {results.Count}");
        Console.WriteLine("--------------------------------------------------------------------------------");
        Console.WriteLine("Index | Текст задачи                     | Статус      | Дата изменения");
        Console.WriteLine("--------------------------------------------------------------------------------");
        
        for (int i = 0; i < results.Count; i++)
        {
            var item = results[i];
            string shortText = GetShortText(item.Text, 30);
            string status = GetStatusDisplay(item.Status);
            string date = item.LastUpdate.ToString("dd.MM.yyyy HH:mm");
            
            Console.WriteLine($"{i + 1,-5} | {shortText,-30} | {status,-10} | {date}");
        }
        Console.WriteLine("--------------------------------------------------------------------------------");
    }
    
    private string GetShortText(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text))
            return "";
        
        if (text.Length <= maxLength)
            return text;
        
        return text.Substring(0, maxLength - 3) + "...";
    }
    
    private string GetStatusDisplay(TodoStatus status)
    {
        return status switch
        {
            TodoStatus.NotStarted => "Не начато",
            TodoStatus.InProgress => "В процессе",
            TodoStatus.Completed => "Выполнено",
            TodoStatus.Postponed => "Отложено",
            TodoStatus.Failed => "Провалено",
            _ => "Неизвестно"
        };
    }
    
    public void Unexecute()
    {
    }
}