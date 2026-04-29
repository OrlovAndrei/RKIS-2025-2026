using System;
using TodoList.Exceptions;

namespace TodoList.Commands
{
	public class UndoCommand : ICommand
	{
		public void Execute()
		{
			if (AppInfo.undoStack.Count == 0)
			{
				throw new InvalidCommandException("История отмены пуста.");
			}

			var command = AppInfo.undoStack.Pop();
			if (command is IUndo undoable)
			{
				undoable.Unexecute();
				AppInfo.redoStack.Push(command);
			}
		}
	}
}