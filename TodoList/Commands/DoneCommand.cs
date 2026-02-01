namespace TodoList.Commands;
public class SetStatusCommand : ICommand
{
	public int Index { get; init; }
	public string Value { get; init; }
	public TodoList Todos { get; init; }

	public void Execute()
	{
		Todos.SetStatus(Index, Enum.Parse<TodoStatus>(Value, true));
	}
}