namespace TodoApp.Commands
{
	public interface IUndo : ICommand
	{
		void Unexecute();
	}
}