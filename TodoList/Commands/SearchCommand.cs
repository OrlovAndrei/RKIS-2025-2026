namespace TodoApp.Commands
{
	public class SearchCommand : BaseCommand, ICommand
	{
		private readonly TodoList _todoList;

		public string? ContainsText { get; set; }
		public string? StartsWithText { get; set; }
		public string? EndsWithText { get; set; }
		public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }
		public TodoStatus? StatusFilter { get; set; }
		public string? SortBy { get; set; }
		public bool SortDescending { get; set; }
		public int? TopCount { get; set; }

		public SearchCommand(TodoList todoList, Guid? currentProfileId)
		{
			_todoList = todoList;
			CurrentProfileId = currentProfileId;
		}

		public override void Execute()
		{
			if (_todoList == null || _todoList.Count == 0)
			{
				Console.WriteLine("Список задач пуст.");
				return;
			}

			try
			{
				var query = _todoList.AsEnumerable();
				query = ApplyFilters(query);
				query = ApplySorting(query);

				if (TopCount.HasValue && TopCount.Value > 0)
				{
					query = query.Take(TopCount.Value);
				}

				var results = query.ToList();

				if (!results.Any())
				{
					Console.WriteLine("Ничего не найдено.");
					return;
				}

				Console.WriteLine($"Найдено задач: {results.Count}");
				Console.WriteLine();

				var tempList = new TodoList(results);
				tempList.View(showIndex: true, showDone: true, showDate: true, showAll: true);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при выполнении поиска: {ex.Message}");
			}
		}

		private IEnumerable<TodoItem> ApplyFilters(IEnumerable<TodoItem> query)
		{
			if (!string.IsNullOrWhiteSpace(ContainsText))
				query = query.Where(item => item.Text.Contains(ContainsText, StringComparison.OrdinalIgnoreCase));

			if (!string.IsNullOrWhiteSpace(StartsWithText))
				query = query.Where(item => item.Text.StartsWith(StartsWithText, StringComparison.OrdinalIgnoreCase));

			if (!string.IsNullOrWhiteSpace(EndsWithText))
				query = query.Where(item => item.Text.EndsWith(EndsWithText, StringComparison.OrdinalIgnoreCase));

			if (FromDate.HasValue)
				query = query.Where(item => item.LastUpdate >= FromDate.Value);

			if (ToDate.HasValue)
				query = query.Where(item => item.LastUpdate <= ToDate.Value);

			if (StatusFilter.HasValue)
				query = query.Where(item => item.Status == StatusFilter.Value);

			return query;
		}

		private IEnumerable<TodoItem> ApplySorting(IEnumerable<TodoItem> query)
		{
			if (string.IsNullOrWhiteSpace(SortBy))
				return query.OrderByDescending(item => item.LastUpdate);

			return SortBy.ToLower() switch
			{
				"text" => SortDescending
					? query.OrderByDescending(item => item.Text)
					: query.OrderBy(item => item.Text),
				"date" => SortDescending
					? query.OrderByDescending(item => item.LastUpdate)
					: query.OrderBy(item => item.LastUpdate),
				_ => query.OrderByDescending(item => item.LastUpdate)
			};
		}
	}
}