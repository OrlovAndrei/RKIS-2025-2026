

namespace TodoList
{
	internal class DeleteCommand : IndexedCommand
	{
		private TodoItem _deletedItem;

		public DeleteCommand(int index) : base(index)
		{
			_deletedItem = AppInfo.CurrentTodoList.GetItem(Index);
			AppInfo.CurrentTodoList.Delete(Index);
		}

		protected override void SubExecute(TodoItem item)
		{
			_deletedItem = item;
			AppInfo.CurrentTodoList.Insert(Index, _deletedItem);
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