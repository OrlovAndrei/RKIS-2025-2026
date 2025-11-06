namespace TodoApp.Commands;

public class RemoveCommand : ICommand
{
	public string Name => "remove";
	public string Description => "Удалить задачу";

	// Индекс задачи
	public int TaskIndex { get; set; }

	// Флаг для принудительного удаления
	public bool Force { get; set; }

	// Свойства для работы с данными
	public TodoList TodoList { get; set; }

	public bool Execute()
	{
		if (TodoList == null)
		{
			Console.WriteLine(" Ошибка: TodoList не установлен");
			return false;
		}

		if (TodoList.IsEmpty)
		{
			Console.WriteLine("📝 Список задач пуст!");
			return false;
		}

		try
		{
			TodoItem task = TodoList.GetItem(TaskIndex - 1);
			string shortText = GetShortText(task.Text);

			if (Force)
			{
				TodoList.Delete(TaskIndex - 1);
				Console.WriteLine(" Задача успешно удалена!");
				return true;
			}
			else
			{
				Console.Write($" Вы уверены, что хотите удалить задачу '{shortText}'? (y/n): ");
				string confirmation = Console.ReadLine()?.Trim().ToLower();

				if (confirmation == "y" || confirmation == "yes" || confirmation == "д" || confirmation == "да")
				{
					TodoList.Delete(TaskIndex - 1);
					Console.WriteLine(" Задача успешно удалена!");
					return true;
				}
				else
				{
					Console.WriteLine("Удаление отменено.");
					return true;
				}
			}
		}
		catch (ArgumentOutOfRangeException)
		{
			Console.WriteLine($"Ошибка: задача с номером {TaskIndex} не найдена!");
			return false;
		}
	}

	private string GetShortText(string text)
	{
		if (string.IsNullOrEmpty(text)) return "";
		string shortText = text.Replace("\n", " ").Replace("\r", " ");
		return shortText.Length > 30 ? shortText.Substring(0, 30) + "..." : shortText;
	}
}