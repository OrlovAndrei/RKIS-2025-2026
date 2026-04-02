using System;

namespace TodoList;

public class DeleteCommand : ICommand
{
    private readonly TodoList _todoList;
    private readonly int _index;
    private readonly string _todoFilePath;

    public DeleteCommand(TodoList todoList, int index, string todoFilePath)
    {
        _todoList = todoList;
        _index = index;
        _todoFilePath = todoFilePath;
    }

    public void Execute()
    {
        _todoList.Delete(_index);
        Console.WriteLine($"Задача {_index + 1} удалена.");
        FileManager.SaveTodos(_todoList, _todoFilePath);
    }
}