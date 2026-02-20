public class UndoCommand : ICommand
{
	public void Execute()
	{
		if (AppInfo.UndoStack.Count > 0)
		{
			ICommand command = AppInfo.UndoStack.Pop();
			if (command is IUndo undoableCommand)
			{
				undoableCommand.Unexecute();
			}
			AppInfo.RedoStack.Push(command);
			Console.WriteLine("Команда отменена");
		}
		else
		{
			Console.WriteLine("Нет команд для отмены");
		}
	}
}