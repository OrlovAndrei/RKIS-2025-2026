using TodoApp.Exceptions;
public interface ICommand
{
	void Execute();
}
public interface IUndo : ICommand
{
	void Unexecute();
}