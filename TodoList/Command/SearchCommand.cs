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

        var resultsList = query.ToList();

        if (resultsList.Count == 0)
        {
            Console.WriteLine("Ничего не найдено");
            return;
        }

        var resultsTodoList = new TodoList(resultsList);
        Console.WriteLine($"\nНайдено задач: {resultsList.Count}");
        resultsTodoList.View(true, true, true);
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
        {
            DateTime fromDateStart = FromDate.Value.Date;
            query = query.Where(item => item.LastUpdate.Date >= fromDateStart);
        }

        if (ToDate.HasValue)
        {
            DateTime toDateEnd = ToDate.Value.Date.AddDays(1).AddSeconds(-1);
            query = query.Where(item => item.LastUpdate <= toDateEnd);
        }

        if (Status.HasValue)
            query = query.Where(item => item.Status == Status.Value);

        return query;
    }

    private IEnumerable<TodoItem> ApplySorting(IEnumerable<TodoItem> query)
    {
        if (string.IsNullOrEmpty(SortBy))
            return query.OrderBy(item => 0);

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
}