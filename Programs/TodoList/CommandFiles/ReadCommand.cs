using System;

namespace Todolist
{
	public class ReadCommand : ICommand
	{
		public TodoList TodoList { get; set; }
		public int TaskNumber { get; set; }

		public void Execute()
		{
			if (TaskNumber > 0 && TaskNumber <= TodoList.Count)
			{
				int index = TaskNumber - 1;
				TodoItem item = TodoList.GetItem(index);
				Console.WriteLine("=======================================");
				Console.WriteLine(item.GetFullInfo());
				Console.WriteLine("=======================================");
			}
			else
			{
				Console.WriteLine("Неверный номер задачи");
			}
		}
	}
}