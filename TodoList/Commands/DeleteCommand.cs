

namespace TodoList
{
	internal class DeleteCommand : IndexedCommand
	{
		private TodoItem _deletedItem;

		public DeleteCommand(int index) : base(index)
		{
			_deletedItem = null;
			AppInfo.UndoPush(this);
		}

		protected override void SubExecute(TodoItem item)
		{
			_deletedItem = item;
			AppInfo.CurrentTodoList.Delete(Index);
			Console.WriteLine("Задача успешно удалена");
		}

		// Not need
		protected override void SubUnExecute()
		{
			AppInfo.CurrentTodoList.Add(_deletedItem);
			Console.WriteLine("Удаление задачи отменено");
		}

	}
}