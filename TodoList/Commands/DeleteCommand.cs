using System;

namespace TodoApp.Commands
{
	public class DeleteCommand : BaseCommand, IUndo
	{
		private readonly TodoList _todoList;
		private readonly int _index;
		private TodoItem? _deletedItem;
		private int _deletedIndex;

		public DeleteCommand(TodoList todoList, int index, Guid? currentProfileId)
		{
			_todoList = todoList;
			_index = index;
			CurrentProfileId = currentProfileId;
		}

		public override void Execute()
		{
			var item = _todoList.GetItem(_index);
			if (item == null)
			{
				Console.WriteLine($"Ошибка: задача с номером {_index + 1} не найдена.");
				return;
			}

			_deletedItem = item;
			_deletedIndex = _index;
			_todoList.Delete(_index);
		}

		public void Unexecute()
		{
			if (_deletedItem != null)
			{
				_todoList.Add(_deletedItem);
				Console.WriteLine($"Отменено удаление задачи: {_deletedItem.Text}");
			}
		}
	}
}