using System;
using System.Text;

class TodoList
{
    private const int InitialCapacity = 2;
    private const int TaskTextMaxDisplay = 30;
    
    private TodoItem[] items;
    private int count;

    public TodoList()
    {
        items = new TodoItem[InitialCapacity];
        count = 0;
    }

    public void Add(TodoItem item)
    {
        if (count >= items.Length)
        {
            items = IncreaseArray(items);
        }
        
        items[count] = item;
        count++;
    }

    public void Delete(int index)
    {
        if (index < 1 || index > count)
        {
            throw new ArgumentException("Индекс вне диапазона.");
        }

        int zeroBasedIndex = index - 1;
        
        for (int i = zeroBasedIndex; i < count - 1; i++)
        {
            items[i] = items[i + 1];
        }

        items[count - 1] = null;
        count--;
    }

    public void View(bool showIndex, bool showDone, bool showDate)
    {
        Console.WriteLine("Ваши задачи:");
        if (count == 0)
        {
            Console.WriteLine(" (список пуст)");
            return;
        }

        // Подготовка ширин колонок
        int idxWidth = showIndex ? Math.Max(3, (count.ToString().Length + 1)) : 0;
        int statusWidth = showDone ? 10 : 0;
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
        for (int i = 0; i < count; i++)
        {
            string text = items[i].Text ?? string.Empty;
            string textDisplay = TruncateWithEllipsis(text, textWidth);

            StringBuilder row = new StringBuilder();
            if (showIndex)
                row.Append((i + 1).ToString().PadRight(idxWidth) + " | ");
            row.Append(textDisplay.PadRight(textWidth));
            if (showDone)
            {
                string state = items[i].IsDone ? "сделано" : "не сделано";
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
        if (index < 1 || index > count)
        {
            throw new ArgumentException("Индекс вне диапазона.");
        }

        int zeroBasedIndex = index - 1;
        TodoItem item = items[zeroBasedIndex];
        
        Console.WriteLine($"Задача {index}:");
        Console.WriteLine("-----------");
        Console.WriteLine(item.Text);
        Console.WriteLine("-----------");
        Console.WriteLine($"Статус: {(item.IsDone ? "выполнена" : "не выполнена")}");
        Console.WriteLine($"Дата последнего изменения: {(item.LastUpdate == default ? "-" : item.LastUpdate.ToString("yyyy-MM-dd HH:mm"))}");
    }

    private TodoItem[] IncreaseArray(TodoItem[] items)
    {
        int newSize = Math.Max(2, items.Length * 2);
        TodoItem[] newItems = new TodoItem[newSize];

        for (int i = 0; i < items.Length; i++)
        {
            newItems[i] = items[i];
        }

        return newItems;
    }

    public int Count
    {
        get { return count; }
    }

    public TodoItem GetItem(int index)
    {
        if (index < 1 || index > count)
        {
            throw new ArgumentException("Индекс вне диапазона.");
        }
        return items[index - 1];
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

