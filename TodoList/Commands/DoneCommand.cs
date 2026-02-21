namespace TodoList.Commands;

public class DoneCommand : ICommand
{
	public int TaskNumber { get; set; }
	public TodoList TodoList { get; set; }
	public void Execute()
	{
		int taskIndex = TaskNumber - 1;
		try
		{
			TodoItem item = TodoList.GetItem(taskIndex);
			item.MarkDone();
			Console.WriteLine($"Задача '{item.Text}' отмечена как выполненная");
		}
		catch (ArgumentOutOfRangeException)
		{
			Console.WriteLine($"Задачи с номером {TaskNumber} не существует.");
		}
	}
}