namespace TodoList.Commands
{
	public class UpdateCommand : ICommand
	{
		public string Arg { get; set; } = string.Empty;
		public TodoList TodoList { get; set; } = null!;

		private string _originalText;
		private int _updatedIndex;

		public void Execute()
		{
			var parts = Arg.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length < 2 || !int.TryParse(parts[0], out int idx))
			{
				Console.WriteLine("Ошибка: укажите номер задачи и текст");
				return;
			}

			_updatedIndex = idx - 1;
			if (_updatedIndex < 0 || _updatedIndex >= TodoList.tasks.Count)
			{
				Console.WriteLine("Ошибка: некорректный номер задачи");
				return;
			}

			_originalText = TodoList.tasks[_updatedIndex].Text;
			TodoList.tasks[_updatedIndex].UpdateText(parts[1].Trim('"', '\''));
			Console.WriteLine("Задача обновлена");
			FileManager.SaveTodos(TodoList, "data/todo.csv");
		}

		public void Unexecute()
		{
			if (_originalText != null && TodoList != null)
			{
				TodoList.tasks[_updatedIndex].UpdateText(_originalText);
				Console.WriteLine("Обновление задачи отменено");
				FileManager.SaveTodos(TodoList, "data/todo.csv");
			}
		}
	}
}