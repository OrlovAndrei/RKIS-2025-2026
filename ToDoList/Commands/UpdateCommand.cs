using System;

namespace TodoList.Commands
{
	public class UpdateCommand : ICommand, IUndo
	{
		public TodoList TodoList { get; set; }
		public int TaskIndex { get; set; }
		public string NewText { get; set; }
		private string oldText;

		public void Execute()
		{
			if (TaskIndex < 0 || TaskIndex >= TodoList.Count)
			{
				throw new Exception("Ошибка: Задача с таким номером не найдена.");
			}
			oldText = TodoList[TaskIndex].Text;
			TodoList[TaskIndex].Text = NewText;
			TodoList[TaskIndex].LastUpdate = DateTime.Now;
			Console.WriteLine("Задача обновлена.");
		}

		public void Unexecute()
		{
			TodoList[TaskIndex].Text = oldText;
			TodoList[TaskIndex].LastUpdate = DateTime.Now;
			Console.WriteLine("Отмена: текст задачи возвращен.");
		}
	}
}