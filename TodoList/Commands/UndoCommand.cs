using System;
using TodoApp;

namespace TodoApp.Commands
{
	public class UndoCommand : BaseCommand, ICommand
	{
		public override void Execute()
		{
			if (AppInfo.UndoStack.Count > 0)
			{
				var command = AppInfo.UndoStack.Pop();
				command.Unexecute();
				AppInfo.RedoStack.Push(command);
				Console.WriteLine("Операция отменена");
			}
			else
			{
				Console.WriteLine("Нет действий для отмены.");
			}
		}
	}
}