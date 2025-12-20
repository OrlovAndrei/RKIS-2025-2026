namespace TodoList.Commands;

public class ViewCommand : ICommand
{
	public bool ShowIndex { get; set; }
	public bool ShowStatus { get; set; }
	public bool ShowDate { get; set; }
	public bool HasAll { get; set; }
	public TodoList TodoList { get; set; }

	public void Execute()
	{
		TodoList.View(ShowIndex, ShowStatus, ShowDate, HasAll);
	}
}