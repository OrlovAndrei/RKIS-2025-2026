using System;
using TodoList.Commands;
using TodoList.Exceptions;
namespace TodoList.Commands
{
	internal class RedoCommand : ICommand
	{
		public void Execute()
		{
			if (AppInfo.CurrentProfile == null)
			{
				throw new AuthenticationException("Необходимо авторизоваться для повтора действий.");
			}
			if (AppInfo.RedoStack.Count == 0)
			{
				throw new InvalidCommandException("Нет действий для повтора (стек пуст).");
			}
			ICommand commandToRedo = AppInfo.RedoStack.Pop();
			commandToRedo.Execute();
			AppInfo.UndoStack.Push(commandToRedo);
			Console.WriteLine("Действие повторено.");
		}
	}
}