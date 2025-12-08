namespace TodoList.Commands
{
	public class UndoCommand : ICommand
	{
		public void Execute()
		{
			if (AppInfo.undoStack.Count > 0)
			{
				ICommand lastCommand = AppInfo.undoStack.Pop();
				lastCommand.Unexecute();
				AppInfo.redoStack.Push(lastCommand);
				Console.WriteLine("Отмена выполнена");
			}
			else
			{
				Console.WriteLine("Нечего отменять");
			}
		}

		public void Unexecute() { }
	}
}