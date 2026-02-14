namespace TodoList.commands;

public class UpdateCommand : ICommand
{
    public required string[] parts { get; set; }
    public TodoItem UpdatedItem { get; private set; }
    public string OldText { get; private set; }
    public string NewText { get; private set; }
    public Guid UserId { get; private set; }

    public void Execute()
    {
        if (!AppInfo.CurrentProfileId.HasValue)
        {
            Console.WriteLine("Ошибка: нет активного профиля");
            return;
        }
        
        UserId = AppInfo.CurrentProfileId.Value;
        
        if (parts.Length < 3)
        {
            Console.WriteLine("Ошибка: укажите номер и новый текст задачи");
            return;
        }

        if (!int.TryParse(parts[1], out var taskNumber))
        {
            Console.WriteLine("Ошибка: неверный номер задачи");
            return;
        }

        var index = taskNumber - 1;
        try
        {
            var todoList = AppInfo.GetCurrentTodoList();
            UpdatedItem = todoList.GetItem(index);
            OldText = UpdatedItem.Text;
            NewText = string.Join(" ", parts, 2, parts.Length - 2);
            
            if (string.IsNullOrWhiteSpace(NewText))
            {
                Console.WriteLine("Ошибка: новый текст задачи не может быть пустым");
                return;
            }

            if (NewText.StartsWith("\"") && NewText.EndsWith("\""))
                NewText = NewText.Substring(1, NewText.Length - 2);
            
            UpdatedItem.UpdateText(NewText);
            Console.WriteLine($"Задача обновлена: '{UpdatedItem.Text}'");
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
        if (UpdatedItem != null && OldText != null && AppInfo.TodosByUser.ContainsKey(UserId))
        {
            UpdatedItem.UpdateText(OldText);
            Console.WriteLine($"Отменено обновление задачи. Восстановлен текст: '{OldText}'");
            FileManager.SaveTodos(UserId, AppInfo.TodosByUser[UserId]);
        }
    }
}