using System;

namespace TodoList.Commands
{
	public class StatusCommand : ICommand
	{
		public TodoList TodoList { get; set; }
		public int TaskIndex { get; set; }
		public TodoStatus NewStatus { get; set; }

		public TodoStatus oldStatus;
		public int statusIndex;

		public void Execute()
		{
			if (TaskIndex < 0 || TaskIndex >= TodoList.Count)
			{
				Console.WriteLine("Ошибка: неверный индекс задачи");
				return;
			}

			statusIndex = TaskIndex;
			oldStatus = TodoList[TaskIndex].Status;

			var task = TodoList[TaskIndex];
			task.SetStatus(NewStatus);
			Console.WriteLine($"Статус задачи {TaskIndex + 1} изменен на: {task.GetStatusText()}");
		}

		public void Unexecute()
		{
			if (TodoList.Count > statusIndex)
			{
				TodoList[statusIndex].SetStatus(oldStatus);
				Console.WriteLine($"Отменено изменение статуса задачи");
			}
		}
	}
}
