using System;

namespace TodoList
{
	class DeleteCommand : ICommand
	{
		public TodoList TodoList { get; set; }
		public int TaskIndex { get; set; }
		public TodoItem deletedItem;
		public int deletedIndex;

		public void Execute()
		{
			try
			{
				deletedIndex = TaskIndex;
				deletedItem = TodoList[TaskIndex];

				TodoList.Delete(TaskIndex);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка: {ex.Message}");
			}
		}

		public void Unexecute()
		{
			if (deletedItem != null)
			{
				TodoList.Add(deletedItem);
				Console.WriteLine($"Восстановлена удаленная задача: {deletedItem.Text}");
			}
		}
	}
}
