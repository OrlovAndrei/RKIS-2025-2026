namespace TodoList.commands;

public class ReadCommand : ICommand
{
	public int TaskIndex { get; init; }
	public required classes.TodoList TodoList { get; init; }

	public void Execute()
	{
		TodoList.Read(TaskIndex);
	}
}