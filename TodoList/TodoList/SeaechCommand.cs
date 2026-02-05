using System;
using System.Collections.Generic;
using System.Linq;
public class SearchCommand : ICommand
{
	public TodoList Todos { get; set; }
	public string Contains { get; set; }
	public string StartsWith { get; set; }
	public string EndsWith { get; set; }
	public DateTime? FromDate { get; set; }
	public DateTime? ToDate { get; set; }
	public TodoStatus? Status { get; set; }
	public string SortBy { get; set; }
	public bool Descending { get; set; }
	public int? Top { get; set; }

	public void Execute()
	{
		var query = Todos.AsEnumerable();
		if (!string.IsNullOrEmpty(Contains))
			query = query.Where(t => t.GetText().Contains(Contains, StringComparison.OrdinalIgnoreCase));
		if (!string.IsNullOrEmpty(StartsWith))
			query = query.Where(t => t.GetText().StartsWith(StartsWith, StringComparison.OrdinalIgnoreCase));
		if (!string.IsNullOrEmpty(EndsWith))
			query = query.Where(t => t.GetText().EndsWith(EndsWith, StringComparison.OrdinalIgnoreCase));
		if (FromDate.HasValue)
			query = query.Where(t => t.GetLastUpdate().Date >= FromDate.Value.Date);
		if (ToDate.HasValue)
			query = query.Where(t => t.GetLastUpdate().Date <= ToDate.Value.Date);
		if (Status.HasValue)
			query = query.Where(t => t.GetStatus() == Status.Value);
		IOrderedEnumerable<TodoItem> orderedQuery;
		if (SortBy == "text")
		{
			orderedQuery = Descending
				? query.OrderByDescending(t => t.GetText())
				: query.OrderBy(t => t.GetText());
			query = orderedQuery.ThenBy(t => t.GetLastUpdate());
		}
		else if (SortBy == "date")
		{
			orderedQuery = Descending
				? query.OrderByDescending(t => t.GetLastUpdate())
				: query.OrderBy(t => t.GetLastUpdate());
			query = orderedQuery.ThenBy(t => t.GetText());
		}
		if (Top.HasValue)
			query = query.Take(Top.Value);
		var finalItems = query.ToList();
		if (!finalItems.Any())
		{
			Console.WriteLine("Задачи не найдены.");
			return;
		}
		Console.WriteLine($"Найдено задач: {finalItems.Count}");
		Console.WriteLine(new string('-', 50));
		finalItems.ForEach(item =>
			Console.WriteLine($"[{item.GetStatusText()}] {item.GetText()} ({item.GetLastUpdate():yyyy-MM-dd})")
		);
	}
	public void Unexecute() 
	{
	}
}