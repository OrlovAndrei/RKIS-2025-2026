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
		var results = Todos.AsEnumerable();
		if (!string.IsNullOrEmpty(Contains))
			results = results.Where(t => t.GetText().Contains(Contains, StringComparison.OrdinalIgnoreCase));
		if (!string.IsNullOrEmpty(StartsWith))
			results = results.Where(t => t.GetText().StartsWith(StartsWith, StringComparison.OrdinalIgnoreCase));
		if (!string.IsNullOrEmpty(EndsWith))
			results = results.Where(t => t.GetText().EndsWith(EndsWith, StringComparison.OrdinalIgnoreCase));
		if (FromDate.HasValue)
			results = results.Where(t => t.GetLastUpdate().Date >= FromDate.Value.Date);
		if (ToDate.HasValue)
			results = results.Where(t => t.GetLastUpdate().Date <= ToDate.Value.Date);
		if (Status.HasValue)
			results = results.Where(t => t.GetStatus() == Status.Value);
		if (!string.IsNullOrEmpty(SortBy))
		{
			if (SortBy == "text")
				results = Descending ? results.OrderByDescending(t => t.GetText()) : results.OrderBy(t => t.GetText());
			else if (SortBy == "date")
				results = Descending ? results.OrderByDescending(t => t.GetLastUpdate()) : results.OrderBy(t => t.GetLastUpdate());
		}
		if (Top.HasValue)
			results = results.Take(Top.Value);
		var finalItems = results.ToList();
		if (finalItems.Count == 0)
		{
			Console.WriteLine("Задачи не найдены.");
			return;
		}
		Console.WriteLine($"Найдено задач: {finalItems.Count}");
		Console.WriteLine("--------------------------------------------------");
		foreach (var item in finalItems)
		{
			Console.WriteLine($"[{item.GetStatusText()}] {item.GetText()} ({item.GetLastUpdate():yyyy-MM-dd})");
		}
	}

	public void Unexecute()
	{
	}
}
