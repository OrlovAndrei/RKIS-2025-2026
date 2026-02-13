namespace TodoApp.Commands
{
	public class UpdateCommand : BaseCommand, IUndo
	{
		private readonly TodoList _todoList;
		private readonly int _index;
		private readonly string _newText;
		private string? _oldText;

		public UpdateCommand(int index, string newText)
		{
			_todoList = AppInfo.Todos;
			_index = index;
			_newText = newText;
		}

		public override void Execute()
		{
			var item = _todoList.GetItem(_index);
			if (item == null) return;

			if (string.IsNullOrWhiteSpace(_newText))
			{
				Console.WriteLine("Ошибка: не указан новый текст задачи");
				return;
			}

			_oldText = item.Text;
			item.UpdateText(_newText);
			Console.WriteLine($"Задача обновлена: {item.Text}");
		}

		public void Unexecute()
		{
			var item = _todoList.GetItem(_index);
			if (item != null && _oldText != null)
			{
				item.UpdateText(_oldText);
				Console.WriteLine($"Отменено обновление задачи: {item.Text}");
			}
		}
	}
}