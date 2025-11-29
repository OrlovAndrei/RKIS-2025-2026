using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

class TodoList : IEnumerable<TodoItem>
{
    private const int TaskTextMaxDisplay = 30;
    
    private List<TodoItem> items;

    public TodoList()
    {
        items = new List<TodoItem>();
    }

    public void Add(TodoItem item)
    {
        items.Add(item);
    }

    public void Delete(int index)
    {
        if (index < 1 || index > items.Count)
        {
            throw new ArgumentException("Индекс вне диапазона.");
        }

        int zeroBasedIndex = index - 1;
        items.RemoveAt(zeroBasedIndex);
    }

    public void View(bool showIndex, bool showDone, bool showDate)
    {
        Console.WriteLine("Ваши задачи:");
        if (items.Count == 0)
        {
            Console.WriteLine(" (список пуст)");
            return;
        }

        // Подготовка ширин колонок
        int idxWidth = showIndex ? Math.Max(3, (items.Count.ToString().Length + 1)) : 0;
        int statusWidth = showDone ? 15 : 0;
        int dateWidth = showDate ? 20 : 0;
        int textWidth = TaskTextMaxDisplay;

        // Заголовок
        StringBuilder header = new StringBuilder();
        if (showIndex)
            header.Append(PadCenter("Idx", idxWidth) + " | ");
        header.Append(PadCenter("Text", textWidth));
        if (showDone)
            header.Append(" | " + PadCenter("Status", statusWidth));
        if (showDate)
            header.Append(" | " + PadCenter("Updated", dateWidth));

        Console.WriteLine(header.ToString());
        Console.WriteLine(new string('-', header.Length));

        // Строки
        for (int i = 0; i < items.Count; i++)
        {
            string text = items[i].Text ?? string.Empty;
            string textDisplay = TruncateWithEllipsis(text, textWidth);

            StringBuilder row = new StringBuilder();
            if (showIndex)
                row.Append((i + 1).ToString().PadRight(idxWidth) + " | ");
            row.Append(textDisplay.PadRight(textWidth));
            if (showDone)
            {
                string state = GetStatusString(items[i].Status);
                row.Append(" | " + state.PadRight(statusWidth));
            }
            if (showDate)
            {
                string d = items[i].LastUpdate == default ? "-" : items[i].LastUpdate.ToString("yyyy-MM-dd HH:mm");
                row.Append(" | " + d.PadRight(dateWidth));
            }

            Console.WriteLine(row.ToString());
        }
    }

    public void Read(int index)
    {
        if (index < 1 || index > items.Count)
        {
            throw new ArgumentException("Индекс вне диапазона.");
        }

        int zeroBasedIndex = index - 1;
        TodoItem item = items[zeroBasedIndex];
        
        Console.WriteLine($"Задача {index}:");
        Console.WriteLine("-----------");
        Console.WriteLine(item.Text);
        Console.WriteLine("-----------");
        Console.WriteLine($"Статус: {GetStatusString(item.Status)}");
        Console.WriteLine($"Дата последнего изменения: {(item.LastUpdate == default ? "-" : item.LastUpdate.ToString("yyyy-MM-dd HH:mm"))}");
    }

    public int Count
    {
        get { return items.Count; }
    }

    // Индексатор для доступа к задачам по индексу (1-based)
    public TodoItem this[int index]
    {
        get
        {
            if (index < 1 || index > items.Count)
            {
                throw new ArgumentException("Индекс вне диапазона.");
            }
            return items[index - 1];
        }
    }

    public TodoItem GetItem(int index)
    {
        if (index < 1 || index > items.Count)
        {
            throw new ArgumentException("Индекс вне диапазона.");
        }
        return items[index - 1];
    }

    // Метод-итератор с использованием yield return
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

    private string GetStatusString(TodoStatus status)
    {
        return status switch
        {
            TodoStatus.NotStarted => "не начато",
            TodoStatus.InProgress => "в процессе",
            TodoStatus.Completed => "выполнено",
            TodoStatus.Postponed => "отложено",
            TodoStatus.Failed => "провалено",
            _ => "неизвестно"
        };
    }

    private string TruncateWithEllipsis(string s, int max)
    {
        if (s == null) return new string(' ', max);
        if (s.Length <= max) return s;
        if (max <= 3) return s.Substring(0, max);
        return s.Substring(0, max - 3) + "...";
    }

    private string PadCenter(string text, int width)
    {
        if (width <= 0) return string.Empty;
        if (text == null) text = string.Empty;
        if (text.Length >= width) return text.Substring(0, width);
        int left = (width - text.Length) / 2;
        int right = width - text.Length - left;
        return new string(' ', left) + text + new string(' ', right);
    }
}

