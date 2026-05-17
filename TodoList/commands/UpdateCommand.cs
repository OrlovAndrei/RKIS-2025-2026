using TodoList.Exceptions;
using TodoList.Services;

namespace TodoList.commands;

public class UpdateCommand : ICommand
{
    public required string[] parts { get; set; }
    public int UpdatedId { get; private set; }
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

        if (!int.TryParse(parts[1], out var taskNumber) || taskNumber <= 0)
            throw new InvalidArgumentException($"Некорректный номер задачи: '{parts[1]}'.");

        var todoList = AppInfo.GetCurrentTodoList();
        if (taskNumber > todoList.items.Count)
            throw new TaskNotFoundException($"Задача с номером {taskNumber} не найдена.");

        var item = todoList.items[taskNumber - 1];
        UpdatedId = item.Id;
        OldText = item.Text;
        NewText = string.Join(" ", parts, 2, parts.Length - 2);

        if (string.IsNullOrWhiteSpace(NewText))
            throw new InvalidArgumentException("Новый текст задачи не может быть пустым.");

        if (NewText.StartsWith("\"") && NewText.EndsWith("\""))
            NewText = NewText.Substring(1, NewText.Length - 2);

        item.UpdateText(NewText);
        AppInfo.TodoRepo.Update(item);
        Console.WriteLine($"Задача обновлена: '{item.Text}'");
        AppInfo.UndoStack.Push(this);
    }

    public void Unexecute()
    {
        if (UpdatedId != 0 && OldText != null && AppInfo.CurrentProfileId.HasValue)
        {
            var item = AppInfo.TodoRepo.GetById(UpdatedId);
            if (item != null)
            {
                item.UpdateText(OldText);
                AppInfo.TodoRepo.Update(item);
                var todoList = AppInfo.GetCurrentTodoList();
                var index = todoList.items.FindIndex(i => i.Id == UpdatedId);
                if (index >= 0) todoList.items[index].UpdateText(OldText);
                Console.WriteLine($"Отменено обновление задачи. Восстановлен текст: '{OldText}'");
            }
        }
    }
}