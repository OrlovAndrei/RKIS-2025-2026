namespace TodoList.Classes;

public class ReadCommand: ICommand
{
	public int TaskIndex { get; set; }
	public TodoList TodoList { get; set; }

	public void Execute()
	{
		TodoList.Read(TaskIndex);
	}
}