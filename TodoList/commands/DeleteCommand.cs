namespace TodoList.commands;

public class DeleteCommand : ICommand
{
	public int TaskIndex { get; init; }
	public required classes.TodoList TodoList { get; init; }

	public void Execute()
	{
		TodoList.Delete(TaskIndex);
	}
}