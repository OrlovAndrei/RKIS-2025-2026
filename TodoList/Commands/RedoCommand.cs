

namespace TodoList.Commands;
internal class RedoCommand : ICommand
{
	public void Execute()
	{
		if (AppInfo.RedoStack.Count == 0)
		{
			Console.WriteLine("Нет действий для повтора.");
			return;
		}

		ICommand commandToRedo = AppInfo.RedoStack.Pop();
		commandToRedo.Execute();
		AppInfo.UndoStack.Push(commandToRedo);
	}

	public void Unexecute() { }
}
