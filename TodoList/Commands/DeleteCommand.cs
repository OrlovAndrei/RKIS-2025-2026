namespace TodoList.Commands;

public class DeleteCommand : ICommand
{
	public int Index { get; set; }
	public TodoList Todos { get; set; }

	public void Execute()
	{
		Todos.Delete(Index);
	}
}