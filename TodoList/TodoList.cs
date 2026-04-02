using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoList;

public class TodoList
{
    private TodoItem[] items = new TodoItem[2];
    private int taskCount;

    public int Count => taskCount;

    public void Add(TodoItem item, bool silent = false)
    {
        if (taskCount == items.Length)
            IncreaseArray();

        items[taskCount] = item;
        if (!silent)
            Console.WriteLine($"Задача добавлена: {item.Text}");
        taskCount++;
    }

    public void Delete(int idx)
    {
        if (idx < 0 || idx >= taskCount)
        {
            Console.WriteLine("Ошибка: неверный индекс");
            return;
        }
        for (var i = idx; i < taskCount - 1; i++)
        {
            items[i] = items[i + 1];
        }
        taskCount--;
    }

    public void MarkDone(int idx)
    {
        if (idx < 0 || idx >= taskCount)
        {
            Console.WriteLine("Ошибка: неверный индекс");
            return;
        }
        items[idx].MarkDone();
    }

    public void Update(int idx, string newText)
    {
        if (idx < 0 || idx >= taskCount)
        {
            Console.WriteLine("Ошибка: неверный индекс");
            return;
        }
        items[idx].UpdateText(newText);
    }

    public void Read(int idx)
    {
        if (idx < 0 || idx >= taskCount)
        {
            Console.WriteLine("Ошибка: неверный индекс");
            return;
        }
        Console.WriteLine(items[idx].GetFullInfo());
    }

    public void View(bool showIndex, bool showStatus, bool showUpdateDate)
    {
        if (taskCount == 0)
        {
            Console.WriteLine("Список задач пуст.");
            return;
        }

        List<string> headers = new List<string> { "Текст задачи".PadRight(36) };
        if (showIndex) headers.Add("Индекс".PadRight(8));
        if (showStatus) headers.Add("Статус".PadRight(16));
        if (showUpdateDate) headers.Add("Дата обновления".PadRight(16));

        Console.WriteLine("+-" + string.Join("---", headers.Select(it => new string('-', it.Length))) + "-+");
        Console.WriteLine("| " + string.Join(" | ", headers) + " |");
        Console.WriteLine("|-" + string.Join("-+-", headers.Select(it => new string('-', it.Length))) + "-|");

        for (int i = 0; i < taskCount; i++)
        {
            string text = items[i].GetShortInfo().Replace("\n", " ");
            string status = items[i].IsDone ? "выполнена" : "не выполнена";
            string date = items[i].LastUpdate.ToString("yyyy-MM-dd HH:mm");

            List<string> rows = new List<string> { text.PadRight(36) };
            if (showIndex) rows.Add((i + 1).ToString().PadRight(8));
            if (showStatus) rows.Add(status.PadRight(16));
            if (showUpdateDate) rows.Add(date.PadRight(16));

            Console.WriteLine("| " + string.Join(" | ", rows) + " |");
        }
        Console.WriteLine("+-" + string.Join("---", headers.Select(it => new string('-', it.Length))) + "-+");
    }

    public TodoItem GetItem(int index)
    {
        if (index < 0 || index >= taskCount)
            throw new IndexOutOfRangeException();
        return items[index];
    }

    private void IncreaseArray()
    {
        var newSize = items.Length * 2;
        Array.Resize(ref items, newSize);
    }
}