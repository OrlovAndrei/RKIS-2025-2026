using TodoList.Exceptions;
using TodoList.Services;

namespace TodoList.commands;

public class ReadCommand : ICommand
{
    public required string[] parts { get; set; }

    public void Execute()
    {
        if (!AppInfo.CurrentProfileId.HasValue)
            throw new AuthenticationException("Необходимо войти в профиль для чтения задачи.");

        if (parts.Length < 2)
            throw new InvalidArgumentException("Укажите номер задачи. Использование: read <номер>");

        if (!int.TryParse(parts[1], out var taskNumber) || taskNumber <= 0)
            throw new InvalidArgumentException($"Некорректный номер задачи: '{parts[1]}'.");

        var todoList = AppInfo.GetCurrentTodoList();
        if (taskNumber > todoList.items.Count)
            throw new TaskNotFoundException($"Задача с номером {taskNumber} не найдена.");

        var item = todoList.items[taskNumber - 1];
        Console.WriteLine($"Задача {taskNumber}:");
        Console.WriteLine(item.GetFullInfo());
    }

    public void Unexecute() { }
}