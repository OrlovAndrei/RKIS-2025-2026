using System;
using TodoList;
namespace TodoApp.Commands
{
	public class RedoCommand : BaseCommand
	{
		public override void Execute()
		{
			if (AppInfo.RedoStack.Count == 0)
			{
				Console.WriteLine("Нет операций для повтора");
				return;
			}

			var command = AppInfo.RedoStack.Pop();
			command.Execute();
			AppInfo.UndoStack.Push(command);
			Console.WriteLine("Операция повторена");
		}

		public override void Unexecute()
		{
			throw new NotImplementedException();
		}
	}
}
