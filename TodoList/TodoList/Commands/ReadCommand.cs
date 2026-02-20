using TodoApp.Exceptions;
public class ReadCommand : ICommand
{
	public int TaskIndex { get; set; }
	public TodoList Todos { get; set; }
	public Guid UserId { get; set; }
	public void Execute()
	{
		if (TaskIndex < 0 || TaskIndex >= Todos.Count)
		{
			throw new TaskNotFoundException("Задача с таким индексом не существует.");
		}
		Console.WriteLine(Todos[TaskIndex].GetFullInfo());
	}
}
