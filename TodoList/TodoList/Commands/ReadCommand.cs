public class ReadCommand : ICommand
{
	public int TaskIndex { get; set; }
	public TodoList Todos { get; set; }
	public Guid UserId { get; set; }
	public void Execute()
	{
		if (TaskIndex < 0 || TaskIndex >= Todos.Count)
		{
			Console.WriteLine("Неверный индекс задачи");
			return;
		}
		Console.WriteLine(Todos[TaskIndex].GetFullInfo());
	}
}
