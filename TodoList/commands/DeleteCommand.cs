using TodoList.Exceptions;

namespace TodoList.commands;

public class DeleteCommand : ICommand
{
    public required string[] parts { get; set; }
    public TodoItem? DeletedItem { get; private set; }
    public int DeletedIndex { get; private set; }
    public Guid UserId { get; private set; }

    public void Execute()
    {
        if (!AppInfo.CurrentProfileId.HasValue)
            throw new AuthenticationException("Необходимо войти в профиль для удаления задач.");
        
        UserId = AppInfo.CurrentProfileId.Value;
        
        if (parts.Length < 2)
            throw new InvalidArgumentException("Укажите номер задачи. Использование: delete <номер>");

        if (!int.TryParse(parts[1], out var taskNumber))
            throw new InvalidArgumentException($"Некорректный номер задачи: '{parts[1]}'. Ожидается целое число.");

        var index = taskNumber - 1;
        var todoList = AppInfo.GetCurrentTodoList();
        
        if (index < 0 || index >= todoList.items.Count)
            throw new TaskNotFoundException($"Задача с номером {taskNumber} не найдена.");

        DeletedItem = todoList.GetItem(index);
        DeletedIndex = index;
        todoList.Delete(index);
        Console.WriteLine($"Задача удалена: {DeletedItem.Text}");
        AppInfo.UndoStack.Push(this);
    }

    public void Unexecute()
    {
        if (DeletedItem != null && AppInfo.TodosByUser.ContainsKey(UserId))
        {
            AppInfo.TodosByUser[UserId].Insert(DeletedIndex, DeletedItem);
            Console.WriteLine($"Отменено удаление задачи: {DeletedItem.Text}");
        }
    }
}