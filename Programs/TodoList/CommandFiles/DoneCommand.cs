using System;

namespace Todolist
{
	public class DoneCommand : ICommand
	{
		public int TaskNumber { get; set; }
		public string Description => $"Отметка задачи #{TaskNumber} как выполненной";

		private TodoStatus _oldStatus;
		private TodoItem _targetItem;

		public void Execute()
		{
			if (TaskNumber > 0 && TaskNumber <= AppInfo.Todos.Count)
			{
				int index = TaskNumber - 1;

				// Запоминаем
				_targetItem = AppInfo.Todos.GetItem(index);
				_oldStatus = _targetItem.Status;

				// Меняем на Completed
				_targetItem.Status = TodoStatus.Completed;
				_targetItem.LastUpdate = DateTime.Now;

				Console.WriteLine($"Задача '{_targetItem.Text}' выполнена!");

				// Сохраняем
				FileManager.SaveTodos(AppInfo.Todos, AppInfo.TodosFilePath);
			}
			else
			{
				Console.WriteLine("Неверный номер задачи");
			}
		}

		public void Unexecute()
		{
			// Отменяем
			if (_targetItem != null)
			{
				_targetItem.Status = _oldStatus;
				_targetItem.LastUpdate = DateTime.Now;
				Console.WriteLine($"❌ Отмена: задача больше не выполнена");

				// Сохраняем
				FileManager.SaveTodos(AppInfo.Todos, AppInfo.TodosFilePath);
			}
		}
	}
}