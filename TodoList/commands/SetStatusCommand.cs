using TodoList.Exceptions;

namespace TodoList.commands;

public class SetStatusCommand : ICommand
{
    public required string[] parts { get; set; }
    public TodoItem? StatusItem { get; private set; }
    public TodoStatus OldStatus { get; private set; }
    public TodoStatus NewStatus { get; private set; }
    public Guid UserId { get; private set; }

    public void Execute()
    {
        if (!AppInfo.CurrentProfileId.HasValue)
            throw new AuthenticationException("Необходимо войти в профиль для изменения статуса задач.");
        
        UserId = AppInfo.CurrentProfileId.Value;
        
        if (parts.Length < 3)
            throw new InvalidArgumentException("Укажите номер задачи и новый статус. Использование: setstatus <номер> <статус>");

        if (!int.TryParse(parts[1], out var taskNumber))
            throw new InvalidArgumentException($"Некорректный номер задачи: '{parts[1]}'. Ожидается целое число.");

        var index = taskNumber - 1;
        var todoList = AppInfo.GetCurrentTodoList();

        if (index < 0 || index >= todoList.items.Count)
            throw new TaskNotFoundException($"Задача с номером {taskNumber} не найдена.");

        StatusItem = todoList.GetItem(index);
        OldStatus = StatusItem.Status;

        if (!Enum.TryParse<TodoStatus>(parts[2], true, out var newStatus))
            throw new InvalidArgumentException($"Неверный статус '{parts[2]}'. Допустимые значения: {string.Join(", ", Enum.GetNames<TodoStatus>())}.");

        NewStatus = newStatus;
        StatusItem.SetStatus(NewStatus);
        Console.WriteLine($"Поставлен новый статус({NewStatus}) для задачи '{StatusItem.Text}'");
        todoList.NotifyStatusChanged(StatusItem);
        AppInfo.UndoStack.Push(this);
    }

    public void Unexecute()
    {
        if (StatusItem != null && AppInfo.TodosByUser.ContainsKey(UserId))
        {
            StatusItem.SetStatus(OldStatus);
            Console.WriteLine($"Отменена смена статуса. Восстановлен статус: {OldStatus}");
            AppInfo.TodosByUser[UserId].NotifyStatusChanged(StatusItem);
        }
    }
}