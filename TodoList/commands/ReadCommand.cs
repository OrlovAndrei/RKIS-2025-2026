using TodoList.Exceptions;

namespace TodoList.commands;

public class ReadCommand : ICommand
{
    public required string[] parts { get; set; }

    public void Execute()
    {
        if (!AppInfo.CurrentProfileId.HasValue)
            throw new AuthenticationException("Необходимо войти в профиль для просмотра задач.");
        
        if (parts.Length < 2)
            throw new InvalidArgumentException("Укажите номер задачи. Использование: read <номер>");

        if (!int.TryParse(parts[1], out var taskNumber))
            throw new InvalidArgumentException($"Некорректный номер задачи: '{parts[1]}'. Ожидается целое число.");

        var index = taskNumber - 1;
        var todoList = AppInfo.GetCurrentTodoList();

        if (index < 0 || index >= todoList.items.Count)
            throw new TaskNotFoundException($"Задача с номером {taskNumber} не найдена.");

        var item = todoList.GetItem(index);
        Console.WriteLine($"Задача {taskNumber}:");
        Console.WriteLine(item.GetFullInfo());
    }

    public void Unexecute() { }
}