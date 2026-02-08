using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoApp.Commands
{
	public class SearchCommand : BaseCommand
	{
		private readonly TodoList _todoList;
		private readonly string _searchQuery;

		public string ContainsText { get; set; }
		public string StartsWithText { get; set; }
		public string EndsWithText { get; set; }
		public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }
		public TodoStatus? StatusFilter { get; set; }
		public string SortBy { get; set; }
		public bool SortDescending { get; set; }
		public int? TopCount { get; set; }
		public bool ShowIndex { get; set; } = true;
		public bool ShowStatus { get; set; } = true;
		public bool ShowDate { get; set; } = true;

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

				DisplayResults(results);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка при выполнении поиска: {ex.Message}");
			}
		}

		private IEnumerable<TodoItem> ApplyFilters(IEnumerable<TodoItem> query)
		{
			if (!string.IsNullOrWhiteSpace(ContainsText))
			{
				query = query.Where(item => item.Text.Contains(ContainsText, StringComparison.OrdinalIgnoreCase));
			}

			if (!string.IsNullOrWhiteSpace(StartsWithText))
			{
				query = query.Where(item => item.Text.StartsWith(StartsWithText, StringComparison.OrdinalIgnoreCase));
			}

			if (!string.IsNullOrWhiteSpace(EndsWithText))
			{
				query = query.Where(item => item.Text.EndsWith(EndsWithText, StringComparison.OrdinalIgnoreCase));
			}

			if (FromDate.HasValue)
			{
				query = query.Where(item => item.LastUpdate >= FromDate.Value);
			}

			if (ToDate.HasValue)
			{
				query = query.Where(item => item.LastUpdate <= ToDate.Value);
			}

			if (StatusFilter.HasValue)
			{
				query = query.Where(item => item.Status == StatusFilter.Value);
			}

			return query;
		}

		private IEnumerable<TodoItem> ApplySorting(IEnumerable<TodoItem> query)
		{
			if (string.IsNullOrWhiteSpace(SortBy))
			{
				return query.OrderByDescending(item => item.LastUpdate);
			}

			switch (SortBy.ToLower())
			{
				case "text":
					return SortDescending
						? query.OrderByDescending(item => item.Text)
						: query.OrderBy(item => item.Text);

				case "date":
					return SortDescending
						? query.OrderByDescending(item => item.LastUpdate)
						: query.OrderBy(item => item.LastUpdate);

				default:
					return query.OrderByDescending(item => item.LastUpdate);
			}
		}

		private void DisplayResults(List<TodoItem> results)
		{
			if (!results.Any())
			{
				Console.WriteLine("Ничего не найдено.");
				return;
			}

			Console.WriteLine($"Найдено задач: {results.Count}");
			Console.WriteLine();


			var table = new List<string[]>();


			var headers = new List<string>();
			if (ShowIndex) headers.Add("№");
			headers.Add("Текст задачи");
			if (ShowStatus) headers.Add("Статус");
			if (ShowDate) headers.Add("Дата изменения");
			table.Add(headers.ToArray());


			for (int i = 0; i < results.Count; i++)
			{
				var item = results[i];
				var row = new List<string>();

				if (ShowIndex) row.Add((i + 1).ToString());


				string shortText = item.Text.Length > 30
					? item.Text.Substring(0, 27) + "..."
					: item.Text;
				row.Add(shortText);

				if (ShowStatus) row.Add(TodoItem.GetStatusDisplayName(item.Status));
				if (ShowDate) row.Add(item.LastUpdate.ToString("dd.MM.yyyy HH:mm"));

				table.Add(row.ToArray());
			}

			PrintTable(table);
		}

		private void PrintTable(List<string[]> table)
		{
			if (table.Count == 0) return;

			int[] columnWidths = new int[table[0].Length];
			for (int i = 0; i < table.Count; i++)
			{
				for (int j = 0; j < table[i].Length; j++)
				{
					if (table[i][j].Length > columnWidths[j])
						columnWidths[j] = table[i][j].Length;
				}
			}

			if (columnWidths.Length > 1 && columnWidths[1] > 50)
				columnWidths[1] = 50;

			Console.Write("┌");
			for (int j = 0; j < columnWidths.Length; j++)
			{
				Console.Write(new string('─', columnWidths[j] + 2));
				if (j < columnWidths.Length - 1) Console.Write("┬");
			}
			Console.WriteLine("┐");

			for (int i = 0; i < table.Count; i++)
			{
				Console.Write("│");
				for (int j = 0; j < table[i].Length; j++)
				{
					Console.Write(" " + table[i][j].PadRight(columnWidths[j] + 1) + "│");
				}
				Console.WriteLine();

				if (i == 0)
				{
					Console.Write("├");
					for (int j = 0; j < columnWidths.Length; j++)
					{
						Console.Write(new string('─', columnWidths[j] + 2));
						if (j < columnWidths.Length - 1) Console.Write("┼");
					}
					Console.WriteLine("┤");
				}
			}

			Console.Write("└");
			for (int j = 0; j < columnWidths.Length; j++)
			{
				Console.Write(new string('─', columnWidths[j] + 2));
				if (j < columnWidths.Length - 1) Console.Write("┴");
			}
			Console.WriteLine("┘");
		}

		public override void Unexecute()
		{
			Console.WriteLine("Отмена поиска (команда поиска не изменяет данные)");
		}
	}
}