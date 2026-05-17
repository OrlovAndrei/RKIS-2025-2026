using TodoList.Exceptions;
using TodoList.Services;

namespace TodoList.commands;

public class SetStatusCommand : ICommand
{
    public required string[] parts { get; set; }
    public int UpdatedId { get; private set; }
    public TodoStatus OldStatus { get; private set; }
    public TodoStatus NewStatus { get; private set; }
    public Guid UserId { get; private set; }

    public void Execute()
    {
        if (!AppInfo.CurrentProfileId.HasValue)
            throw new AuthenticationException("Необходимо войти в профиль для изменения статуса.");

        UserId = AppInfo.CurrentProfileId.Value;

        if (parts.Length < 3)
            throw new InvalidArgumentException("Укажите номер задачи и новый статус. Использование: setstatus <номер> <статус>");

        if (!int.TryParse(parts[1], out var taskNumber) || taskNumber <= 0)
            throw new InvalidArgumentException($"Некорректный номер задачи: '{parts[1]}'.");

        var todoList = AppInfo.GetCurrentTodoList();
        if (taskNumber > todoList.items.Count)
            throw new TaskNotFoundException($"Задача с номером {taskNumber} не найдена.");

        var item = todoList.items[taskNumber - 1];
        UpdatedId = item.Id;
        OldStatus = item.Status;

        if (!Enum.TryParse<TodoStatus>(parts[2], true, out var newStatus))
            throw new InvalidArgumentException($"Неверный статус '{parts[2]}'. Допустимые значения: {string.Join(", ", Enum.GetNames<TodoStatus>())}.");

        NewStatus = newStatus;
        item.SetStatus(NewStatus);
        AppInfo.TodoRepo.Update(item);
        Console.WriteLine($"Поставлен новый статус ({NewStatus}) для задачи '{item.Text}'");
        AppInfo.UndoStack.Push(this);
    }

    public void Unexecute()
    {
        if (UpdatedId != 0 && AppInfo.CurrentProfileId.HasValue)
        {
            var item = AppInfo.TodoRepo.GetById(UpdatedId);
            if (item != null)
            {
                item.SetStatus(OldStatus);
                AppInfo.TodoRepo.Update(item);
                var todoList = AppInfo.GetCurrentTodoList();
                var index = todoList.items.FindIndex(i => i.Id == UpdatedId);
                if (index >= 0) todoList.items[index].SetStatus(OldStatus);
                Console.WriteLine($"Отменена смена статуса. Восстановлен статус: {OldStatus}");
            }
        }
    }
}