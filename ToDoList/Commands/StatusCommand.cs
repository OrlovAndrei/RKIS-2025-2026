using System;

namespace TodoList.Commands
{
	public class StatusCommand : ICommand, IUndo
	{
		public TodoList TodoList { get; set; }
		public int TaskIndex { get; set; }
		public TodoStatus NewStatus { get; set; }
		private TodoStatus oldStatus;

		public void Execute()
		{
			if (TaskIndex < 0 || TaskIndex >= TodoList.Count)
			{
				throw new Exception("Ошибка: Задача с таким номером не найдена.");
			}
			oldStatus = TodoList[TaskIndex].Status;
			TodoList[TaskIndex].Status = NewStatus;
			Console.WriteLine($"Статус изменен на: {NewStatus}");
		}

		public void Unexecute()
		{
			TodoList[TaskIndex].Status = oldStatus;
			Console.WriteLine($"Отмена: статус возвращен на {oldStatus}");
		}
	}
}