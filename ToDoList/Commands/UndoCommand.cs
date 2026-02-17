using System;

namespace TodoList.Commands
{
	public class UndoCommand : ICommand
	{
		public void Execute()
		{
			if (AppInfo.undoStack != null && AppInfo.undoStack.Count > 0)
			{
				ICommand lastCommand = AppInfo.undoStack.Pop();

				if (lastCommand is IUndo undoableCommand)
				{
					undoableCommand.Unexecute();
				}
			}
			else
			{
				Console.WriteLine("История команд пуста.");
			}
		}
	}
}