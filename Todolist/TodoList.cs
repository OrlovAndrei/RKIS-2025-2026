using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

class TodoList : IEnumerable<TodoItem>
{
    private const int TaskTextMaxDisplay = 30;
    
    private List<TodoItem> items;

    public event Action<TodoItem>? OnTodoAdded;
    public event Action<TodoItem>? OnTodoDeleted;
    public event Action<TodoItem>? OnTodoUpdated;
    public event Action<TodoItem>? OnStatusChanged;

    public TodoList()
    {
        items = new List<TodoItem>();
    }

    public void Add(TodoItem item)
    {
        items.Add(item);
        OnTodoAdded?.Invoke(item);
    }

    public void Insert(int index, TodoItem item)
    {
        if (index < 1 || index > items.Count + 1)
        {
            throw new ArgumentException("Неверный индекс задачи.");
        }

        int zeroBasedIndex = index - 1;
        items.Insert(zeroBasedIndex, item);
        OnTodoAdded?.Invoke(item);
    }

    public void Delete(int index)
    {
        if (index < 1 || index > items.Count)
        {
            throw new ArgumentException("Неверный индекс задачи.");
        }

        int zeroBasedIndex = index - 1;
        var removed = items[zeroBasedIndex];
        items.RemoveAt(zeroBasedIndex);
        OnTodoDeleted?.Invoke(removed);
    }

    public void Update(int index, string newText)
    {
        if (index < 1 || index > items.Count)
        {
            throw new ArgumentException("Неверный индекс задачи.");
        }

        int zeroBasedIndex = index - 1;
        items[zeroBasedIndex].UpdateText(newText);
        OnTodoUpdated?.Invoke(items[zeroBasedIndex]);
    }

    public void View(bool showIndex, bool showDone, bool showDate)
    {
        Console.WriteLine("Список задач:");
        if (items.Count == 0)
        {
            Console.WriteLine(" (пусто)");
            return;
        }

        int idxWidth = showIndex ? Math.Max(3, (items.Count.ToString().Length + 1)) : 0;
        int statusWidth = showDone ? 15 : 0;
        int dateWidth = showDate ? 20 : 0;
        int textWidth = TaskTextMaxDisplay;

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
            throw new ArgumentException("Неверный индекс задачи.");
        }

        int zeroBasedIndex = index - 1;
        TodoItem item = items[zeroBasedIndex];
        
        Console.WriteLine($"Задача {index}:");
        Console.WriteLine("-----------");
        Console.WriteLine(item.Text);
        Console.WriteLine("-----------");
        Console.WriteLine($"Статус: {GetStatusString(item.Status)}");
        Console.WriteLine($"Дата обновления: {(item.LastUpdate == default ? "-" : item.LastUpdate.ToString("yyyy-MM-dd HH:mm"))}");
    }

    public int Count => items.Count;

    public TodoItem this[int index]
    {
        get
        {
            if (index < 1 || index > items.Count)
            {
                throw new ArgumentException("Неверный индекс задачи.");
            }
            return items[index - 1];
        }
    }

    public TodoItem GetItem(int index)
    {
        if (index < 1 || index > items.Count)
        {
            throw new ArgumentException("Неверный индекс задачи.");
        }
        return items[index - 1];
    }

    public void SetStatus(int index, TodoStatus status)
    {
        if (index < 1 || index > items.Count)
        {
            throw new ArgumentException("Неверный индекс задачи.");
        }

        int zeroBasedIndex = index - 1;
        items[zeroBasedIndex].Status = status;
        items[zeroBasedIndex].LastUpdate = DateTime.Now;
        OnStatusChanged?.Invoke(items[zeroBasedIndex]);
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

    private string GetStatusString(TodoStatus status)
    {
        return status switch
        {
            TodoStatus.NotStarted => "Не начата",
            TodoStatus.InProgress => "В работе",
            TodoStatus.Completed => "Завершена",
            TodoStatus.Postponed => "Отложена",
            TodoStatus.Failed => "Провалена",
            _ => "Неизвестно"
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

