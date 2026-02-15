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
		var query = Todos.Select((item, index) => new { Item = item, OriginalIndex = index });
		if (!string.IsNullOrEmpty(Contains))
			query = query.Where(x => x.Item.GetText().Contains(Contains, StringComparison.OrdinalIgnoreCase));
		if (!string.IsNullOrEmpty(StartsWith))
			query = query.Where(x => x.Item.GetText().StartsWith(StartsWith, StringComparison.OrdinalIgnoreCase));
		if (!string.IsNullOrEmpty(EndsWith))
			query = query.Where(x => x.Item.GetText().EndsWith(EndsWith, StringComparison.OrdinalIgnoreCase));
		if (FromDate.HasValue)
			query = query.Where(x => x.Item.GetLastUpdate().Date >= FromDate.Value.Date);
		if (ToDate.HasValue)
			query = query.Where(x => x.Item.GetLastUpdate().Date <= ToDate.Value.Date);
		if (Status.HasValue)
			query = query.Where(x => x.Item.GetStatus() == Status.Value);
		if (!string.IsNullOrEmpty(SortBy))
		{
			bool isText = SortBy == "text";
			if (Descending)
			{
				var ordered = isText
					? query.OrderByDescending(x => x.Item.GetText())
					: query.OrderByDescending(x => x.Item.GetLastUpdate());
				query = ordered.ThenBy(x => x.OriginalIndex);
			}
			else
			{
				var ordered = isText
					? query.OrderBy(x => x.Item.GetText())
					: query.OrderBy(x => x.Item.GetLastUpdate());
				query = ordered.ThenBy(x => x.OriginalIndex);
			}
		}
		if (Top.HasValue)
			query = query.Take(Top.Value);
		var finalResults = new TodoList(query.Select(x => x.Item).ToList());
		finalResults.View();
	}
	public void Unexecute() 
	{
	}
}