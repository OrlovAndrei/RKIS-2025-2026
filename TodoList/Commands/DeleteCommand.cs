namespace TodoList.Commands;

public class DeleteCommand : ICommand
{
	public int TaskIndex { get; set; }
	public TodoList TodoList { get; set; }

	public void Execute()
	{
		TodoList.Delete(TaskIndex);
	}
}