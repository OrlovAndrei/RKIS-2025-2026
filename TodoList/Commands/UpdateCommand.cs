namespace TodoList.Commands;

public class UpdateCommand : ICommand
{
	public int Index { get; set; }
	public string NewText { get; set; }

	public TodoList Todos { get; set; }

	public void Execute()
	{
		Todos.Update(Index, NewText);
	}
}