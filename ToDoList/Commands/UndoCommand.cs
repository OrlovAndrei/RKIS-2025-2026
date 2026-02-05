using System;

namespace TodoList
{
	class UndoCommand : ICommand
	{
		public void Execute()
		{
			if (AppInfo.undoStack.Count > 0)
			{
				ICommand command = AppInfo.undoStack.Pop();
				command.Unexecute();
				AppInfo.redoStack.Push(command);
				Console.WriteLine("Отменено последнее действие");
			}
			else
			{
				Console.WriteLine("Нет действий для отмены");
			}
		}

		public void Unexecute()
		{
			// пустой метод
		}
	}
}
