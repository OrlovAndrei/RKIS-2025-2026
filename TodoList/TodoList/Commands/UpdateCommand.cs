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
		Todos.GetItem(TaskIndex).UpdateText(NewText);
		Console.WriteLine($"Задача {TaskIndex} обновлена!");
		FileManager.SaveUserTodos(UserId, Todos, DataDir);
	}
	public void Unexecute()
	{
		if (!string.IsNullOrEmpty(oldText))
		{
			Todos.GetItem(TaskIndex).UpdateText(oldText);
			Console.WriteLine("Обновление задачи отменено");
			FileManager.SaveUserTodos(UserId, Todos, DataDir);
		}
	}
}