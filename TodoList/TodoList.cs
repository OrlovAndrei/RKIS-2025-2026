using System;
using System.Collections.Generic;
namespace TodoApp;
public class TodoList
{
	private List<TodoItem> _items;
	public int Count => _items.Count;
	public TodoItem this[int index] => _items[index];
	public TodoList()
	{
		_items = new List<TodoItem>();
	}
	public void Add(TodoItem item)
	{
        _items.Add(item);
        Console.WriteLine($"Задача добавлена: {item.Text}");
	}
	public void Delete(int index)
	{
        if (index < 0 || index >= _items.Count)
        {
            Console.WriteLine("Неверный номер задачи.");
            return;
        }
        _items.RemoveAt(index);
        Console.WriteLine($"Задача {index + 1} удалена.");
	}
    public void SetStatus(int index, TodoStatus status)
    {
        if (index < 0 || index >= _items.Count)
        {
            Console.WriteLine("Неверный номер задачи.");
            return;
        }
        var item = _items[index];
        item.Status = status;
        Console.WriteLine($"Статус задачи '{item.Text}' изменен на: {TodoItem.GetStatusDisplayName(status)}");
    }
	public TodoItem GetItem(int index)
	{
		if (index < 0 || index >= _items.Count)
		{
			Console.WriteLine("Неверный номер задачи.");
			return null;
		}
		return _items[index];
	}
	public void View(bool showIndex = false, bool showDone = true, bool showDate = false)
	{
        if (_items.Count == 0)
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
        int indexCounter = 0;
        foreach (var item in _items)
        {
            if (!showDone && item.Status == TodoStatus.Completed) 
            {
                indexCounter++;
                continue;
            }
            var row = new List<string>();
            if (showIndex) row.Add((indexCounter + 1).ToString());
            string displayText = item.Text.Replace("\n", " | ").Replace("\r", "");
            if (displayText.Length > 30)
                displayText = displayText.Substring(0, 30) + "...";
            row.Add(displayText);
            if (showDate) row.Add(item.LastUpdate.ToString("dd.MM.yyyy HH:mm"));
            if (showDone) row.Add(TodoItem.GetStatusDisplayName(item.Status));
            table.Add(row.ToArray());
            indexCounter++;
        }
        PrintTable(table);
    }
	public void ViewByStatus(TodoStatus statusFilter)
    {
        if (_items.Count == 0)
        {
            Console.WriteLine("Задач нет!");
            return;
        }
        var table = new List<string[]>();
        table.Add(new string[] { "№", "Задача", "Статус", "Дата изменения" });
        int indexCounter = 0;
		int displayedCount = 0;
        foreach (var item in _items)
        {
            if (item.Status == statusFilter)
            {
                var row = new List<string>();
                row.Add((indexCounter + 1).ToString());
                string displayText = item.Text.Replace("\n", " | ").Replace("\r", "");
                if (displayText.Length > 30)
                    displayText = displayText.Substring(0, 30) + "...";
                row.Add(displayText);
                row.Add(TodoItem.GetStatusDisplayName(item.Status));
                row.Add(item.LastUpdate.ToString("dd.MM.yyyy HH:mm"));
                table.Add(row.ToArray());
            }
            indexCounter++;
        }
        if (table.Count == 1)
        {
            Console.WriteLine($"Задачи со статусом '{TodoItem.GetStatusDisplayName(statusFilter)}' не найдены.");
            return;
        }
        PrintTable(table);
    }
	public IEnumerator<TodoItem> GetEnumerator()
	{
    	foreach (var item in _items)
    	{
        	yield return item;
    	}
	}
	private void PrintTable(List<string[]> table)
	{
		if (table.Count == 0) return;
		int[] columnWidths = new int[table[0].Length];
		for (int i = 0; i < table.Count; i++)
			for (int j = 0; j < table[i].Length; j++)
				if (table[i][j].Length > columnWidths[j])
					columnWidths[j] = table[i][j].Length;
		if (columnWidths.Length > 1 && columnWidths[1] > 50)
			columnWidths[1] = 50;
        Console.Write("┌");
        for (int j = 0; j < columnWidths.Length; j++)
        {
            Console.Write(new string('─', columnWidths[j] + 2));
            if (j < columnWidths.Length - 1) Console.Write("┬");
        }
        Console.WriteLine("┐");
        for (int i = 0; i < table.Count; i++)
        {
            Console.Write("│");
            for (int j = 0; j < table[i].Length; j++)
            {
                Console.Write(" " + table[i][j].PadRight(columnWidths[j] + 1) + "│");
            }
            Console.WriteLine();
            if (i == 0)
            {
                Console.Write("├");
                for (int j = 0; j < columnWidths.Length; j++)
                {
                    Console.Write(new string('─', columnWidths[j] + 2));
                    if (j < columnWidths.Length - 1) Console.Write("┼");
                }
                Console.WriteLine("┤");
            }
            else if (i < table.Count - 1)
            {
                Console.Write("├");
                for (int j = 0; j < columnWidths.Length; j++)
                {
                    Console.Write(new string('─', columnWidths[j] + 2));
                    if (j < columnWidths.Length - 1) Console.Write("┼");
                }
                Console.WriteLine("┤");
            }
        }
        string bottomBorder = "└";
        for (int j = 0; j < columnWidths.Length; j++)
        {
            bottomBorder += new string('─', columnWidths[j] + 2);
            if (j < columnWidths.Length - 1) bottomBorder += "┴";
        }
        bottomBorder += "┘";
        Console.WriteLine(bottomBorder);
    }
}