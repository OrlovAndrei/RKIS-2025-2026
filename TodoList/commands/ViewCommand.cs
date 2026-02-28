namespace TodoList.commands;

public class ViewCommand : ICommand
{
	public bool ShowIndex { get; init; }
	public bool ShowStatus { get; init; }
	public bool ShowDate { get; init; }
	public required classes.TodoList TodoList { get; init; }

	public void Execute()
	{
		TodoList.View(ShowIndex, ShowStatus, ShowDate);
	}
}