using System;

class AddCommand : ICommand
{
    public TodoList TodoList { get; set; }
    public string Text { get; set; }

    public void Execute()
    {
        if (string.IsNullOrWhiteSpace(Text))
        {
            Console.WriteLine("add <текст задачи>");
            return;
        }

        TodoList.Add(new TodoItem(Text));
    }
}

