using System;
using TodoList.Exceptions;

namespace TodoList.Commands
{
	public class DeleteCommand : ICommand, IUndo
	{
		public TodoList TodoList { get; set; }
		public int TaskIndex { get; set; }
		private TodoItem deletedItem;
		private int deletedIndex;

		public void Execute()
		{
			if (TaskIndex < 0 || TaskIndex >= AppInfo.Todos.Count)
			{
				throw new TaskNotFoundException($"Задача с номером {TaskIndex + 1} не существует.");
			}
			deletedIndex = TaskIndex;
			deletedItem = TodoList[TaskIndex];
			TodoList.Delete(TaskIndex);
			Console.WriteLine("Задача удалена.");
		}

		public void Unexecute()
		{
			if (deletedItem != null)
			{
				TodoList.Insert(deletedIndex, deletedItem);
				Console.WriteLine($"Отмена: восстановлена задача '{deletedItem.Text}'");
			}
		}
	}
}