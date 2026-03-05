using System;

namespace Todolist
{
	public class DeleteCommand : ICommand
	{
		public int TaskNumber { get; set; }
		public string TodoFilePath { get; set; }
		public string Description => $"Удаление задачи #{TaskNumber}";
		private TodoItem _deletedTask;
		private int _deletedIndex;

		public void Execute()
		{
			if (TaskNumber > 0 && TaskNumber <= AppInfo.Todos.Count)
			{
				int index = TaskNumber - 1;

				_deletedTask = AppInfo.Todos.GetItem(index);
				_deletedIndex = index;

				string deletedTask = _deletedTask.Text;

				//Удаляем
				AppInfo.Todos.Delete(index);
				Console.WriteLine($"Задача '{deletedTask}' удалена");

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
			if (_deletedTask != null)
			{
				AppInfo.Todos.Insert(_deletedIndex, _deletedTask);
				Console.WriteLine($"Отмена: задача '{_deletedTask.Text}' возвращена");

				// Сохраняем
				FileManager.SaveTodos(AppInfo.Todos, AppInfo.TodosFilePath);
			}
		}
	}
}