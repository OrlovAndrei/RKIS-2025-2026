using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TodoList;
using TodoList.Exceptions;
namespace TodoList.Commands
{
	public class SearchCommand : ICommand
	{
		private string _rawArgs;
		private string? _contains;
		private string? _startsWith;
		private string? _endsWith;
		private DateTime? _dateFrom;
		private DateTime? _dateTo;
		private TodoStatus? _status;
		private string? _sortBy;
		private bool _desc;
		private int? _top;
		public SearchCommand(string args)
		{
			_rawArgs = args;
		}
		public void Execute()
		{
			if (AppInfo.CurrentProfile == null)
			{
				throw new AuthenticationException("Для поиска задач необходимо авторизоваться.");
			}
			ParseArguments();

			var todoList = AppInfo.CurrentUserTodos;
			if (todoList == null || todoList.Count == 0)
			{
				Console.WriteLine("Список задач пуст или не найден.");
				return;
			}

			var query = todoList.AsEnumerable();
			if (!string.IsNullOrEmpty(_contains))
				query = query.Where(x => x.Text.IndexOf(_contains, StringComparison.OrdinalIgnoreCase) >= 0);
			if (!string.IsNullOrEmpty(_startsWith))
				query = query.Where(x => x.Text.Trim().StartsWith(_startsWith, StringComparison.OrdinalIgnoreCase));
			if (!string.IsNullOrEmpty(_endsWith))
				query = query.Where(x => x.Text.Trim().EndsWith(_endsWith, StringComparison.OrdinalIgnoreCase));
			if (_status.HasValue)
				query = query.Where(x => x.Status == _status.Value);
			if (_dateFrom.HasValue)
				query = query.Where(x => x.LastUpdate.Date >= _dateFrom.Value);
			if (_dateTo.HasValue)
				query = query.Where(x => x.LastUpdate.Date <= _dateTo.Value);
			if (!string.IsNullOrEmpty(_sortBy))
			{
				if (_sortBy == "text")
					query = _desc ? query.OrderByDescending(x => x.Text) : query.OrderBy(x => x.Text);
				else if (_sortBy == "date")
					query = _desc ? query.OrderByDescending(x => x.LastUpdate) : query.OrderBy(x => x.LastUpdate);
			}
			if (_top.HasValue)
				query = query.Take(_top.Value);

			var results = query.ToList();
			if (results.Any())
			{
				Console.WriteLine($"\n--- Результаты поиска (найдено: {results.Count}) ---");
				var resultList = new TodoList(results);
				resultList.View(showIndex: true, showStatus: true, showDate: true);
			}
			else
			{
				Console.WriteLine("Ничего не найдено по вашему запросу.");
			}
		}
		private void ParseArguments()
		{
			var args = SplitArgs(_rawArgs);
			for (int i = 0; i < args.Count; i++)
			{
				string arg = args[i].ToLower();
				if (i == 0 && !arg.StartsWith("--"))
				{
					_contains = args[i];
					continue;
				}
				if (arg.StartsWith("--"))
				{
					if (arg == "--desc")
					{
						_desc = true;
						continue;
					}
					if (i + 1 >= args.Count)
					{
						throw new InvalidArgumentException($"Отсутствует значение для флага '{arg}'.");
					}
					string val = args[i + 1];
					bool paramConsumed = true;
					switch (arg)
					{
						case "--contains":
							_contains = val;
							break;
						case "--starts-with":
							_startsWith = val;
							break;
						case "--ends-with":
							_endsWith = val;
							break;
						case "--status":
							if (Enum.TryParse<TodoStatus>(val, true, out var st))
							{
								_status = st;
							}
							else
							{
								throw new InvalidArgumentException($"Статус '{val}' не существует. Доступные: NotStarted, InProgress, Completed, Postponed, Failed.");
							}
							break;
						case "--from":
							if (DateTime.TryParse(val, out var df))
							{
								_dateFrom = df.Date;
							}
							else
							{
								throw new InvalidArgumentException($"Некорректный формат даты '{val}' для флага --from. Ожидается yyyy-MM-dd.");
							}
							break;
						case "--to":
							if (DateTime.TryParse(val, out var dt))
							{
								_dateTo = dt.Date;
							}
							else
							{
								throw new InvalidArgumentException($"Некорректный формат даты '{val}' для флага --to. Ожидается yyyy-MM-dd.");
							}
							break;
						case "--sort":
							if (val == "text" || val == "date")
							{
								_sortBy = val;
							}
							else
							{
								throw new InvalidArgumentException("Параметр sort должен быть 'text' или 'date'.");
							}
							break;
						case "--top":
							if (int.TryParse(val, out int t) && t > 0)
							{
								_top = t;
							}
							else
							{
								throw new InvalidArgumentException($"Параметр top должен быть целым числом больше 0. Вы ввели: '{val}'");
							}
							break;
						default:
							throw new InvalidArgumentException($"Неизвестный флаг: '{arg}'. Проверьте справку (help).");
					}
					if (paramConsumed)
					{
						i++;
					}
				}
			}
		}
		private List<string> SplitArgs(string input)
		{
			var result = new List<string>();
			if (string.IsNullOrWhiteSpace(input)) return result;
			bool inQuotes = false;
			StringBuilder sb = new StringBuilder();
			foreach (char c in input)
			{
				if (c == '"') inQuotes = !inQuotes;
				else if (c == ' ' && !inQuotes)
				{
					if (sb.Length > 0)
					{
						result.Add(sb.ToString());
						sb.Clear();
					}
				}
				else sb.Append(c);
			}
			if (sb.Length > 0) result.Add(sb.ToString());
			return result;
		}
	}
}