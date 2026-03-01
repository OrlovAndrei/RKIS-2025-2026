namespace TodoList.Commands;

public class ViewCommand : ICommand
{
	public bool HasIndex { get; set; }
	public bool HasStatus { get; set; }
	public bool HasDate { get; set; }
	public bool HasAll { get; set; }
	public TodoList Todos { get; set; }

	public void Execute()
	{
		Todos.View(HasIndex, HasStatus, HasDate, HasAll);
	}
}