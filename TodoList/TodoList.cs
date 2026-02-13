namespace TodoApp.Commands
{
	public class TodoList : IEnumerable<TodoItem>
	{
		private List<TodoItem> _items;
		public int Count => _items.Count;
		public TodoItem this[int index] => _items[index];
		public event Action<TodoList>? OnTodoAdded;
        public event Action<TodoList>? OnTodoDeleted;
        public event Action<TodoItem>? OnTodoUpdated;
        public event Action<TodoList>? OnStatusChanged;
		public event Action<TodoList>? OnTodoListChanged;
        public event Action<TodoList, string>? OnTodoListSaveRequested;
		
		public TodoList(List<TodoItem> items)
		{
			_items = items;
		}
		public void Add(TodoItem item)
		{
			_items.Add(item);
			OnTodoAdded?.Invoke(this);
			OnTodoListChanged?.Invoke(this);
			OnStatusChanged?.Invoke(this);
		}
		public void Delete(int index)
		{
			if (index < 0 || index >= _items.Count)
			{
				Console.WriteLine("Неверный номер задачи.");
				return;
			}
			var deletedItem = _items[index];
            _items.RemoveAt(index);
			OnTodoDeleted?.Invoke(this);
			OnTodoListChanged?.Invoke(this);
			OnStatusChanged?.Invoke(this);
			Console.WriteLine($"Задача {index + 1} удалена.");
		}
		public void SetStatus(int index, TodoStatus status)
		{
			if (index < 0 || index >= _items.Count)
			{
				Console.WriteLine("Неверный номер задачи.");
				return;
			}
			var item = _items[index];
			item.Status = status;
			OnTodoUpdated?.Invoke(item);
			OnTodoListChanged?.Invoke(this);
			OnStatusChanged?.Invoke(this);
			Console.WriteLine($"Статус задачи '{item.Text}' изменен на: {TodoItem.GetStatusDisplayName(status)}");
		}
		public void Update(TodoItem item)
        {
            var index = _items.IndexOf(item);
            if (index >= 0)
            {
                _items[index] = item;
                OnTodoUpdated?.Invoke(item); 
				OnTodoListChanged?.Invoke(this);
				OnStatusChanged?.Invoke(this);
            }
        }
		public TodoItem? GetItem(int index)
		{
			if (index < 0 || index >= _items.Count)
			{
				Console.WriteLine("Неверный номер задачи.");
				return null;
			}
			return _items[index];
		}
		public void View(bool showIndex = false, bool showDone = true, bool showDate = false, bool showAll = false)
		{
			if (_items.Count == 0)
			{
				Console.WriteLine("Задач нет!");
				return;
			}
			var filteredItems = showAll
				? _items
				: _items.Where(i => i.Status != TodoStatus.Completed).ToList();
			if (!filteredItems.Any())
			{
				Console.WriteLine("Задачи не найдены по заданным критериям.");
				return;
			}
			var table = BuildTable(filteredItems, showIndex, showDate, showDone);
			PrintTable(table);
		}
		public void ViewByStatus(
			TodoStatus statusFilter,
			bool showIndex,
			bool showDate,
			bool showDone,
			bool showAll
		)
		{
			var filteredItems = showAll
				? _items.Where(i => i.Status == statusFilter).ToList()
				: _items.Where(i => i.Status == statusFilter && i.Status != TodoStatus.Completed).ToList();
			if (!filteredItems.Any())
			{
				Console.WriteLine($"Задачи со статусом '{TodoItem.GetStatusDisplayName(statusFilter)}' не найдены.");
				return;
			}
			var table = BuildTable(filteredItems, showIndex, showDate, showDone);
			PrintTable(table);
		}
		private List<string[]> BuildTable(IEnumerable<TodoItem> items, bool showIndex, bool showDate, bool showDone)
		{
			var table = new List<string[]>();
			var headers = new List<string>();
			if (showIndex) headers.Add("№");
			headers.Add("Текст задачи");
			if (showDone) headers.Add("Статус");
			if (showDate) headers.Add("Дата изменения");
			table.Add(headers.ToArray());
			foreach (var item in items)
			{
				var row = new List<string>();
				if (showIndex) row.Add((item.GetIndex(_items) + 1).ToString());
				row.Add(item.Text);
				if (showDone) row.Add(TodoItem.GetStatusDisplayName(item.Status));
				if (showDate) row.Add(item.LastUpdate.ToString("dd.MM.yyyy HH:mm"));
				table.Add(row.ToArray());
			}
			return table;
		}
		public IEnumerator<TodoItem> GetEnumerator() => _items.GetEnumerator();
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
		private void PrintTable(List<string[]> table)
		{
			if (table.Count == 0) return;
			int[] columnWidths = new int[table[0].Length];
			for (int i = 0; i < table.Count; i++)
				for (int j = 0; j < table[i].Length; j++)
					if (table[i][j].Length > columnWidths[j])
						columnWidths[j] = table[i][j].Length;
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
				else if (i < table.Count - 1)
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
			string bottomBorder = "└";
			for (int j = 0; j < columnWidths.Length; j++)
			{
				bottomBorder += new string('─', columnWidths[j] + 2);
				if (j < columnWidths.Length - 1) bottomBorder += "┴";
			}
			bottomBorder += "┘";
			Console.WriteLine(bottomBorder);
		}
		public int GetIndex(TodoItem item)
		{
			return _items.IndexOf(item);
		}
		public void RequestSave()
        {
            if (AppInfo.CurrentProfileId.HasValue)
            {
                string filePath = Path.Combine("data", $"todos_{AppInfo.CurrentProfileId}.csv");
                OnTodoListSaveRequested?.Invoke(this, filePath);
            }
        }
	}
}