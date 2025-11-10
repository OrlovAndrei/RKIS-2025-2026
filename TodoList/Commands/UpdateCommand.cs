namespace TodoList.Commands;

public class UpdateCommand : ICommand
{
	public int TaskIndex { get; set; }
	public string NewText { get; set; }

	public TodoList TodoList { get; set; }

	public void Execute()
	{
		TodoList.Update(TaskIndex, NewText);
	}
}