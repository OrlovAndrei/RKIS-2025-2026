using System;
using System.Collections.Generic;
using System.Linq;
using TodoList.Exceptions;

namespace TodoList.Commands
{
	public class SearchCommand : ICommand
	{
		public string Arg { get; set; } = string.Empty;
		public string[] Flags { get; set; } = Array.Empty<string>();

		public void Execute()
		{
			var todos = AppInfo.CurrentUserTodos;
			if (todos == null)
			{
				throw new AuthenticationException("Не удалось получить список задач. Войдите в профиль.");
			}

			var allTasks = todos.GetAllTasks();
			if (allTasks.Count == 0)
			{
				Console.WriteLine("Список задач пуст");
				return;
			}

			IEnumerable<TodoItem> query = allTasks;

			query = ApplyTextFilters(query);
			query = ApplyDateFilters(query);
			query = ApplyStatusFilter(query);

			query = ApplySorting(query);

			query = ApplyTopLimit(query);

			var resultsList = new TodoList(query.ToList());

			Console.WriteLine("\n=== Результаты поиска ===");

			var viewFlags = new[] { "index", "status", "update-date" };
			resultsList.ViewTasks(viewFlags);

			Console.WriteLine($"Найдено задач: {resultsList.GetAllTasks().Count}");
		}

		private IEnumerable<TodoItem> ApplyTextFilters(IEnumerable<TodoItem> query)
		{
			string containsText = GetFlagValue("contains");
			string startsWithText = GetFlagValue("starts-with");
			string endsWithText = GetFlagValue("ends-with");

			if (!string.IsNullOrEmpty(containsText))
			{
				query = query.Where(t => t.Text.Contains(containsText, StringComparison.OrdinalIgnoreCase));
			}
			else if (!string.IsNullOrEmpty(startsWithText))
			{
				query = query.Where(t => t.Text.StartsWith(startsWithText, StringComparison.OrdinalIgnoreCase));
			}
			else if (!string.IsNullOrEmpty(endsWithText))
			{
				query = query.Where(t => t.Text.EndsWith(endsWithText, StringComparison.OrdinalIgnoreCase));
			}

			return query;
		}

		private IEnumerable<TodoItem> ApplyDateFilters(IEnumerable<TodoItem> query)
		{
			string fromDateStr = GetFlagValue("from");
			string toDateStr = GetFlagValue("to");

			if (!string.IsNullOrEmpty(fromDateStr))
			{
				if (!DateTime.TryParseExact(fromDateStr, "yyyy-MM-dd", 
					System.Globalization.CultureInfo.InvariantCulture, 
					System.Globalization.DateTimeStyles.None, out DateTime fromDate))
				{
					throw new InvalidArgumentException($"Неверный формат даты '{fromDateStr}'. Используйте yyyy-MM-dd.");
				}
				query = query.Where(t => t.LastUpdate.Date >= fromDate.Date);
			}

			if (!string.IsNullOrEmpty(toDateStr))
			{
				if (!DateTime.TryParseExact(toDateStr, "yyyy-MM-dd", 
					System.Globalization.CultureInfo.InvariantCulture, 
					System.Globalization.DateTimeStyles.None, out DateTime toDate))
				{
					throw new InvalidArgumentException($"Неверный формат даты '{toDateStr}'. Используйте yyyy-MM-dd.");
				}
				query = query.Where(t => t.LastUpdate.Date <= toDate.Date);
			}

			return query;
		}

		private IEnumerable<TodoItem> ApplyStatusFilter(IEnumerable<TodoItem> query)
		{
			string statusStr = GetFlagValue("status");

			if (!string.IsNullOrEmpty(statusStr))
			{
				if (!Enum.TryParse<TodoStatus>(statusStr, true, out TodoStatus status))
				{
					throw new InvalidArgumentException($"Неверный статус '{statusStr}'. Допустимые значения: NotStarted, InProgress, Completed, Postponed, Failed");
				}
				query = query.Where(t => t.Status == status);
			}

			return query;
		}

		private IEnumerable<TodoItem> ApplySorting(IEnumerable<TodoItem> query)
		{
			string sortBy = GetFlagValue("sort");
			bool descending = Flags.Contains("desc");

			if (string.IsNullOrEmpty(sortBy))
				return query;

			switch (sortBy.ToLower())
			{
				case "text":
					query = descending
						? query.OrderByDescending(t => t.Text)
						: query.OrderBy(t => t.Text);
					break;

				case "date":
					query = descending
						? query.OrderByDescending(t => t.LastUpdate)
						: query.OrderBy(t => t.LastUpdate);
					break;

				default:
					throw new InvalidArgumentException($"Неизвестный тип сортировки '{sortBy}'. Используйте 'text' или 'date'.");
			}

			return query;
		}

		private IEnumerable<TodoItem> ApplyTopLimit(IEnumerable<TodoItem> query)
		{
			string topStr = GetFlagValue("top");

			if (!string.IsNullOrEmpty(topStr))
			{
				if (!int.TryParse(topStr, out int top) || top <= 0)
				{
					throw new InvalidArgumentException($"Неверное значение для --top '{topStr}'. Должно быть положительное число.");
				}
				query = query.Take(top);
			}

			return query;
		}

		private string GetFlagValue(string flagName)
		{
			int index = Array.IndexOf(Flags, flagName);
			if (index >= 0 && index + 1 < Flags.Length)
			{
				if (!Flags[index + 1].StartsWith("--") && !Flags[index + 1].StartsWith("-"))
				{
					string value = Flags[index + 1];
					if (value.StartsWith("\"") && value.EndsWith("\""))
					{
						return value.Substring(1, value.Length - 2);
					}
					return value;
				}
			}
			return string.Empty;
		}
	}
}