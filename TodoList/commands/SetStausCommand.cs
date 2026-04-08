using TodoList.classes;

namespace TodoList.commands;

public class SetStausCommand : ICommand
{
	public int TaskIndex { get; init; }
	public required string EnumValue { get; init; }
	public required classes.TodoList TodoList { get; init; }

	public void Execute()
	{
		TodoList.SetStatus(TaskIndex, Enum.Parse<TodoStatus>(EnumValue, true));
	}
}