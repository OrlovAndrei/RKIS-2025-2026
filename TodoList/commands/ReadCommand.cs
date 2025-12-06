namespace TodoList.commands;

public class ReadCommand : ICommand
{
    public required string[] parts { get; set; }

    public void Execute()
    {
        if (!AppInfo.CurrentProfileId.HasValue)
        {
            Console.WriteLine("Ошибка: нет активного профиля");
            return;
        }
        
        if (parts.Length < 2 || !int.TryParse(parts[1], out var taskNumber))
        {
            Console.WriteLine("Ошибка: укажите номер задачи");
            return;
        }

        var index = taskNumber - 1;
        try
        {
            var todoList = AppInfo.GetCurrentTodoList();
            var item = todoList.GetItem(index);
            Console.WriteLine($"Задача {taskNumber}:");
            Console.WriteLine(item.GetFullInfo());
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine("Ошибка: неверный номер задачи");
        }
    }

    public void Unexecute()
    {
        // Команда read не требует отмены
    }
}