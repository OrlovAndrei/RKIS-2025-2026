using System;
using TodoList.Commands;
using TodoList.Exceptions;
namespace TodoList.Commands
{
	internal class UndoCommand : ICommand
	{
		public void Execute()
		{
			if (AppInfo.CurrentProfile == null)
			{
				throw new AuthenticationException("Необходимо авторизоваться для отмены действий.");
			}
			if (AppInfo.UndoStack.Count == 0)
			{
				throw new InvalidCommandException("Нет действий для отмены (стек пуст).");
			}
			ICommand lastCommand = AppInfo.UndoStack.Pop();
			if (lastCommand is IUndo undoableCommand)
			{
				undoableCommand.Unexecute();
				AppInfo.RedoStack.Push(lastCommand);
				Console.WriteLine("Действие отменено.");
			}
			else
			{
				Console.WriteLine("Эту команду нельзя отменить.");
			}
		}
	}
}