namespace TodoList.Commands;

public class DoneCommand : ICommand
{
	public int TaskIndex { get; set; }
	public TodoList TodoList { get; set; }

	public void Execute()
	{
		TodoList.MarkDone(TaskIndex);
	}
}