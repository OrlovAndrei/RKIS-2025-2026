using TodoList.Exceptions;
using TodoList.Services;

namespace TodoList.commands;

public class DeleteCommand : ICommand
{
    public required string[] parts { get; set; }
    public int DeletedId { get; private set; }
    public string? DeletedText { get; private set; }
    public Guid UserId { get; private set; }

    public void Execute()
    {
        if (!AppInfo.CurrentProfileId.HasValue)
            throw new AuthenticationException("Необходимо войти в профиль для удаления задач.");

        UserId = AppInfo.CurrentProfileId.Value;

        if (parts.Length < 2)
            throw new InvalidArgumentException("Укажите номер задачи. Использование: delete <номер>");

        if (!int.TryParse(parts[1], out var taskNumber) || taskNumber <= 0)
            throw new InvalidArgumentException($"Некорректный номер задачи: '{parts[1]}'.");

        var todoList = AppInfo.GetCurrentTodoList();
        if (taskNumber > todoList.items.Count)
            throw new TaskNotFoundException($"Задача с номером {taskNumber} не найдена.");

        var item = todoList.items[taskNumber - 1];
        DeletedId = item.Id;
        DeletedText = item.Text;

        AppInfo.TodoRepo.Delete(DeletedId);
        todoList.items.RemoveAt(taskNumber - 1);

        Console.WriteLine($"Задача удалена: {DeletedText}");
        AppInfo.UndoStack.Push(this);
    }

    public void Unexecute()
    {
        if (DeletedId != 0 && DeletedText != null && AppInfo.CurrentProfileId.HasValue)
        {
            var restoredItem = new TodoItem(DeletedText, AppInfo.CurrentProfileId.Value);
            restoredItem.Id = DeletedId;
            AppInfo.TodoRepo.Add(restoredItem);
            var todoList = AppInfo.GetCurrentTodoList();
            todoList.items.Insert(DeletedId - 1, restoredItem);
            Console.WriteLine($"Отменено удаление задачи: {DeletedText}");
        }
    }
}