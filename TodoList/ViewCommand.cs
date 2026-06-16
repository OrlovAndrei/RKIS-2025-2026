namespace TodoList;

public class ViewCommand : ICommand
{
	public bool HasIndex { get; set; }
	public bool HasStatus { get; set; }
	public bool HasDate { get; set; }
	public bool HasAll { get; set; }
	public TodoList TodoList { get; set; }

	public void Execute()
	{
		TodoList.View(HasIndex, HasStatus, HasDate, HasAll);
	}
}