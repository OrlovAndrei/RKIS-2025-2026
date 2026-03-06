using System;

namespace Todolist
{
	public class StatusCommand : ICommand
	{
		public int TaskNumber { get; set; }
		public TodoStatus NewStatus { get; set; }
		public string TodoFilePath { get; set; }
		public string Description => $"Изменение статуса задачи #{TaskNumber}";
		private TodoStatus _oldStatus;
		private TodoItem _targetItem;

		public void Execute()
		{
			if (TaskNumber > 0 && TaskNumber <= AppInfo.Todos.Count)
			{
				int index = TaskNumber - 1;

				// Запоминаем
				_targetItem = AppInfo.Todos.GetItem(index);
				_oldStatus = _targetItem.Status;  // ← запомнили старый статус

				// Меняем
				_targetItem.Status = NewStatus;
				_targetItem.LastUpdate = DateTime.Now;

				string statusText = GetStatusText(NewStatus);
				Console.WriteLine($"Статус задачи #{TaskNumber} изменён на '{statusText}'");

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
			// ОТМЕНЯЕМ изменение статуса
			if (_targetItem != null)
			{
				_targetItem.Status = _oldStatus;
				_targetItem.LastUpdate = DateTime.Now;
				Console.WriteLine($"Отмена: статус возвращен к '{GetStatusText(_oldStatus)}'");

				// Сохраняем
				FileManager.SaveTodos(AppInfo.Todos, AppInfo.TodosFilePath);
			}
		}

		private string GetStatusText(TodoStatus status)
		{
			return status switch
			{
				TodoStatus.NotStarted => "Не начато",
				TodoStatus.InProgress => "В процессе",
				TodoStatus.Completed => "Выполнено",
				TodoStatus.Postponed => "Отложено",
				TodoStatus.Failed => "Провалено",
				_ => status.ToString()
			};
		}
	}
}	
