using TodoApp;
namespace TodoApp.Commands;
public class DeleteCommand : BaseCommand
{
	public TodoList TodoList { get; set; }
	public int Index { get; set; }
	private TodoItem _deletedItem;
	private int _originalIndex;

	public DeleteCommand()
	{
		TodoList = AppInfo.Todos;
	}

	public DeleteCommand(int index) : this()
	{
		Index = index;
	}

	public override void Execute()
	{
		var item = TodoList.GetItem(Index);
		if (item != null)
		{
			_deletedItem = item;
			_originalIndex = Index;
			TodoList.Delete(Index);
			Console.WriteLine($"Задача удалена: {item.Text}");
		}
	}

	public override void Unexecute()
	{
		if (_deletedItem != null)
		{
			TodoList.Add(_deletedItem);
			Console.WriteLine($"Восстановлена задача: {_deletedItem.Text}");
		}
	}
}
