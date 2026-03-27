namespace Todolist;

public class UpdateCommand : ICommand
{
	public int TaskNumber { get; set; }
	public string NewText { get; set; }
	public string TodoFilePath { get; set; }
	public string Description => $"Обновление задачи #{TaskNumber}";
	private string _oldText;
	private TodoItem _targetItem;

	public void Execute()
	{
		if (string.IsNullOrWhiteSpace(NewText))
		{
			Console.WriteLine("Ошибка: новый текст не может быть пустым");
			return;
		}

		if (TaskNumber > 0 && TaskNumber <= AppInfo.Todos.Count)
		{
			int index = TaskNumber - 1;

			// Запоминаем
			_targetItem = AppInfo.Todos.GetItem(index);
			_oldText = _targetItem.Text;

			// Меняем
			_targetItem.UpdateText(NewText);
			Console.WriteLine($"Задача обновлена: '{_oldText}' -> '{NewText}'");

			// Сохраняем
			FileManager.SaveTodos(AppInfo.Todos, AppInfo.TodosFilePath);
		}
		else
		{
			Console.WriteLine("Неверный номер задачи");
		}
	}
	public void Unexecute()
	{
		// ОТМЕНЯЕМ обновление - возвращаем старый текст
		if (_targetItem != null && _oldText != null)
		{
			_targetItem.UpdateText(_oldText);
			Console.WriteLine($"Отмена: текст возвращен к '{_oldText}'");

			// Сохраняем
			FileManager.SaveTodos(AppInfo.Todos, AppInfo.TodosFilePath);
		}
	}
}