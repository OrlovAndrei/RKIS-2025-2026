
namespace TodoList;
internal class UpdateCommand : IndexedCommand
{
	private string _task;
	private string _prevTask;
	private TodoItem _targetItem;

	public UpdateCommand(int index, string task) : base(index)
	{
		_task = task;
		_prevTask = "";
		_targetItem = null;
		AppInfo.UndoPush(this);
	}

	protected override void SubExecute(TodoItem item)
	{
		_prevTask = item.Text;
		_targetItem = item;
		item.UpdateText(_task);
		Console.WriteLine($"Задача под номером {Index} изменена на \"{_task}\".");
	}

	protected override void SubUnExecute()
	{
		_targetItem.UpdateText(_prevTask);
		Console.WriteLine($"Задача под номером {Index} восстановлена на \"{_prevTask}\".");
	}

}
