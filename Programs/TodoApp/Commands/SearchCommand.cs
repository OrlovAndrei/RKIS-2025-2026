using System;
using System.Collections.Generic;
using System.Linq;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Commands
{
	public class SearchCommand : ICommand
	{
		private string _contains; 
		private string _startsWith;
		private string _endsWith;
		private DateTime? _from; // не раньше
		private DateTime? _to; // не позже
		private TodoStatus? _status;
		private string _sortBy;
		private bool _descending;
		private int? _top;

		public SearchCommand(
			string contains = null,
			string startsWith = null,
			string endsWith = null,
			DateTime? from = null,
			DateTime? to = null,
			TodoStatus? status = null,
			string sortBy = null,
			bool descending = false, //false - возрастание true - убывание
			int? top = null)
		{
			_contains = contains;
			_startsWith = startsWith;
			_endsWith = endsWith;
			_from = from;
			_to = to;
			_status = status;
			_sortBy = sortBy;
			_descending = descending;
			_top = top;
		}

		public void Execute()
		{
			var todos = AppInfo.GetCurrentTodoList();
			if (todos == null || todos.Count == 0)
			{
				Console.WriteLine("Список задач пуст"); // достаём список задач и проверяем, не пуст ли он
				return;
			}

			
			var items = todos.GetAll().Select((item, index) => new { Item = item, Index = index });

			var filtered = items.Where(x => ApplyFilters(x.Item)); //фильтрация через linqwhere

			IOrderedEnumerable<dynamic> sorted;
			if (_sortBy == "text")
			{
				if (_descending)
					sorted = filtered.OrderByDescending(x => x.Item.Text);
				else
					sorted = filtered.OrderBy(x => x.Item.Text);
			}
			else if (_sortBy == "date")
			{
				if (_descending)
					sorted = filtered.OrderByDescending(x => x.Item.LastUpdate);
				else
					sorted = filtered.OrderBy(x => x.Item.LastUpdate);
			}
			else
			{
				if (_descending)
					sorted = filtered.OrderByDescending(x => x.Index);
				else
					sorted = filtered.OrderBy(x => x.Index);
			}

			var result = _top.HasValue ? sorted.Take(_top.Value) : sorted;

			
			var resultList = result.ToList();
			if (!resultList.Any())
			{
				Console.WriteLine("Ничего не найдено");
				return;
			}
			// вывод результата 
			
			PrintTable(resultList); //и в табличку его agada
		}

		private bool ApplyFilters(TodoItem item) //фильтры начинается текстом, заканчивается, дата не раньше не позже и т.п.
		{
			if (_contains != null && !item.Text.Contains(_contains, StringComparison.OrdinalIgnoreCase))
				return false;

			if (_startsWith != null && !item.Text.StartsWith(_startsWith, StringComparison.OrdinalIgnoreCase))
				return false;

			if (_endsWith != null && !item.Text.EndsWith(_endsWith, StringComparison.OrdinalIgnoreCase))
				return false;

			if (_from.HasValue && item.LastUpdate < _from.Value)
				return false;

			if (_to.HasValue && item.LastUpdate > _to.Value)
				return false;

			if (_status.HasValue && item.Status != _status.Value)
				return false;

			return true;
		}

		private void PrintTable(IEnumerable<dynamic> items)
		{
			// Ширина колонок
			int indexWidth = 5;
			int statusWidth = 12;
			int dateWidth = 20;
			int textWidth = 30;

			string separator = new string('─', indexWidth + textWidth + statusWidth + dateWidth + 7);

			// Заголовок
			Console.WriteLine(separator);
			Console.WriteLine($"| {"Idx",-4} | {"Text".PadRight(textWidth)} | {"Status",-10} | {"LastUpdate",-19} |");
			Console.WriteLine(separator);

			// Вывод 
			foreach (var item in items)
			{
				string shortText = item.Item.Text.Length > textWidth
					? item.Item.Text.Substring(0, textWidth - 3) + "..."
					: item.Item.Text.PadRight(textWidth);

				string status = GetStatusShortName(item.Item.Status);
				string date = item.Item.LastUpdate.ToString("yyyy-MM-dd HH:mm:ss");

				Console.WriteLine($"| {item.Index,4} | {shortText} | {status,-10} | {date,-19} |");
			}

			Console.WriteLine(separator);
			Console.WriteLine($"Найдено задач: {items.Count()}");
		}

		private string GetStatusShortName(TodoStatus status)
		{
			return status switch
			{
				TodoStatus.NotStarted => "NotStart",
				TodoStatus.InProgress => "Progress",
				TodoStatus.Completed => "Complete",
				TodoStatus.Postponed => "Postpone",
				TodoStatus.Failed => "Failed",
				_ => status.ToString()
			};
		}
	}
}