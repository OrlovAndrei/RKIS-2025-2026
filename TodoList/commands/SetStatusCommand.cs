namespace TodoList.commands;

public class SetStatusCommand : ICommand
{
    public required string[] parts { get; set; }
    public TodoItem StatusItem { get; private set; }
    public TodoStatus OldStatus { get; private set; }
    public TodoStatus NewStatus { get; private set; }
    public Guid UserId { get; private set; }

    public void Execute()
    {
        if (!AppInfo.CurrentProfileId.HasValue)
        {
            Console.WriteLine("Ошибка: нет активного профиля");
            return;
        }
        
        UserId = AppInfo.CurrentProfileId.Value;
        
        if (parts.Length < 2 || !int.TryParse(parts[1], out var taskNumber))
        {
            Console.WriteLine("Ошибка: укажите номер задачи");
            return;
        }

        var index = taskNumber - 1;
        try
        {
            var todoList = AppInfo.GetCurrentTodoList();
            StatusItem = todoList.GetItem(index);
            OldStatus = StatusItem.Status;
            NewStatus = Enum.Parse<TodoStatus>(parts[2], true);
            
            StatusItem.SetStatus(NewStatus);
            Console.WriteLine($"Поставлен новый статус({NewStatus}) для задачи '{StatusItem.Text}'");
            AppInfo.UndoStack.Push(this);
            FileManager.SaveTodos(UserId, todoList);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    public void Unexecute()
    {
        if (StatusItem != null && AppInfo.TodosByUser.ContainsKey(UserId))
        {
            StatusItem.SetStatus(OldStatus);
            Console.WriteLine($"Отменена смена статуса. Восстановлен статус: {OldStatus}");
            FileManager.SaveTodos(UserId, AppInfo.TodosByUser[UserId]);
        }
    }
}