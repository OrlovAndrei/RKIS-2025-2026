using TodoList.Exceptions;

namespace TodoList.commands;

public class UpdateCommand : ICommand
{
    public required string[] parts { get; set; }
    public TodoItem? UpdatedItem { get; private set; }
    public string? OldText { get; private set; }
    public string? NewText { get; private set; }
    public Guid UserId { get; private set; }

    public void Execute()
    {
        if (!AppInfo.CurrentProfileId.HasValue)
            throw new AuthenticationException("Необходимо войти в профиль для обновления задач.");
        
        UserId = AppInfo.CurrentProfileId.Value;
        
        if (parts.Length < 3)
            throw new InvalidArgumentException("Укажите номер задачи и новый текст. Использование: update <номер> \"<новый текст>\"");

        if (!int.TryParse(parts[1], out var taskNumber))
            throw new InvalidArgumentException($"Некорректный номер задачи: '{parts[1]}'. Ожидается целое число.");

        var index = taskNumber - 1;
        var todoList = AppInfo.GetCurrentTodoList();

        if (index < 0 || index >= todoList.items.Count)
            throw new TaskNotFoundException($"Задача с номером {taskNumber} не найдена.");

        UpdatedItem = todoList.GetItem(index);
        OldText = UpdatedItem.Text;
        NewText = string.Join(" ", parts, 2, parts.Length - 2);
        
        if (string.IsNullOrWhiteSpace(NewText))
            throw new InvalidArgumentException("Новый текст задачи не может быть пустым.");

        if (NewText.StartsWith("\"") && NewText.EndsWith("\""))
            NewText = NewText.Substring(1, NewText.Length - 2);
        
        UpdatedItem.UpdateText(NewText);
        Console.WriteLine($"Задача обновлена: '{UpdatedItem.Text}'");
        todoList.NotifyItemUpdated(UpdatedItem);
        AppInfo.UndoStack.Push(this);
    }

    public void Unexecute()
    {
        if (UpdatedItem != null && OldText != null && AppInfo.TodosByUser.ContainsKey(UserId))
        {
            UpdatedItem.UpdateText(OldText);
            Console.WriteLine($"Отменено обновление задачи. Восстановлен текст: '{OldText}'");
            AppInfo.TodosByUser[UserId].NotifyItemUpdated(UpdatedItem);
        }
    }
}