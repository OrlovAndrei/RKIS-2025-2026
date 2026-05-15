using System;
using System.Collections.Generic;
using System.Linq;

public class SearchCommand : ICommand
{
    public string ContainsText { get; set; } = "";
    public string StartsWithText { get; set; } = "";
    public string EndsWithText { get; set; } = "";
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public TodoStatus? Status { get; set; }
    public string SortBy { get; set; } = "";
    public bool SortDescending { get; set; }
    public int? Top { get; set; }
    public TodoList TodoList { get; set; } = null!;

    public void Execute()
    {
        if (TodoList == null || TodoList.Count == 0)
        {
            Console.WriteLine("Список задач пуст.");
            return;
        }

        var query = ApplyFilters(TodoList);
        query = ApplySorting(query);
        if (Top.HasValue && Top.Value > 0) query = query.Take(Top.Value);
        var results = query.ToList();
        if (results.Count == 0)
        {
            Console.WriteLine("Ничего не найдено");
            return;
        }
        var resultList = new TodoList();
        foreach (var r in results) resultList.Add(r);
        Console.WriteLine($"\nНайдено задач: {results.Count}");
        resultList.View(true, true, true);
    }

    private IEnumerable<TodoItem> ApplyFilters(TodoList todoList)
    {
        IEnumerable<TodoItem> query = todoList;
        if (!string.IsNullOrEmpty(ContainsText))
            query = query.Where(item => item.Text.Contains(ContainsText));
        if (!string.IsNullOrEmpty(StartsWithText))
            query = query.Where(item => item.Text.StartsWith(StartsWithText));
        if (!string.IsNullOrEmpty(EndsWithText))
            query = query.Where(item => item.Text.EndsWith(EndsWithText));
        if (FromDate.HasValue)
            query = query.Where(item => item.LastUpdate.Date >= FromDate.Value.Date);
        if (ToDate.HasValue)
            query = query.Where(item => item.LastUpdate.Date <= ToDate.Value.Date);
        if (Status.HasValue)
            query = query.Where(item => item.Status == Status.Value);
        return query;
    }

    private IEnumerable<TodoItem> ApplySorting(IEnumerable<TodoItem> query)
    {
        if (string.IsNullOrEmpty(SortBy)) return query;
        if (SortBy.ToLower() == "text")
            return SortDescending ? query.OrderByDescending(i => i.Text) : query.OrderBy(i => i.Text);
        if (SortBy.ToLower() == "date")
            return SortDescending ? query.OrderByDescending(i => i.LastUpdate) : query.OrderBy(i => i.LastUpdate);
        return query;
    }
}