
namespace TodoList;
internal class TodoList
{
	private List<TodoItem> _items;
	public List<TodoItem> Items { get => _items; }

	// Количество задач в списке
	public int Length { get => _items.Count; }
	public int LenghtAllocated { get => _items.Count; }

	// Event Actions
	public event Action<TodoItem>? OnTodoAdded;
	public event Action<TodoItem>? OnTodoDeleted;
	public event Action<TodoItem>? OnStatusChanged;

	public TodoList()
	{
		_items = new List<TodoItem>();
	}

	public TodoList(List<TodoItem> list)
	{
		_items = list;
	}

	//добавление задачи
	public void Add(TodoItem item)
	{
		_items.Add(item);
		OnTodoAdded?.Invoke(item);
	}

	//удаление
	public void Delete(int index)
	{
		var item = _items[index];
		_items.RemoveAt(index);
		OnTodoDeleted?.Invoke(item);
	}

	public void View(bool showIndex, bool showDone, bool showDate)
	{
		Console.WriteLine(GenerateTableHeader(showIndex, showDone, showDate));
		for (int i = 0; i < Length; i++)
		{
			Console.WriteLine(GenerateTableRow(_items[i], i, showIndex, showDone, showDate));
		}
	}

	public IEnumerator<TodoItem> GetEnumerator()
	{
		foreach (TodoItem item in _items)
			yield return item;
	}

	public TodoItem GetItem(int index)
	{
		return IsValidIndex(index) ? _items[index] : null;
	}

	private string GenerateTableHeader(bool showIndex, bool showDone, bool showDate)
	{
		string header = "";
		if (showIndex) header += "Index";
		if (showDone) header += "\tStatus ";
		if (showDate) header += "\t\tDate ";
		header += "\t\tText";
		return header;
	}

	private string GenerateTableRow(TodoItem item, int index, bool showIndex, bool showDone, bool showDate)
	{
		string row = "";
		if (showIndex) row += $"{index}\t";
		if (showDone) row += $"{item.Status}\t";
		if (showDate) row += $"{item.LastUpdate.ToShortDateString()} ";
		row += $"\t\"{item.ToPrintText()}\" ";
		row += "\n";
		return row;
	}

	public bool IsValidIndex(int index) => index >= 0 && index < Length;

	public void SetStatus(int index, TodoStatus status)
	{
		if (IsValidIndex(index))
		{
			_items[index].SetStatus(status);
			OnStatusChanged?.Invoke(_items[index]);
		}
		else
		{
			Console.WriteLine($"Ошибка: Некорректный индекс для установки статуса: {index}");
		}
	}

	public void Insert(int index, TodoItem item)
	{
		if (index < 0 || index > _items.Count) //вставка в конец списка 
		{
			throw new ArgumentOutOfRangeException(nameof(index), "Индекс вышел за пределы диапазона для вставки.");
		}
		_items.Insert(index, item);
	}
}
