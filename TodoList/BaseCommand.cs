
using TodoList1;

public abstract class BaseCommand : ICommand
{

	public TodoList todoList { get; set; }
	public Profile Profile { get; set; }
	public abstract void Execute();
}

