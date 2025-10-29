using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TodoList;
public class banan2
{
	const int IndexWidth = 6;
	const int textWidth = 36;
	const int statusWidth = 14;
	const int dateWidth = 16;

	kakos1[] kakosiki = new kakos1[2];
	int taskCount = 0;

	public void Add(kakos1 item)
	{
		if (taskCount == kakosiki.Length)
			IncreaseArray();

		kakosiki[taskCount] = item;
		Console.WriteLine($"Добавлена задача: {taskCount}) {item.Text}");
		taskCount++;
	}

	public void Delete(int idx)
	{
		for (var i = idx; i < taskCount - 1; i++)
		{
			kakosiki[i] = kakosiki[i + 1];
		}

		taskCount--;
		Console.WriteLine($"Задача {idx} удалена.");
	}

	public void MarkDone(int idx)
	{
		kakosiki[idx].MarkDone();
		Console.WriteLine($"Задача {kakosiki[idx].Text} отмечена выполненной");
	}

	public void Update(int idx, string newText)
	{
		kakosiki[idx].UpdateText(newText);
		Console.WriteLine("Задача обновлена");
	}

	public void Read(int idx)
	{
		Console.WriteLine(kakosiki[idx].GetFullInfo(idx));
	}

	public void View(bool showIndex, bool showStatus, bool showUpdateDate)
	{
		List<string> headers = ["Текст задачи".PadRight(textWidth)];
		if (showIndex) headers.Add("Индекс".PadRight(IndexWidth));
		if (showStatus) headers.Add("Статус".PadRight(statusWidth));
		if (showUpdateDate) headers.Add("Дата обновления".PadRight(dateWidth));

		Console.WriteLine("+-" + string.Join("---", headers.Select(it => new string('-', it.Length))) + "-+");
		Console.WriteLine("| " + string.Join(" | ", headers) + " |");
		Console.WriteLine("|-" + string.Join("-+-", headers.Select(it => new string('-', it.Length))) + "-|");

		for (int i = 0; i < taskCount; i++)
		{
			string text = kakosiki[i].Text.Replace("\n", " ");
			if (text.Length > 30) text = text.Substring(0, 30) + "...";

			string status = kakosiki[i].IsDone ? "выполнена" : "не выполнена";
			string date = kakosiki[i].LastUpdate.ToString("yyyy-MM-dd HH:mm");

			List<string> rows = [text.PadRight(textWidth)];
			if (showIndex) rows.Add((i + 1).ToString().PadRight(IndexWidth));
			if (showStatus) rows.Add(status.PadRight(statusWidth));
			if (showUpdateDate) rows.Add(date.PadRight(dateWidth));

			Console.WriteLine("| " + string.Join(" | ", rows) + " |");
		}
		Console.WriteLine("+-" + string.Join("---", headers.Select(it => new string('-', it.Length))) + "-+");
	}

	private void IncreaseArray()
	{
		var newSize = kakosiki.Length * 2;
		Array.Resize(ref kakosiki, newSize);
	}
}
