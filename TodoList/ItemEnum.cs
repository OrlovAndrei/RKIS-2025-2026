
namespace TodoList;
internal class ItemEnum
{
	private List<TodoItem> _items;
	private int _index;

	public ItemEnum(List<TodoItem> items)
	{
		_items = items;
		_index = -1;
	}

	public bool MoveNext()
	{
		_index++;
		return (_index < _items.Count);
	}

	public void Reset()
	{
		_index = -1;
	}

	public TodoItem Current => _items[_index];

}
