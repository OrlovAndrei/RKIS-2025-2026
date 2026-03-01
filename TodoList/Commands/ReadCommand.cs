namespace TodoList.Commands;

public class ReadCommand : ICommand
{
	public int Index { get; set; }

	public TodoList Todos { get; set; }

	public void Execute()
	{
		Todos.Read(Index);
	}
}