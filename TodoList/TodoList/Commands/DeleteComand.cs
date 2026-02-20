using System;
using TodoList.Exceptions; 
namespace TodoList.Commands
{
	public class DeleteCommand : ICommand, IUndo
	{
		private int _index;
		private TodoItem? _deletedItem;
		public DeleteCommand(int index) => _index = index;
		public void Execute()
		{
			if (AppInfo.CurrentUserTodos == null)
				throw new Exception("Список задач не инициализирован.");
			_deletedItem = AppInfo.CurrentUserTodos.GetItem(_index);
			if (_deletedItem == null)
			{
				throw new TaskNotFoundException($"Задача с номером {_index} не найдена.");
			}
			AppInfo.CurrentUserTodos.Delete(_index);
			Console.WriteLine($"Задача {_index} удалена.");
		}
		public void Unexecute()
		{
			if (_deletedItem != null)
				AppInfo.CurrentUserTodos?.Insert(_index, _deletedItem);
		}
	}
}