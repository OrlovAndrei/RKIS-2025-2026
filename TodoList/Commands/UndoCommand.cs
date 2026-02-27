

namespace TodoList.Commands;
internal class UndoCommand : ICommand
{
	public void Execute()
	{
		if (AppInfo.UndoStack.TryPeek(out var command))
		{
			if (command is IRedo)
			{
				AppInfo.RedoPush(command);
				AppInfo.UndoPop();
				IRedo redo = (IRedo)command;
				redo.Unexecute();
			}
		}
	}

}
