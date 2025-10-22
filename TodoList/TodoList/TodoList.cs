public class TodoList
{
	private TodoItem[] items;
	private int count;
	public TodoList(int initialCapacity = 4)
	{
		items = new TodoItem[initialCapacity];
		count = 0;
	}
	public int Count => count;
	public void Add(TodoItem item)
	{
		if (count == items.Length)
			IncreaseArray(items, item);
		else
			items[count++] = item;
	}
	public void Delete(int index)
	{
		if (index < 0 || index >= count)
			throw new ArgumentOutOfRangeException(nameof(index), "Индекс вне диапазона");
		for (int i = index; i < count - 1; i++)
			items[i] = items[i + 1];
		items[--count] = null;
	}
	public void View(bool showIndex = false, bool showDone = false, bool showDate = false)
	{
		if (count == 0)
		{
			Console.WriteLine("Список задач пуст");
			return;
		}
		bool showStatus = showDone;
		bool showUpdateDate = showDate;
		if (showIndex && showStatus && showUpdateDate)
		{
			Console.WriteLine("Ваш список задач:");
			PrintTableHeader(showIndex, showStatus, showUpdateDate);
			PrintTableSeparator(showIndex, showStatus, showUpdateDate);
			for (int i = 0; i < count; i++)
				if (items[i] != null)
					PrintTaskRow(i, items[i].Text, items[i].IsDone, items[i].LastUpdate, showIndex, showStatus, showUpdateDate);
			return;
		}
		if (!showIndex && !showStatus && !showUpdateDate)
		{
			Console.WriteLine("Ваш список задач:");
			for (int i = 0; i < count; i++)
				if (items[i] != null)
					Console.WriteLine(items[i].GetShortInfo());
			return;
		}
		Console.WriteLine("Ваш список задач:");
		PrintTableHeader(showIndex, showStatus, showUpdateDate);
		PrintTableSeparator(showIndex, showStatus, showUpdateDate);
		for (int i = 0; i < count; i++)
			if (items[i] != null)
				PrintTaskRow(i, items[i].Text, items[i].IsDone, items[i].LastUpdate, showIndex, showStatus, showUpdateDate);
	}
	public TodoItem GetItem(int index)
	{
		if (index < 0 || index >= count)
			throw new ArgumentOutOfRangeException(nameof(index), "Индекс вне диапазона");
		return items[index];
	}
	private void IncreaseArray(TodoItem[] oldArray, TodoItem newItem)
	{
		TodoItem[] newArray = new TodoItem[oldArray.Length * 2];
		for (int i = 0; i < oldArray.Length; i++)
			newArray[i] = oldArray[i];
		newArray[count] = newItem;
		count++;
		items = newArray;
	}
	private void PrintTableHeader(bool showIndex, bool showStatus, bool showDate)
	{
		List<string> headers = new List<string>();
		if (showIndex) headers.Add($"{"Индекс",-6}");
		headers.Add($"{"Задача",-30}");
		if (showStatus) headers.Add($"{"Статус",-10}");
		if (showDate) headers.Add($"{"Дата изменения",-19}");
		Console.WriteLine("| " + string.Join(" | ", headers) + " |");
	}
	private void PrintTableSeparator(bool showIndex, bool showStatus, bool showDate)
	{
		List<string> separators = new List<string>();
		if (showIndex) separators.Add(new string('-', 6));
		separators.Add(new string('-', 30));
		if (showStatus) separators.Add(new string('-', 10));
		if (showDate) separators.Add(new string('-', 19));
		Console.WriteLine("|-" + string.Join("-|-", separators) + "-|");
	}
	private void PrintTaskRow(int index, string task, bool status, DateTime date, bool showIndex, bool showStatus, bool showDate)
	{
		List<string> columns = new List<string>();
		if (showIndex) columns.Add($"{index,6}");
		string taskText = GetTruncatedTaskText(task);
		columns.Add($"{taskText,-30}");
		if (showStatus) columns.Add($"{(status ? "Выполнена" : "Не выполнена"),-10}");
		if (showDate) columns.Add($"{date:dd.MM.yyyy HH:mm:ss}");
		Console.WriteLine("| " + string.Join(" | ", columns) + " |");
	}
	private string GetTruncatedTaskText(string taskText)
	{
		if (string.IsNullOrEmpty(taskText)) return "";
		taskText = taskText.Replace("\r", " ").Replace("\n", " ");
		while (taskText.Contains("  "))
			taskText = taskText.Replace("  ", " ");
		return taskText.Length <= 30 ? taskText : taskText.Substring(0, 27) + "...";
	}
}
