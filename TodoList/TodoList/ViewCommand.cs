public class ViewCommand : ICommand
{
	public bool ShowIndex {get; set;}
	public bool ShowStatus {get; set;}
	public bool ShowDate {get; set;}
	public bool AllOutput {get; set;}
	public TodoList Todos {get; set;}

	public void Execute()
	{
		if (AllOutput)
		{
			ShowIndex = true;
			ShowStatus = true;
			ShowDate = true;
		}
		Todos.View(ShowIndex, ShowStatus, ShowDate);
	}
}
