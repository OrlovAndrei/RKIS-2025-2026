namespace TodoList.commands;

public class AddCommand : ICommand
{
    public required string[] parts { get; set; }
    public bool multiline { get; set; }
    public TodoItem AddedItem { get; private set; }
    public Guid UserId { get; private set; }

    public void Execute()
    {
        if (!AppInfo.CurrentProfileId.HasValue)
        {
            Console.WriteLine("Ошибка: нет активного профиля");
            return;
        }
        
        UserId = AppInfo.CurrentProfileId.Value;
        
        if (multiline)
            AddMultilineTask();
        else
        {
            if (parts.Length < 2)
            {
                Console.WriteLine("Ошибка: укажите текст задачи");
                return;
            }

            var taskText = string.Join(" ", parts, 1, parts.Length - 1);
            AddSingleTask(taskText);
        }
        
        AppInfo.UndoStack.Push(this);
        FileManager.SaveTodos(UserId, AppInfo.GetCurrentTodoList());
    }

    private void AddSingleTask(string taskText)
    {
        if (string.IsNullOrWhiteSpace(taskText))
        {
            Console.WriteLine("Ошибка: текст задачи не может быть пустым");
            return;
        }

        AddedItem = new TodoItem(taskText);
        AppInfo.GetCurrentTodoList().Add(AddedItem);
        Console.WriteLine($"Задача добавлена: {taskText}");
    }

    private void AddMultilineTask()
    {
        Console.WriteLine("Введите текст задачи (для завершения введите 'end'):");
        var taskText = "";
        while (true)
        {
            Console.Write("> ");
            var line = Console.ReadLine();
            if (line == null)
                continue;
            if (line == "end")
                break;
            taskText += line + "\n";
        }

        taskText = taskText.TrimEnd('\n');
        if (string.IsNullOrWhiteSpace(taskText))
        {
            Console.WriteLine("Ошибка: текст задачи не может быть пустым");
            return;
        }

        AddSingleTask(taskText);
    }

    public void Unexecute()
    {
        if (AddedItem != null && AppInfo.TodosByUser.ContainsKey(UserId))
        {
            AppInfo.TodosByUser[UserId].items.Remove(AddedItem);
            Console.WriteLine($"Отменено добавление задачи: {AddedItem.Text}");
            FileManager.SaveTodos(UserId, AppInfo.TodosByUser[UserId]);
        }
    }
}