using System;
using TodoList.Exceptions;
namespace TodoList.Commands
{
	public class UpdateCommand : ICommand, IUndo
	{
		public int Index { get; set; }
		public string NewText { get; set; }
		private string? _oldText;
		public UpdateCommand(int index, string newText)
		{
			Index = index;
			NewText = newText;
		}
		public void Execute()
		{
			if (string.IsNullOrWhiteSpace(NewText))
				throw new InvalidArgumentException("Текст задачи не может быть пустым.");
			if (AppInfo.CurrentUserTodos == null) return;
			var item = AppInfo.CurrentUserTodos.GetItem(Index);
			if (item == null)
			{
				throw new TaskNotFoundException($"Задача с номером {Index} не найдена для обновления.");
			}
			_oldText = item.Text;
			AppInfo.CurrentUserTodos.Update(Index, NewText);
			Console.WriteLine($"Задача {Index} обновлена.");
		}
		public void Unexecute()
		{
			var item = AppInfo.CurrentUserTodos?.GetItem(Index);
			if (item != null && _oldText != null)
				AppInfo.CurrentUserTodos?.Update(Index, _oldText);
		}
	}
}