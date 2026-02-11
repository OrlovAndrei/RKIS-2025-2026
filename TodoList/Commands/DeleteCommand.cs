namespace TodoList.Commands;

public class DeleteCommand : ICommand
{
	public int TaskNumber { get; set; }
	public TodoList TodoList { get; set; }

	public void Execute()
	{
		int taskIndex = TaskNumber - 1;
		try
		{
			TodoList.Delete(taskIndex);
			Console.WriteLine("Задача удалена");
		}
		catch (ArgumentOutOfRangeException)
		{
			Console.WriteLine($"Задачи с номером {TaskNumber} не существует.");
		}
	}
}