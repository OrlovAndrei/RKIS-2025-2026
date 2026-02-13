namespace TodoApp.Commands
{
	public class StatusCommand : BaseCommand, IUndo
	{
		private readonly TodoList _todoList;
		private readonly int _index;
		private readonly TodoStatus _newStatus;
		private TodoStatus _oldStatus;

		public StatusCommand(TodoList todoList, int index, TodoStatus status, Guid? currentProfileId)
		{
			_todoList = todoList;
			_index = index;
			_newStatus = status;
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

			_oldStatus = item.Status;
			_todoList.SetStatus(_index, _newStatus);
		}

		public void Unexecute()
		{
			var item = _todoList.GetItem(_index);
			if (item != null)
			{
				_todoList.SetStatus(_index, _oldStatus);
				Console.WriteLine($"Отменено изменение статуса задачи: {item.Text}");
			}
		}
	}
}