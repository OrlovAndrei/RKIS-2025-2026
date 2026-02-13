

namespace TodoList.Commands;
internal class UndoCommand : ICommand
{
	public void Execute()
	{
		if (AppInfo.UndoStack.TryPeek(out var command))
		{
			AppInfo.RedoPush(command);
			AppInfo.UndoPop();
			command.Unexecute();
		}
	}

	public void Unexecute() { }
}
