public class UpdateCommand : ICommand
{
	public int TaskIndex { get; set; }
	public string NewText { get; set; }
	public TodoList Todos { get; set; }
	public string DataDir { get; set; }
	public Guid UserId { get; set; }
	private string oldText;
	public void Execute()
	{
		if (TaskIndex < 0 || TaskIndex >= Todos.Count)
		{
			Console.WriteLine("Неверный индекс задачи");
			return;
		}
		oldText = Todos.GetItem(TaskIndex).GetText();
		Todos.Update(TaskIndex, NewText);

		Console.WriteLine($"Задача {TaskIndex} обновлена!");
	}
	public void Unexecute()
	{
		if (!string.IsNullOrEmpty(oldText))
		{
			Todos.Update(TaskIndex, oldText);
			Console.WriteLine("Обновление задачи отменено");
		}
	}
}