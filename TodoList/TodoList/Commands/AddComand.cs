using System;
using TodoList.Exceptions;
namespace TodoList.Commands
{
	public class AddCommand : ICommand, IUndo
	{
		public string TaskText { get; set; }
		private TodoItem? _addedItem;
		public AddCommand(string taskText) => TaskText = taskText;

		public void Execute()
		{
			if (AppInfo.CurrentProfile == null)
			{
				throw new AuthenticationException("Для добавления задач необходимо авторизоваться.");
			}
			if (string.IsNullOrWhiteSpace(TaskText))
			{
				throw new InvalidArgumentException("Текст задачи не может быть пустым.");
			}
			_addedItem = new TodoItem(TaskText);
			AppInfo.CurrentUserTodos?.Add(_addedItem);
			Console.WriteLine("Задача добавлена!");
		}
		public void Unexecute()
		{
			if (_addedItem != null && AppInfo.CurrentUserTodos != null && AppInfo.CurrentUserTodos.Contains(_addedItem))
			{
				AppInfo.CurrentUserTodos.Remove(_addedItem);
			}
		}
	}
}