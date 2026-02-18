

namespace TodoList
{
	internal class DeleteCommand : IndexedCommand
	{
		private TodoItem _deletedItem;

		public DeleteCommand(int index) : base(index)
		{
			_deletedItem = AppInfo.CurrentTodoList.GetItem(Index);
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
			AppInfo.CurrentTodoList.Add(new TodoItem(_deletedItem));
			Console.WriteLine("Удаление задачи отменено");
		}

	}
}