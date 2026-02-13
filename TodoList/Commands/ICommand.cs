
namespace TodoList;
internal interface ICommand
{
	public void Execute();
	public void Unexecute();
}
