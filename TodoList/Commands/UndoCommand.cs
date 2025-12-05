using TodoApp.Commands;
using TodoApp;
namespace TodoApp.Commands
{
    public class UndoCommand : BaseCommand
	{
		public override void Execute()
		{
			if (AppInfo.UndoStack.Count > 0)
			{
				var command = AppInfo.UndoStack.Pop();
				command.Unexecute();
				AppInfo.RedoStack.Push(command);
			}
			else
			{
				Console.WriteLine("Нет действий для отмены.");
			}
		}
		
		public override void Unexecute() { }
		}
	}

