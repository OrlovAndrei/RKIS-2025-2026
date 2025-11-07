using System;

namespace Todolist
{
	public class DeleteCommand : ICommand
	{
		public TodoList TodoList { get; set; }
		public int TaskNumber { get; set; }

		public void Execute()
		{
			if (TaskNumber > 0 && TaskNumber <= TodoList.Count)
			{
				int index = TaskNumber - 1;
				string deletedTask = TodoList.GetItem(index).Text;
				TodoList.Delete(index);
				Console.WriteLine($"Задача '{deletedTask}' удалена");
			}
			else
			{
				Console.WriteLine("Неверный номер задачи");
			}
		}
	}
}