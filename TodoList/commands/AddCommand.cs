using TodoList.Exceptions;

namespace TodoList.commands;

public class AddCommand : ICommand
{
    public required string[] parts { get; set; }
    public bool multiline { get; set; }
    public TodoItem? AddedItem { get; private set; }
    public Guid UserId { get; private set; }

    public void Execute()
    {
        if (!AppInfo.CurrentProfileId.HasValue)
            throw new AuthenticationException("Необходимо войти в профиль для добавления задач.");
        
        UserId = AppInfo.CurrentProfileId.Value;
        
        if (multiline)
            AddMultilineTask();
        else
        {
            if (parts.Length < 2)
                throw new InvalidArgumentException("Укажите текст задачи. Использование: add <текст>");

            var taskText = string.Join(" ", parts, 1, parts.Length - 1);
            AddSingleTask(taskText);
        }
        
        AppInfo.UndoStack.Push(this);
    }

    private void AddSingleTask(string taskText)
    {
        if (string.IsNullOrWhiteSpace(taskText))
            throw new InvalidArgumentException("Текст задачи не может быть пустым.");

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
        AddSingleTask(taskText);
    }

    public void Unexecute()
    {
        if (AddedItem != null && AppInfo.TodosByUser.ContainsKey(UserId))
        {
            AppInfo.TodosByUser[UserId].Remove(AddedItem);
            Console.WriteLine($"Отменено добавление задачи: {AddedItem.Text}");
        }
    }
}