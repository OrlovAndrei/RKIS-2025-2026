using System;

public class TodoList
{
    private TodoItem[] items;
    private int count;
    
    public int Count
    {
        get { return count; }
    }
    
    public TodoList(int initialCapacity = 10)
    {
        if (initialCapacity <= 0)
            throw new ArgumentException("Емкость должна быть положительным числом");
        
        items = new TodoItem[initialCapacity];
        count = 0;
    }
    
    public void Add(TodoItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));
        
        if (count >= items.Length)
        {
            IncreaseArray(items, item);
        }
        else
        {
            items[count] = item;
            count++;
        }
    }
    
    public void Delete(int index)
    {
        if (index < 0 || index >= count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Индекс находится вне диапазона");
        }
        
        for (int i = index; i < count - 1; i++)
        {
            items[i] = items[i + 1];
        }
        
        items[count - 1] = null;
        count--;
    }
    
    public void View(bool showIndex = true, bool showDone = true, bool showDate = true)
    {
        if (count == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }
        
        int indexWidth = showIndex ? 8 : 0;
        int titleWidth = 20;
        int descriptionWidth = 30;
        int statusWidth = showDone ? 12 : 0;
        int dateWidth = showDate ? 15 : 0;
        
        int totalWidth = indexWidth + titleWidth + descriptionWidth + statusWidth + dateWidth + 4;
        Console.WriteLine(new string('-', totalWidth));
        
        string header = "";
        if (showIndex) header += "Индекс".PadRight(indexWidth) + "|";
        header += "Заголовок".PadRight(titleWidth) + "|";
        header += "Описание".PadRight(descriptionWidth) + "|";
        if (showDone) header += "Статус".PadRight(statusWidth) + "|";
        if (showDate) header += "Дата".PadRight(dateWidth);
        
        Console.WriteLine(header);
        Console.WriteLine(new string('-', totalWidth));
        
        for (int i = 0; i < count; i++)
        {
            string row = "";
            
            if (showIndex) row += i.ToString().PadRight(indexWidth) + "|";
            row += TruncateString(items[i].Title, titleWidth).PadRight(titleWidth) + "|";
            row += TruncateString(items[i].Description, descriptionWidth).PadRight(descriptionWidth) + "|";
            if (showDone) row += (items[i].IsDone ? "Выполнено" : "Не выполнено").PadRight(statusWidth) + "|";
            if (showDate) row += items[i].DueDate.ToString("dd.MM.yyyy").PadRight(dateWidth);
            
            Console.WriteLine(row);
        }
        
        Console.WriteLine(new string('-', totalWidth));
    }
    
    public TodoItem GetItem(int index)
    {
        if (index < 0 || index >= count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Индекс находится вне диапазона");
        }
        
        return items[index];
    }
    
    public bool TryGetItem(int index, out TodoItem item)
    {
        item = null;
        
        if (index < 0 || index >= count)
            return false;
        
        item = items[index];
        return true;
    }
    
    private void IncreaseArray(TodoItem[] oldItems, TodoItem newItem)
    {
        int newSize = (int)(oldItems.Length * 1.5) + 1;
        TodoItem[] newItems = new TodoItem[newSize];
        
        for (int i = 0; i < oldItems.Length; i++)
        {
            newItems[i] = oldItems[i];
        }
        
        newItems[count] = newItem;
        count++;
        items = newItems;
    }
    
    private string TruncateString(string text, int maxLength)
    {
        if (string.IsNullOrEmpty(text) || text.Length <= maxLength - 2)
            return text;
        
        return text.Substring(0, maxLength - 3) + "...";
    }
}