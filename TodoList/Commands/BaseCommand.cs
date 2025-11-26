namespace TodoApp.Commands;
public abstract class BaseCommand
{
	public virtual void Execute() { }
	public abstract void Unexecute();
}