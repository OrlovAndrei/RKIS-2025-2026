using System;
using TodoList.Exceptions;
namespace TodoList.Commands
{
	public class StatusCommand : ICommand, IUndo
	{
		private int _index;
		private TodoStatus _newStatus;
		private TodoStatus _oldStatus;
		public StatusCommand(int index, TodoStatus newStatus)
		{
			_index = index;
			_newStatus = newStatus;
		}
		public void Execute()
		{
			if (AppInfo.CurrentProfile == null)
			{
				throw new AuthenticationException("Для изменения статуса задач необходимо авторизоваться.");
			}
			var item = AppInfo.CurrentUserTodos.GetItem(_index);
			if (item == null)
			{
				throw new TaskNotFoundException($"Задача с индексом {_index} не найдена.");
			}
			_oldStatus = item.Status;
			AppInfo.CurrentUserTodos.SetStatus(_index, _newStatus);
			Console.WriteLine($"Статус задачи {_index} изменен на '{_newStatus}'.");
		}
		public void Unexecute()
		{
			var item = AppInfo.CurrentUserTodos.GetItem(_index);
			if (item != null) AppInfo.CurrentUserTodos.SetStatus(_index, _oldStatus);
		}
	}
}