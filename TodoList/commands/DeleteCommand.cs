namespace TodoList.commands;

public class DeleteCommand : ICommand
{
    public required string[] parts { get; set; }
    public TodoItem DeletedItem { get; private set; }
    public int DeletedIndex { get; private set; }
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
            DeletedItem = todoList.GetItem(index);
            DeletedIndex = index;
            todoList.Delete(index);
            Console.WriteLine($"Задача удалена: {DeletedItem.Text}");
            AppInfo.UndoStack.Push(this);
            FileManager.SaveTodos(UserId, todoList);
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine("Ошибка: неверный номер задачи");
        }
    }

    public void Unexecute()
    {
        if (DeletedItem != null && AppInfo.TodosByUser.ContainsKey(UserId))
        {
            AppInfo.TodosByUser[UserId].items.Insert(DeletedIndex, DeletedItem);
            Console.WriteLine($"Отменено удаление задачи: {DeletedItem.Text}");
            FileManager.SaveTodos(UserId, AppInfo.TodosByUser[UserId]);
        }
    }
}