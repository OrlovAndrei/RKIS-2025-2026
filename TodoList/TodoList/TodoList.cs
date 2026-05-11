using System;
using System.Collections;
using System.Collections.Generic;
public class TodoList : IEnumerable<TodoItem>
{
	private List<TodoItem> items;
	public event Action<TodoItem>? OnTodoAdded;
	public event Action<TodoItem>? OnTodoDeleted;
	public event Action<TodoItem>? OnTodoUpdated;
	public event Action<TodoItem>? OnStatusChanged;
	public TodoList()
	{
		items = new List<TodoItem>();
	}
	public TodoList(List<TodoItem> items)
	{
		this.items = items;
	}
	public int Count => items.Count;
	public TodoItem this[int index]
	{
		get
		{
			if (index < 0 || index >= items.Count)
				throw new ArgumentOutOfRangeException(nameof(index), "Индекс вне диапазона");
			return items[index];
		}
	}
	public void Add(TodoItem item)
	{
		items.Add(item);
		OnTodoAdded?.Invoke(item);
	}
	public void Delete(int index)
	{
		if (index < 0 || index >= items.Count)
			throw new ArgumentOutOfRangeException(nameof(index), "Индекс вне диапазона");

		var item = items[index];
		items.RemoveAt(index);
		OnTodoDeleted?.Invoke(item);
	}
	public IEnumerator<TodoItem> GetEnumerator()
	{
		foreach (var item in items)
		{
			yield return item;
		}
	}
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
	public void View(bool showIndex = false, bool showStatus = false, bool showDate = false)
	{
		if (items.Count == 0)
		{
			Console.WriteLine("Список задач пуст");
			return;
		}
		Console.WriteLine("Ваш список задач:");
		if (!showIndex && !showStatus && !showDate)
		{
			foreach (var item in items)
			{
				Console.WriteLine(item.GetShortInfo());
			}
			return;
		}
		PrintTableHeader(showIndex, showStatus, showDate);
		PrintTableSeparator(showIndex, showStatus, showDate);

		for (int i = 0; i < items.Count; i++)
		{
			PrintTaskRow(i, items[i].GetText(), items[i].GetIsDone(), items[i].GetLastUpdate(), showIndex, showStatus, showDate);
		}
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
		if (showStatus)
		{
			string statusText = items[index].GetStatusText();
			columns.Add($"{statusText,-12}");
		}
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
	public void Update(int index, string newText)
	{
		if (index < 0 || index >= items.Count)
			throw new ArgumentOutOfRangeException(nameof(index), "Индекс вне диапазона");
		items[index].UpdateText(newText);
		OnTodoUpdated?.Invoke(items[index]);
	}
	public void SetStatus(int index, TodoStatus status)
	{
		if (index < 0 || index >= items.Count)
			throw new ArgumentOutOfRangeException(nameof(index), "Индекс вне диапазона");
		items[index].UpdateStatus(status);
		OnStatusChanged?.Invoke(items[index]);
	}
	public TodoItem GetItem(int index)
	{
		if (index < 0 || index >= items.Count)
			throw new ArgumentOutOfRangeException(nameof(index), "Индекс вне диапазона");
		return items[index];
	}
}