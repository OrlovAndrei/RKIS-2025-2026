using System;
using System.Collections.Generic;
using TodoApp;
using TodoApp.Commands;

namespace TodoList.Commands
{
    public class UndoCommand : BaseCommand
	{
	public override void Execute()
	{
		if (AppInfo.UndoStack.Count == 0)
		{
			Console.WriteLine("Нет операций для отмены");
			return;
		}

		var command = AppInfo.UndoStack.Pop();
		command.Unexecute();
		AppInfo.RedoStack.Push(command);
		Console.WriteLine("Операция отменена");
	}

	public override void Unexecute()
		{
		throw new NotImplementedException();
		}
	}
}
