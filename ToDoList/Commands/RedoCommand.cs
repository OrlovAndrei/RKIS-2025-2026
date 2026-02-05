using System;

namespace TodoList
{
	class RedoCommand : ICommand
	{
		public void Execute()
		{
			if (AppInfo.redoStack.Count > 0)
			{
				ICommand command = AppInfo.redoStack.Pop();
				command.Execute();
				AppInfo.undoStack.Push(command);
				Console.WriteLine("Повторено последнее отмененное действие");
			}
			else
			{
				Console.WriteLine("Нет действий для повтора");
			}
		}

		public void Unexecute()
		{
			// пустой метод
		}
	}
}
