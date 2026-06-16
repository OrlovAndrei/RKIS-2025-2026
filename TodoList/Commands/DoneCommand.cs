namespace TodoList.Commands;

public class SetStausCommand : ICommand
{
	public int TaskIndex { get; init; }
	public string EnumValue { get; init; }
	public TodoList TodoList { get; init; }

	public void Execute()
	{
		TodoList.SetStatus(TaskIndex, Enum.Parse<TodoStatus>(EnumValue, true));
	}
}