using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList;
public class TodoList
{
	TodoItem[] items = new TodoItem[2];
	int index = 0;

	public void Add(TodoItem item)
	{
		if (index == items.Length)
			IncreaseArray();

		items[index] = item;
		Console.WriteLine($"Добавлена задача: {index}) {item.Text}");
		index++;
	}

	public void Delete(int idx)
	{
		for (var i = idx; i < index - 1; i++)
		{
			items[i] = items[i + 1];
		}

		index--;
		Console.WriteLine($"Задача {idx} удалена.");
	}

	public void MarkDone(int idx)
	{
		items[idx].MarkDone();
		Console.WriteLine($"Задача {items[idx].Text} отмечена выполненной");
	}

	public void Update(int idx, string newText)
	{
		items[idx].UpdateText(newText);
		Console.WriteLine("Задача обновлена");
	}

	public void Read(int idx)
	{
		Console.WriteLine(items[idx].GetFullInfo(idx));
	}

	public void View(bool showIndex, bool showStatus, bool showDate)
	{
		int indexWidth = 8;
		int textWidth = 34;
		int statusWidth = 16;
		int dateWidth = 16;

		string headerRow = "Текст задачи".PadRight(textWidth) + "|";
		if (showIndex) headerRow += "Индекс".PadRight(indexWidth) + "|";
		if (showStatus) headerRow += "Статус".PadRight(statusWidth) + "|";
		if (showDate) headerRow += "Дата обновления".PadRight(dateWidth) + "|";

		Console.WriteLine(headerRow);
		Console.WriteLine(new string('-', headerRow.Length));

		for (int i = 0; i < index; i++)
		{
			string text = items[i].Text.Replace("\r", " ").Replace("\n", " ");
			if (text.Length > 30) text = text.Substring(0, 30) + "...";

			string status = items[i].IsDone ? "выполнена" : "не выполнена";
			string date = items[i].LastUpdate.ToString("yyyy-MM-dd HH:mm");

			string row = text.PadRight(textWidth) + "|";
			if (showIndex) row += i.ToString().PadRight(indexWidth) + "|";
			if (showStatus) row += status.PadRight(statusWidth) + "|";
			if (showDate) row += date.PadRight(dateWidth) + "|";

			Console.WriteLine(row);
		}
	}

	private void IncreaseArray()
	{
		var newSize = items.Length * 2;
		Array.Resize(ref items, newSize);
	}
}
