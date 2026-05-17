namespace TodoList.commands;

public class UpdateCommand : ICommand
{
	public int TaskIndex { get; init; }
	public required string NewText { get; init; }

	public required classes.TodoList TodoList { get; init; }

	public void Execute()
	{
		TodoList.Update(TaskIndex, NewText);
	}
}