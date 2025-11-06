namespace TodoApp;

public class TodoList
{
	// Приватное поле: массив задач
	private TodoItem[] items;

	// Конструктор
	public TodoList(int initialCapacity = 10)
	{
		items = new TodoItem[initialCapacity];
		Count = 0;
	}

	// Метод для добавления задачи
	public void Add(TodoItem item)
	{
		if (Count >= items.Length)
		{
			IncreaseArray(items, item);
		}
		else
		{
			items[Count] = item;
			Count++;
		}
	}

	// Метод для удаления задачи по индексу
	public void Delete(int index)
	{
		if (index < 0 || index >= Count)
		{
			throw new ArgumentOutOfRangeException(nameof(index), "Индекс находится вне диапазона");
		}

		// Сдвигаем элементы массива
		for (int i = index; i < Count - 1; i++)
		{
			items[i] = items[i + 1];
		}

		items[Count - 1] = null;
		Count--;
	}

	// Метод для вывода задач в виде таблицы
	public void View(bool showIndex = true, bool showDone = true, bool showDate = true)
	{
		if (Count == 0)
		{
			Console.WriteLine("Список задач пуст");
			return;
		}

		// Заголовок таблицы
		Console.WriteLine(new string('-', 80));
		string header = "";
		if (showIndex) header += "№".PadRight(5);
		header += "Задача".PadRight(35);
		if (showDone) header += "Статус".PadRight(15);
		if (showDate) header += "Дата изменения";
		Console.WriteLine(header);
		Console.WriteLine(new string('-', 80));

		// Вывод задач
		for (int i = 0; i < Count; i++)
		{
			string row = "";

			if (showIndex)
			{
				row += $"{i + 1}".PadRight(5);
			}

			// Обрезаем текст задачи до 30 символов и заменяем переносы строк
			string displayText = items[i].Text.Replace("\n", " ").Replace("\r", " ");
			string shortText = displayText.Length > 30 ?
				displayText.Substring(0, 30) + "..." :
				displayText;

			// Фиксированная ширина для текста задачи - 35 символов
			row += shortText.PadRight(35);

			if (showDone)
			{
				string status = items[i].IsDone ? "✓ Выполнена" : "✗ Не выполнена";
				// Фиксированная ширина для статуса - 15 символов
				row += status.PadRight(15);
			}

			if (showDate)
			{
				row += items[i].LastUpdate.ToString("dd.MM.yyyy HH:mm");
			}

			Console.WriteLine(row);
		}
		Console.WriteLine(new string('-', 80));
	}

	// Метод для получения задачи по индексу
	public TodoItem GetItem(int index)
	{
		if (index < 0 || index >= Count)
		{
			throw new ArgumentOutOfRangeException(nameof(index), "Индекс находится вне диапазона");
		}
		return items[index];
	}

	// Приватный метод для увеличения размера массива
	private void IncreaseArray(TodoItem[] oldArray, TodoItem newItem)
	{
		// Увеличиваем размер массива в 2 раза
		TodoItem[] newArray = new TodoItem[oldArray.Length * 2];

		// Копируем старые элементы
		for (int i = 0; i < oldArray.Length; i++)
		{
			newArray[i] = oldArray[i];
		}

		// Добавляем новый элемент
		newArray[Count] = newItem;
		Count++;

		// Заменяем старый массив новым
		items = newArray;
	}

	// Свойство для получения количества задач
	public int Count { get; private set; }

	// Свойство для проверки, пуст ли список
	public bool IsEmpty => Count == 0;
}
