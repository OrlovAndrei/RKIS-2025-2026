using System;

public class TodoItem
{
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsDone { get; set; }
    public DateTime DueDate { get; set; }
    
    public TodoItem(string title, string description, DateTime dueDate)
    {
        Title = title;
        Description = description;
        IsDone = false;
        DueDate = dueDate;
    }
}

public class TodoList
{
    // Приватное поле: массив задач
    private TodoItem[] items;
    private int count;
    
    public TodoList(int initialCapacity = 10)
    {
        items = new TodoItem[initialCapacity];
        count = 0;
    }
    
    // Add(TodoItem item) - добавить задачу
    public void Add(TodoItem item)
    {
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
    
    // Delete(int index) — удалить задачу
    public void Delete(int index)
    {
        if (index < 0 || index >= count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Индекс находится вне диапазона");
        }
        
        // Сдвигаем элементы массива
        for (int i = index; i < count - 1; i++)
        {
            items[i] = items[i + 1];
        }
        
        items[count - 1] = null;
        count--;
    }
    
    // View(bool showIndex, bool showDone, bool showDate) - вывод задач в виде таблицы
    public void View(bool showIndex = true, bool showDone = true, bool showDate = true)
    {
        if (count == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }
        
        // Определяем ширину колонок
        int indexWidth = showIndex ? 8 : 0;
        int titleWidth = 20;
        int descriptionWidth = 30;
        int statusWidth = showDone ? 12 : 0;
        int dateWidth = showDate ? 15 : 0;
        
        // Выводим заголовок таблицы
        Console.WriteLine(new string('-', indexWidth + titleWidth + descriptionWidth + statusWidth + dateWidth + 5));
        
        string header = "";
        if (showIndex) header += "Индекс".PadRight(indexWidth) + "|";
        header += "Заголовок".PadRight(titleWidth) + "|";
        header += "Описание".PadRight(descriptionWidth) + "|";
        if (showDone) header += "Статус".PadRight(statusWidth) + "|";
        if (showDate) header += "Дата".PadRight(dateWidth);
        
        Console.WriteLine(header);
        Console.WriteLine(new string('-', indexWidth + titleWidth + descriptionWidth + statusWidth + dateWidth + 5));
        
        // Выводим задачи
        for (int i = 0; i < count; i++)
        {
            string row = "";
            
            if (showIndex) row += i.ToString().PadRight(indexWidth) + "|";
            row += (items[i].Title.Length > titleWidth - 2 ? 
                   items[i].Title.Substring(0, titleWidth - 3) + "..." : 
                   items[i].Title).PadRight(titleWidth) + "|";
            row += (items[i].Description.Length > descriptionWidth - 2 ? 
                   items[i].Description.Substring(0, descriptionWidth - 3) + "..." : 
                   items[i].Description).PadRight(descriptionWidth) + "|";
            if (showDone) row += (items[i].IsDone ? "Выполнено" : "Не выполнено").PadRight(statusWidth) + "|";
            if (showDate) row += items[i].DueDate.ToString("dd.MM.yyyy").PadRight(dateWidth);
            
            Console.WriteLine(row);
        }
        
        Console.WriteLine(new string('-', indexWidth + titleWidth + descriptionWidth + statusWidth + dateWidth + 5));
    }
    
    // GetItem(int index) — получить задачу по индексу
    public TodoItem GetItem(int index)
    {
        if (index < 0 || index >= count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Индекс находится вне диапазона");
        }
        
        return items[index];
    }
    
    // private IncreaseArray(TodoItem[] items, TodoItem item) - увеличение размера массива
    private void IncreaseArray(TodoItem[] oldItems, TodoItem newItem)
    {
        // Создаем новый массив в 1.5 раза больше
        int newSize = (int)(oldItems.Length * 1.5) + 1;
        TodoItem[] newItems = new TodoItem[newSize];
        
        // Копируем старые элементы
        for (int i = 0; i < oldItems.Length; i++)
        {
            newItems[i] = oldItems[i];
        }
        
        // Добавляем новый элемент
        newItems[count] = newItem;
        count++;
        
        // Заменяем старый массив новым
        items = newItems;
    }
    
    // Дополнительное свойство для получения количества задач
    public int Count => count;
}