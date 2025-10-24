

namespace TodoList;
internal class TodoList
{
	private Todoitem[] _items = new Todoitem[0];

	// Количество задач в списке
	public int Lenght { get => _items.Length; }

	//добавление задачи
	public void Add(Todoitem item)
	{
		_items = IncreaseArray(_items, item);
	}

	//удаление
	public void Delete(int index)
	{
		if (index >= 0 && index < _items.Length)
		{
			Todoitem[] newArray = new Todoitem[_items.Length - 1];
			Array.Copy(_items, 0, newArray, 0, index);
			Array.Copy(_items, index + 1, newArray, index, _items.Length - index - 1);
			_items = newArray;
		}
		else
		{
			Console.WriteLine("Неверный индекс");
		}
	}

	public void View(bool showIndex, bool showDone, bool showDate)
	{
		Console.WriteLine(GenerateTableHeader(showIndex, showDone, showDate));
		for (int i = 0; i < _items.Length; i++)
		{
			Console.WriteLine(GenerateTableRow(_items[i], i, showIndex, showDone, showDate));
		}
	}

	public Todoitem GetItem(int index)
	{
		if (index >= 0 && index < _items.Length)
		{
			return _items[index];
		}
		else
		{
			return null;
		}
	}

	// увеличение размера массива при переполнении
	private Todoitem[] IncreaseArray(Todoitem[] items, Todoitem item)
	{
		Todoitem[] newArray = new Todoitem[items.Length + 1];
		Array.Copy(items, newArray, items.Length);
		newArray[items.Length] = item;
		return newArray;
	}

	private string GenerateTableHeader(bool showIndex, bool showDone, bool showDate)
	{
		string header = "";
		if (showIndex) header += "Indext";
		header += "Textt";
		if (showDone) header += "Donet";
		if (showDate) header += "Datet";
		return header;
	}

	private string GenerateTableRow(Todoitem item, int index, bool showIndex, bool showDone, bool showDate)
	{
		string row = "";
		if (showIndex) row += $"{index}t";
		row += $"{item.Text}t";
		if (showDone) row += $"{item.IsDone}t";
		if (showDate) row += $"{item.LastUpdate.ToShortDateString()}t";
		return row;
	}
}
