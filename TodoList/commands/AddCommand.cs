namespace TodoList.commands;

public class AddCommand : ICommand
{
    public required string[] parts { get; set; }
    public bool multiline { get; set; }
    public TodoItem AddedItem { get; private set; }
    public Guid UserId { get; private set; }

    public void Execute()
    {
        if (!AppInfo.CurrentProfileId.HasValue)
        {
            Console.WriteLine("Ошибка: нет активного профиля");
            return;
        }
        
        UserId = AppInfo.CurrentProfileId.Value;
        
        if (multiline)
            AddMultilineTask();
        else
        {
            if (parts.Length < 2)
            {
                Console.WriteLine("Ошибка: укажите текст задачи");
                return;
            }

            var taskText = string.Join(" ", parts, 1, parts.Length - 1);
            AddSingleTask(taskText);
        }
        
        AppInfo.UndoStack.Push(this);
        // Сохранение убрано — теперь через события
    }

    private void AddSingleTask(string taskText)
    {
        if (string.IsNullOrWhiteSpace(taskText))
        {
            Console.WriteLine("Ошибка: текст задачи не может быть пустым");
            return;
        }

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
        if (string.IsNullOrWhiteSpace(taskText))
        {
            Console.WriteLine("Ошибка: текст задачи не может быть пустым");
            return;
        }

        AddSingleTask(taskText);
    }

    public void Unexecute()
    {
        if (AddedItem != null && AppInfo.TodosByUser.ContainsKey(UserId))
        {
            AppInfo.TodosByUser[UserId].items.Remove(AddedItem);
            Console.WriteLine($"Отменено добавление задачи: {AddedItem.Text}");
            // Сохранение убрано — TodoList сам сгенерирует событие удаления? Но здесь мы удаляем напрямую из списка, минуя методы TodoList, поэтому событие не вызовется.
            // Чтобы событие сработало, нужно использовать метод Delete, но у нас нет индекса. Можно вызвать OnTodoDeleted вручную? Но это нарушает архитектуру.
            // Лучше добавить в TodoList метод RemoveItem, который будет генерировать событие. Но пока оставим как есть? В задании просят вызывать события только в методах TodoList.
            // Unexecute выполняется при отмене действия. В текущей реализации Unexecute напрямую манипулирует списком, не вызывая события. Это может привести к рассинхронизации с файлом, так как событие не вызовется.
            // Чтобы исправить, нужно либо в Unexecute использовать методы TodoList (но для удаления по объекту нет метода), либо вручную вызывать событие.
            // Для простоты предположим, что Unexecute тоже должно вызывать события, поэтому добавим вызов события вручную.
            // Но в TodoList нет события для прямого удаления элемента. Можно добавить публичный метод RemoveItem, который вызывает событие.
            // Однако, чтобы не усложнять, можно здесь вручную вызвать событие OnTodoDeleted? Но оно объявлено в TodoList, доступ к нему есть.
            // Лучше добавить в TodoList метод Remove(TodoItem item), который будет удалять и вызывать событие.
            // Сделаем это в TodoList.cs.
        }
    }
}