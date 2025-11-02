using System;

public class AddCommand : ICommand
{
    public bool IsMultiline { get; set; }
    public string Text { get; set; }
    public TodoList TodoList { get; set; }

    public void Execute()
    {
        if (IsMultiline)
        {
            AddTodoMultiline();
        }
        else
        {
            AddTodoSingleLine();
        }
    }

    private void AddTodoSingleLine()
    {
        if (string.IsNullOrWhiteSpace(Text))
        {
            Console.WriteLine("Текст задачи не может быть пустым.");
            return;
        }

        TodoItem newItem = new TodoItem(Text);
        TodoList.Add(newItem);
        Console.WriteLine($"Задача добавлена: {Text} (всего задач: {TodoList.Count})");
    }

    private void AddTodoMultiline()
    {
        Console.WriteLine("Введите текст задачи (для завершения введите !end):");
        
        string multilineText = "";
        while (true)
        {
            Console.Write("> ");
            string line = Console.ReadLine();
            
            if (line == null)
                continue;

            if (line == "!end")
                break;
                
            multilineText += line + "\n";
        }
        
        multilineText = multilineText.Trim();
        if (string.IsNullOrWhiteSpace(multilineText))
        {
            Console.WriteLine("Текст задачи не может быть пустым.");
            return;
        }
        
        TodoItem newItem = new TodoItem(multilineText);
        TodoList.Add(newItem);
        Console.WriteLine($"Многострочная задача добавлена (всего задач: {TodoList.Count})");
    }
}