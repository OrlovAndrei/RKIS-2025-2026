using System;
using System.Collections.Generic;
public class TodoList
{
	private TodoItem[] _items;
	private int _count;
	public TodoList(int initialCapacity = 3)
	{
		_items = new TodoItem[initialCapacity];
		_count = 0;
	}
	public void Add(TodoItem item)
	{
		if (_count >= _items.Length)
		{
			IncreaseArray();
		}
		_items[_count] = item;
		_count++;
		Console.WriteLine($"Задача добавлена: {item.Text}");
	}
	public void Delete(int index)
	{
		if (index < 0 || index >= _count)
		{
			Console.WriteLine("Неверный номер задачи.");
			return;
		}
		for (int i = index; i < _count - 1; i++)
		{
			_items[i] = _items[i + 1];
		}
		_items[_count - 1] = null;
		_count--;
		Console.WriteLine($"Задача {index + 1} удалена.");
	}
	public TodoItem GetItem(int index)
	{
		if (index < 0 || index >= _count)
		{
			Console.WriteLine("Неверный номер задачи.");
			return null;
		}
		return _items[index];
	}
	public void View(bool showIndex = false, bool showDone = true, bool showDate = false)
	{
		if (_count == 0)
		{
			Console.WriteLine("Задач нет!");
			return;
		}
		var table = new List<string[]>();
		var headers = new List<string>();
		if (showIndex) headers.Add("№");
		headers.Add("Задача");
		if (showDate) headers.Add("Дата изменения");
		if (showDone) headers.Add("Статус");
		table.Add(headers.ToArray());
		for (int i = 0; i < _count; i++)
		{
			if (!showDone && _items[i].IsDone) continue;
			var row = new List<string>();
			if (showIndex) row.Add((i + 1).ToString());
			string displayText = _items[i].Text.Replace("\n", " | ").Replace("\r", "");
			if (displayText.Length > 30)
			{
				displayText = displayText.Substring(0, 30) + "...";
			}
			row.Add(displayText);
			if (showDate) row.Add(_items[i].LastUpdate.ToString("dd.MM.yyyy HH:mm"));
			if (showDone) row.Add(_items[i].IsDone ? "Выполнено" : "Не выполнено");
			table.Add(row.ToArray());
		}
		PrintTable(table);
	}
	private void IncreaseArray()
	{
		int newSize = _items.Length * 2;
		TodoItem[] newArray = new TodoItem[newSize];
		for (int i = 0; i < _count; i++)
		{
			newArray[i] = _items[i];
		}
		_items = newArray;
		Console.WriteLine("Массив задач расширен!");
	}
	private void PrintTable(List<string[]> table)
	{
		if (table.Count == 0) return;
		int[] columnWidths = new int[table[0].Length];
		for (int i = 0; i < table.Count; i++)
		{
			for (int j = 0; j < table[i].Length; j++)
			{
				if (table[i][j].Length > columnWidths[j])
				{
					columnWidths[j] = table[i][j].Length;
				}
			}
		}
		if (columnWidths.Length > 1 && columnWidths[1] > 50)
		{
			columnWidths[1] = 50;
		}
		int totalWidth = GetTotalWidth(columnWidths);
		Console.WriteLine("\n" + new string('-', totalWidth));
		for (int i = 0; i < table.Count; i++)
		{
			Console.Write("|");
			for (int j = 0; j < table[i].Length; j++)
			{
				string cellContent = table[i][j];
				if (cellContent.Length > 50 && j == 1)
				{
					cellContent = cellContent.Substring(0, 47) + "...";
				}
				Console.Write($" {cellContent.PadRight(columnWidths[j])} |");
			}
			Console.WriteLine();

			if (i == 0)
			{
				Console.WriteLine(new string('-', totalWidth));
			}
		}
		Console.WriteLine(new string('-', totalWidth));
	}
	private int GetTotalWidth(int[] columnWidths)
	{
		int total = columnWidths.Length + 1;
		foreach (int width in columnWidths)
		{
			total += width + 2;
		}
		return total;
	}
}