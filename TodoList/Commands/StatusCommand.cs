
namespace TodoList.Commands;
internal class StatusCommand : ICommand
{
    private readonly int _index;
    private TodoStatus _status, _prevStatus;

    public StatusCommand(int index, TodoStatus status)
    {
        _index = index;
        _status = status;
		_prevStatus = TodoStatus.NotStarted;
		AppInfo.UndoPush(this);
	}

    public void Execute()
    {
        if (!AppInfo.CurrentTodoList.IsValidIndex(_index))
        {
            Console.WriteLine($"Ошибка: Задачи с индексом {_index} не существует.");
            return;
        }
		_prevStatus = AppInfo.CurrentTodoList.GetItem(_index).Status;
		AppInfo.CurrentTodoList.SetStatus(_index, _status);
        Console.WriteLine($"Статус задачи {_index} изменен на {_status}.");
    }

	public void Unexecute()
	{
		if (!AppInfo.CurrentTodoList.IsValidIndex(_index))
		{
			Console.WriteLine($"Ошибка: Задачи с индексом {_index} не существует.");
			return;
		}
		AppInfo.CurrentTodoList.SetStatus(_index, _prevStatus);
		Console.WriteLine("Изменение статуса отменено.");
	}

	public bool PossibleToUndo() => true;

}
