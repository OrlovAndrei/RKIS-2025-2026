using System;

public class TodoItem
{
    private string title;
    private string description;
    private bool isDone;
    private DateTime dueDate;
    
    public string Title
    {
        get { return title; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Название не может быть пустым");
            title = value;
        }
    }
    
    public string Description
    {
        get { return description; }
        set { description = value ?? string.Empty; }
    }
    
    public bool IsDone
    {
        get { return isDone; }
        set { isDone = value; }
    }
    
    public DateTime DueDate
    {
        get { return dueDate; }
        set { dueDate = value; }
    }
    
    public TodoItem(string title, string description, DateTime dueDate)
    {
        Title = title;
        Description = description;
        IsDone = false;
        DueDate = dueDate;
    }
    
    public TodoItem(string title, DateTime dueDate) : this(title, "", dueDate)
    {
    }
    
    public void MarkDone()
    {
        IsDone = true;
    }
    
    public void MarkUndone()
    {
        IsDone = false;
    }
    
    public void UpdateText(string newTitle, string newDescription = null)
    {
        Title = newTitle;
        if (newDescription != null)
        {
            Description = newDescription;
        }
    }
    
    public string GetFullInfo()
    {
        string status = IsDone ? "Выполнено" : "Не выполнено";
        string overdue = IsOverdue() ? " (ПРОСРОЧЕНО)" : "";
        return $"Задача: {Title}\n" +
               $"Описание: {Description}\n" +
               $"Статус: {status}{overdue}\n" +
               $"Срок выполнения: {DueDate:dd.MM.yyyy}";
    }
    
    public bool IsOverdue()
    {
        return !IsDone && DueDate < DateTime.Now;
    }
    
    public override string ToString()
    {
        string status = IsDone ? "[X]" : "[ ]";
        return $"{status} {Title} - {DueDate:dd.MM.yyyy}";
    }
}