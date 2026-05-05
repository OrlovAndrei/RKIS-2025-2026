using System;
using TodoList.Exceptions;

namespace TodoList
{
	class RedoCommand : ICommand
	{
		public void Execute()
		{
			if (AppInfo.redoStack.Count == 0)
			{
				throw new InvalidCommandException("Стек повтора пуст. Нечего повторять.");
			}

			var command = AppInfo.redoStack.Pop();
			command.Execute();
			AppInfo.undoStack.Push(command);
		}
	}
}